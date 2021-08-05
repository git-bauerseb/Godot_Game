namespace EngineLearning.scripts.grid_game {
    public class RotationTile : Tile {
        public int Amount { get; }
        public RotationDirection Direction { get; }

        public RotationTile(int x, int y, int amount, RotationDirection direction) {
            Amount = amount;
            Direction = direction;
            X = x;
            Y = y;
        }

        public override TileTypes GetTileType() {
            return TileTypes.ROTATION;
        }

        public override Tile Copy() {
            return new RotationTile(X, Y, Amount, Direction);
        }

        public int GetRotationInDegree() {
            return Amount * (Direction == RotationDirection.RIGHT ? 1 : -1);
        }
    }
}