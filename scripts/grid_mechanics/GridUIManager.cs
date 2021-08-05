using Godot;
using System;
using EngineLearning.scripts.grid_game.misc;
using EngineLearning.scripts.grid_mechanics;
using Godot.Collections;

public class GridUIManager : Node2D {

    private int _numCellsX = 8;
    private int _numCellsY = 8;

    private int _cellSize = 64;

    private Sprite[,] _cellSprites;

    private Texture _cellTexture;
    private Texture _playerTexture;
    private Texture _rotateRightTile;

    private Dictionary<int, Sprite> moveableSprites;
    private Dictionary<int, Sprite> stationarySprites;

    /*
     * Represents the grid logic.
     * Separate from UI.
     */
    private GridManager _gridManager;


    /*
     * Represents the game time in seconds.
     */
    private float _time;

    public override void _Ready() {
        _cellSprites = new Sprite[_numCellsY, _numCellsX];
        _cellTexture = GD.Load<Texture>("res://textures/grid_game/cell_border.png");
        _playerTexture = GD.Load<Texture>("res://textures/grid_game/player_tile.png");
        _rotateRightTile = GD.Load<Texture>("res://textures/grid_game/rotate_right_tile.png");

        moveableSprites = new Dictionary<int, Sprite>();
        stationarySprites = new Dictionary<int, Sprite>();
        
        InitializeBoard();
        InitializePlayer();
    }

    public override void _Process(float delta) {

        _time += delta;

        if (_time > 1.0f) {
            _time = 0.0f;
        }
        
        // Update UI
        UpdateUI(_time);
    }

    private void UpdateUI(float time) {
        _gridManager.MoveableTiles.ForEach(tile => {
            if (moveableSprites.TryGetValue(tile.GetHashCode(), out var sprite)) {

                var targetPosition = ScreenCoordFromGridCoord(tile.X, tile.Y);
                
                sprite.Position = LinearInterpolation(sprite.Position, targetPosition, _time);
                sprite.Rotation = Mathf.Deg2Rad(tile.Orientation.GetDegreeOrientation());
                

                if (Mathf.Abs(sprite.Position.DistanceTo(targetPosition)) < 1E-3) {
                    sprite.Position = targetPosition;
                }
            }
        });
    }
    
    private Vector2 LinearInterpolation(Vector2 S, Vector2 E, float t) {
        return (1 - t) * S + t * E;
    }

    private float LinearInterpolation(float s, float e, float t) {
        return (1 - t) * s + t * e;
    }

    private Vector2Int NextPosition(Vector2Int current) {
        return new Vector2Int(current.X + 1, current.Y);
    }

    private bool IsInGrid(Vector2Int pos) {
        return (0 <= pos.X && pos.X < _numCellsX) &&
               (0 <= pos.Y && pos.Y < _numCellsY);
    }

    private void InitializePlayer() {
        
        _gridManager.AddTile(TileType.MOVEABLE_TILE, 2, 0);
        _gridManager.AddTile(TileType.MOVEABLE_TILE, 3, 0);

        _gridManager.AddTile(TileType.ROTATE_RIGHT_TILE, 6, 0);
        _gridManager.AddTile(TileType.ROTATE_RIGHT_TILE, 6, 6);
        
        // Initialize moveable tiles
        _gridManager.MoveableTiles.ForEach(tile => {
            var sprite = new Sprite();
            sprite.Position = ScreenCoordFromGridCoord(tile.X, tile.Y);

            switch (tile.Type) {
                case TileType.MOVEABLE_TILE:
                    sprite.Scale = ScaleFromTexture(_playerTexture);
                    sprite.Texture = _playerTexture;
                    sprite.Name = $"player_{tile.GetHashCode()}";
                    break;
                default: break;
            }
            
            AddChild(sprite);
            moveableSprites.Add(tile.GetHashCode(), sprite);
        });
        
        // Initialize stationary tiles
        _gridManager.StationaryTiles.ForEach(tile => {
            var sprite = new Sprite();
            sprite.Position = ScreenCoordFromGridCoord(tile.X, tile.Y);

            switch (tile.Type) {
                case TileType.ROTATE_RIGHT_TILE:
                    sprite.Scale = ScaleFromTexture(_rotateRightTile);
                    sprite.Texture = _rotateRightTile;
                    sprite.Name = $"rotate_right_{tile.GetHashCode()}";
                    break;
                default: break;
            }
            
            AddChild(sprite);
            stationarySprites.Add(tile.GetHashCode(), sprite);
        });
    }
    
    private void InitializeBoard() {

        _gridManager = new GridManager(_numCellsX, _numCellsY);
        
        for (int y = 0; y < _numCellsY; y++) {
            for (int x = 0; x < _numCellsX; x++) {
                Sprite cell = new Sprite();
                cell.Texture = _cellTexture;
                cell.Position = ScreenCoordFromGridCoord(x, y);
                cell.Scale = ScaleFromTexture(_cellTexture);
                cell.Name = $"cell_{y}_{x}";
                
                AddChild(cell);

            }
        }
    }

    /*
     * Computes the screen coordinates (in pixel) from given
     * grid coordinates.
     *
     * x:  The grid x coordinate
     * y:  The grid y coordinate
     */
    private Vector2 ScreenCoordFromGridCoord(int x, int y) {
        Vector2 coord = new Vector2(
            -(_numCellsX / 2) * _cellSize + _cellSize * x,
            -(_numCellsY / 2) * _cellSize + _cellSize * y
            );
        
        return coord;
    }
    
    /*
     * Computes the scale of a sprite given a texutre.
     * The scale is determined by dividing the cell
     * size by the texture size.
     */
    private Vector2 ScaleFromTexture(Texture texture) {
        return new Vector2(
            _cellSize / texture.GetSize().x,
            _cellSize / texture.GetSize().y
            );
    }

    public void Step() {
        _gridManager.UpdateBoard();
    }
}
