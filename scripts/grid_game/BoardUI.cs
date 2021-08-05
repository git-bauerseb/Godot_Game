using Godot;
using System;
using System.Collections.Generic;
using EngineLearning.scripts.grid_game;

public class BoardUI : Node2D {

    private readonly int X_WIDTH = 8;
    private readonly int Y_WIDTH = 8;

    private readonly int CELL_SIZE = 64;

    private Sprite[] cells;
    
    private Texture emptyCellTexture;
    private Texture moveableCellTexture;
    private Texture rotationRightTexture;
    private Texture spawnerTexture;
    
    private Board board;

    private List<SpriteIdent> moveableTiles;

    private float modTime;

    public override void _Ready() {
        emptyCellTexture = GD.Load<Texture>("res://textures/empty_cell.png");
        moveableCellTexture = GD.Load<Texture>("res://textures/moveable_brick.png");
        rotationRightTexture = GD.Load<Texture>("res://textures/rotate_right_brick.png");
        spawnerTexture = GD.Load<Texture>("res://textures/spawner_brick.png");

        cells = new Sprite[X_WIDTH * Y_WIDTH];

        board = new Board(X_WIDTH, Y_WIDTH);
        
        // Set a moveable tile
        // board.SetMoveableBrick((X_WIDTH / 2), Y_WIDTH - 1, TileOrientation.Up);

        
        // Set rotation tile
        
        board.SetRotationBrick(3, 4, 90, RotationDirection.RIGHT);
        board.SetSpawnerBrick(3, 3, TileOrientation.Up);
        board.SetMoveableBrick(3, 2, TileOrientation.Up);
        
        

        moveableTiles = new List<SpriteIdent>();
        
        InitializeBoard();
    }

    private void InitializeBoard() {
        int i = 0;
        
        Vector2 screenCenter = GetViewportRect().Size * 0.5f;
        
        int boardXSize = CELL_SIZE * X_WIDTH;
        int boardYSize = CELL_SIZE * Y_WIDTH;

        Vector2 position = new Vector2(
            screenCenter.x - (((X_WIDTH / 2) - 1) * CELL_SIZE + (CELL_SIZE / 2)),
            screenCenter.y - (((Y_WIDTH / 2) - 1) * CELL_SIZE + (CELL_SIZE / 2))
        );
        
        for (int y = 0; y < Y_WIDTH; y++) {
            for (int x = 0; x < X_WIDTH; x++, i++) {

                // Set new sprite for empty cell
                Sprite cell = new Sprite();
                cell.Texture = emptyCellTexture;
                cell.Position = position;

                cell.Scale = 0.5f * Vector2.One;
                
                AddChild(cell);
                cells[i] = cell;

                position.x += CELL_SIZE;
            }

            position.y += CELL_SIZE;
            position.x -= CELL_SIZE * X_WIDTH;
        }

        // Foreach tile set, create a new sprite and add it to the scene
        foreach (var tile in board.GetTiles()) {
            Sprite tileSprite = new Sprite();
            tileSprite.Position = CalculatePosFromCellPos(tile.X, tile.Y);
            tileSprite.Scale = 0.5f * Vector2.One;

            switch (tile.GetTileType()) {
                case TileTypes.EMPTY: break;
                case TileTypes.SPAWNER:
                    tileSprite.Texture = spawnerTexture;
                    break;
                case TileTypes.MOVEABLE:
                    tileSprite.Texture = moveableCellTexture;
                    break;
                case TileTypes.ROTATION:
                    // @TODO differentiate between left/right rotation
                    tileSprite.Texture = rotationRightTexture;
                    break;
                default:
                    break;
            }
            
            AddChild(tileSprite);

            SpriteIdent spriteIdent = new SpriteIdent();
            spriteIdent.Sprite = tileSprite;
            spriteIdent.Id = tile.Id;
            
            moveableTiles.Add(spriteIdent);
        }
    }

    public override void _Process(float delta) {
        modTime += delta;

        if (modTime >= 2.0f) {
            UpdateBoard();
            modTime = 0.0f;
            
            // Set orientation of tile
            // TODO: aktuell wird die Rotation auf einen festen Wert gesetzt; muss in Zukunft behoben werden
            foreach (var tile in moveableTiles) {
                tile.Sprite.Rotation =
                    Mathf.Deg2Rad(board.GetOrientationOfTileById(tile.Id).GetDegreeFromOrientation());
            }

        }
        
        // Synchronize board state with what is displayed on screen
        for (int i = 0; i < moveableTiles.Count; i++) {
            SpriteIdent tile = moveableTiles[i];
            int x, y;
            board.GetPosOfTileById(out x, out y, tile.Id);
            

            if (x > 0 && y >= 0) {
                // Set new position of tile
                var target = CalculatePosFromCellPos(x, y);

                tile.Sprite.Position = LinearInterpolate(tile.Sprite.Position, target, modTime);
            }
        }
    }

    private Vector2 LinearInterpolate(Vector2 A, Vector2 B, float t) {
        return (1 - t) * A + t * B;
    }

    private void AddNewSprites(List<Tile> tiles) {
        foreach (var tile in tiles) {
            Sprite tileSprite = new Sprite();
            tileSprite.Position = CalculatePosFromCellPos(tile.X, tile.Y);
            tileSprite.Scale = 0.5f * Vector2.One;

            switch (tile.GetTileType()) {
                case TileTypes.EMPTY: break;
                case TileTypes.SPAWNER:
                    tileSprite.Texture = spawnerTexture;
                    break;
                case TileTypes.MOVEABLE:
                    tileSprite.Texture = moveableCellTexture;
                    break;
                case TileTypes.ROTATION:
                    // @TODO differentiate between left/right rotation
                    tileSprite.Texture = rotationRightTexture;
                    break;
                default:
                    break;
            }
            
            AddChild(tileSprite);

            SpriteIdent spriteIdent = new SpriteIdent();
            spriteIdent.Sprite = tileSprite;
            spriteIdent.Id = tile.Id;
            
            moveableTiles.Add(spriteIdent);
        }
    }
    
    private void UpdateBoard() {
        
        board.Update();

        // Collect all new tiles that are spawned
        List<Tile> newTiles = board.GetTiles().FindAll(x => moveableTiles.FindIndex(sprite => sprite.Id == x.Id) < 0);
        AddNewSprites(newTiles);
    }

    private Vector2 CalculatePosFromCellPos(int x, int y) {
        Vector2 screenCenter = GetViewportRect().Size * 0.5f;
        return new Vector2(
            screenCenter.x - (((X_WIDTH / 2) - 1) * CELL_SIZE + (CELL_SIZE / 2)) + CELL_SIZE * x,
            screenCenter.y - (((Y_WIDTH / 2) - 1) * CELL_SIZE + (CELL_SIZE / 2)) + CELL_SIZE * y
        );
    }
}
