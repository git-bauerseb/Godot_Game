namespace EngineLearning.scripts.grid_game {
    public class SpawnerTile : Tile {

        public TileOrientation Orientation { get; set; }
        
        public SpawnerTile(int x, int y, TileOrientation orientation) {
            X = x;
            Y = y;
            Orientation = orientation;
        }


        public override TileTypes GetTileType() {
            return TileTypes.SPAWNER;
        }

        public override Tile Copy() {
            return new SpawnerTile(X, Y, Orientation);
        }
    }
}