namespace EngineLearning.scripts.grid_game {
    public abstract class Tile {
        public int X { get; set; }
        public int Y { get; set; }
        
        // Unique id of the tile. This is used to synchronize
        // the state of the board with what is actually displayed
        public int Id { get; set; }

        public abstract TileTypes GetTileType();

        public override string ToString() {
            return $"Tile[Type:{GetTileType()}]";
        }

        public abstract Tile Copy();
    }
}