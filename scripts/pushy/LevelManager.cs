using System.Collections.Generic;
using EngineLearning.scripts.grid_game.misc;
using Godot;

namespace EngineLearning.scripts.pushy {
    
    /// <summary>
    /// Manages the current state of the game.
    /// </summary>
    public class LevelManager {
        private int[,] _level = {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 2, 0, 0, 0, 0, 1, 1, 1, 1},
            {1, 0, 0, 0, 8, 8, 0, 16, 16, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            {1, 0, 0, 0, 4, 0, 1, 0, 0, 1},
            {1, 1, 0, 0, 0, 0, 1, 0, 1, 1},
            {1, 0, 0, 1, 1, 1, 1, 0, 1, 1},
            {1, 0, 32, 1, 0, 0, 1, 0, 1, 1},
            {1, 1, 0, 1, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 1, 0, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        };

        private int _numCorrectBalls = 0;
        private int _totalNumCorrectBalls = 2;

        private int _idCounter = 1;

        private IntVec2 _housePosition;
        internal IntVec2 PlayerPosition { get; set; }
        
        /// <summary>
        /// A list of tiles with which the player can interact
        /// and move around.
        /// </summary>
        internal List<Tile> MoveableTiles { get; set; }

        public LevelManager() {

            MoveableTiles = new List<Tile>();
            
            PlayerPosition = new IntVec2(1, 1);
            _housePosition = new IntVec2(4, 6);

            Initialize();
        }

        private void Initialize() {
            for (int y = 0; y < _level.GetLength(0); y++) {
                for (int x = 0; x < _level.GetLength(1); x++) {
                    switch (_level[y,x]) {
                        case (int)TileType.RedBall:
                            _idCounter++;
                            MoveableTiles.Add(new Tile(_idCounter, new IntVec2(x,y), TileType.RedBall));
                            break;
                        case (int)TileType.WoodBox:
                            _idCounter++;
                            MoveableTiles.Add(new Tile(_idCounter, new IntVec2(x,y), TileType.WoodBox));
                            break;
                    }
                }
            }
        }

        internal TileType[,] GetEnumLevel() {
            TileType[,] level = new TileType[_level.GetLength(0), _level.GetLength(1)];

            for (int y = 0; y < _level.GetLength(0); y++) {
                for (int x = 0; x < _level.GetLength(1); x++) {
                    switch (_level[y, x]) {
                        case 0:
                            level[y, x] = TileType.Empty;
                            break;
                        case 1:
                            level[y, x] = TileType.Wall;
                            break;
                        case 2:
                            level[y, x] = TileType.Player;
                            break;
                        case 4:
                            level[y, x] = TileType.House;
                            break;
                        case 8:
                            level[y, x] = TileType.RedBall;
                            break;
                        case 16:
                            level[y, x] = TileType.RedBallHole;
                            break;
                        case 32:
                            level[y, x] = TileType.WoodBox;
                            break;
                    }
                }
            }
            return level;
        }

        /// <summary>
        /// Computes if the current game state is a winning state.
        /// The state is winning if the player is standing at the house
        /// and all balls have been moved to their correct places.
        /// </summary>
        /// <returns>true if the state is won; false otherwise</returns>
        internal bool IsWon() {
            return PlayerPosition == _housePosition
                && _numCorrectBalls == _totalNumCorrectBalls;
        }

        /// <summary>
        /// Updates the game state based on the direction the player wants to move.
        /// </summary>
        internal void UpdateState(Direction dir) {

            var desiredPlayerPos = PlayerPosition + dir.GetOffsetVec();
            
            if (AreValidCoordinates(desiredPlayerPos) && !IsWon()) {

                // If there is no wall then move player
                if (IsValidCell(desiredPlayerPos)) {
                    _level[PlayerPosition.Y, PlayerPosition.X] = (int) TileType.Empty;
                    _level[desiredPlayerPos.Y, desiredPlayerPos.X] = (int) TileType.Player;
                    PlayerPosition = desiredPlayerPos;
                }
                else {
                    // If there is a moveable object then move
                    if (IsMoveable(desiredPlayerPos) && CanBeMoved(desiredPlayerPos, desiredPlayerPos + dir.GetOffsetVec())) {

                        // Set grid to updated values
                        _level[PlayerPosition.Y, PlayerPosition.X] = (int) TileType.Empty;
                        var temp = _level[desiredPlayerPos.Y, desiredPlayerPos.X];
                        _level[desiredPlayerPos.Y, desiredPlayerPos.X] |= (int) TileType.Player;
                        _level[(desiredPlayerPos + dir.GetOffsetVec()).Y, (desiredPlayerPos + dir.GetOffsetVec()).X] |= temp;

                        PlayerPosition = desiredPlayerPos;
                        
                        MoveableTiles.FindAll(tile => tile.Coordinates == desiredPlayerPos)
                            .ForEach(tile => {
                                tile.Updated = false;
                                tile.Coordinates = desiredPlayerPos + dir.GetOffsetVec();
                            });
                    }
                }
            }
        }

        /// <summary>
        /// Determines if an tile can be moved from start position
        /// to target position.
        /// </summary>
        /// <param name="from">The position to move from</param>
        /// <param name="to">The position to move to</param>
        /// <returns>true if it can be moved; false otherwise</returns>
        internal bool CanBeMoved(IntVec2 from, IntVec2 to) {

            if (IsBall(from) && IsHole(to)) {
                _numCorrectBalls++;
                return true;
            }

            if (IsWoodBox(from) && IsEmpty(to)) {
                return true;
            }
            
            if (IsMoveable(from) && (_level[to.Y, to.X] == (int) TileType.Empty)) {
                return true;
            }

            return false;
        }
        
        internal bool IsBall(IntVec2 pos) {
            return (_level[pos.Y, pos.X] & ((int) TileType.RedBall)) != 0;
        }
        
        internal bool IsEmpty(IntVec2 pos) {
            return (_level[pos.Y, pos.X] & ((int) TileType.Empty)) != 0;
        }

        internal bool IsHole(IntVec2 pos) {
            return (_level[pos.Y, pos.X] & ((int) TileType.RedBallHole)) != 0;
        }
        
        internal bool IsWoodBox(IntVec2 pos) {
            return (_level[pos.Y, pos.X] & ((int) TileType.WoodBox)) != 0;
        }

        internal bool IsMoveable(IntVec2 pos) {
            return (IsBall(pos) && !IsHole(pos)) || IsWoodBox(pos);
        }

        internal bool IsWall(IntVec2 pos) {
            return (_level[pos.Y, pos.X] & ((int) TileType.Wall)) != 0;
        }

        internal IntVec2 GetDimension() {
            return new IntVec2(_level.GetLength(1), _level.GetLength(1));
        }
        
        private bool AreValidCoordinates(IntVec2 coords) {
            return (0 <= coords.X && coords.X < _level.GetLength(1))
                   && (0 <= coords.Y && coords.Y < _level.GetLength(0));
        }

        private bool IsValidCell(IntVec2 pos) {
            return !IsHole(pos) && !IsBall(pos) && !IsWall(pos) && !IsWoodBox(pos);
        }
    }
}