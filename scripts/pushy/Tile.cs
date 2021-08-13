using EngineLearning.scripts.grid_game.misc;
using Godot;

namespace EngineLearning.scripts.pushy {
    public class Tile {
        internal Sprite Sprite { get; set; }
        internal Vector2Int Coordinates { get; set; }
        internal Vector2Int OldCoordinates { get; set; }

        public Tile(Sprite sprite, Vector2Int coords) {
            Sprite = sprite;
            Coordinates = coords;
        }

        public void UpdatePosition(Vector2Int newCoords) {
            var temp = Coordinates;
            Coordinates = newCoords;
            OldCoordinates = temp;
        }
    }
}