namespace EngineLearning.scripts.grid_mechanics {
    public enum TileType {
        
        /*
         * Represents a tile that moves and has a direction.
         * This is the main player tile.
         */
        MOVEABLE_TILE,
        
        /*
         * Tile that rotates all moveable neighbors by 90 degree in
         * right direction.
         */
        ROTATE_RIGHT_TILE
    }
}