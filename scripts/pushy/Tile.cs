using EngineLearning.scripts.grid_game.misc;
using Godot;

namespace EngineLearning.scripts.pushy {
    public class Tile {
        internal IntVec2 Coordinates { get; set; }
        
        internal int SpriteId { get; set; }
        
        internal TileType Type { get; set; }
        public bool Updated { get; set; }

        public Tile(int id, IntVec2 coords, TileType type) {
            SpriteId = id;
            Coordinates = coords;
            Type = type;
        }

        public override string ToString() {
            return $"tile_type={Type},x={Coordinates.X},y={Coordinates.Y}";
        }
    }
}