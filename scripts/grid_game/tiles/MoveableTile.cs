namespace EngineLearning.scripts.grid_game {
    public class MoveableTile : Tile {
        public TileOrientation Orientation { get; set; }

        public MoveableTile(int x, int y, TileOrientation orientation) {
            this.Orientation = orientation;
            this.X = x;
            this.Y = y;
        }

        public override TileTypes GetTileType() {
            return TileTypes.MOVEABLE;
        }

        public override Tile Copy() {
            return new MoveableTile(X, Y, Orientation);
        }
    }
}