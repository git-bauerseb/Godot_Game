using Godot;
using System;
using EngineLearning.scripts.grid_game.misc;

public class GridUIManager : Node2D {

    private int _numCellsX = 8;
    private int _numCellsY = 8;

    private int _cellSize = 64;

    private Sprite[,] _cellSprites;

    private Texture _cellTexture;
    private Texture _playerTexture;
    private Texture _rotateRightTile;

    private Sprite _player;

    private float _playerRotation;

    private Vector2Int playerGridPosition;


    /*
     * Represents the game time in seconds.
     */
    private float _time;

    public override void _Ready() {
        _cellSprites = new Sprite[_numCellsY, _numCellsX];
        _cellTexture = GD.Load<Texture>("res://textures/grid_game/cell_border.png");
        _playerTexture = GD.Load<Texture>("res://textures/grid_game/player_tile.png");
        _rotateRightTile = GD.Load<Texture>("res://textures/grid_game/rotate_right_tile.png");
        
        InitializeBoard();
        InitializePlayer();
    }

    public override void _Process(float delta) {

        _time += delta;

        if (_time > 1.0f) {
            _time = 0.0f;
            UpdatePlayer();

        }


        Vector2 nextPosition = ScreenCoordFromGridCoord(playerGridPosition.X, playerGridPosition.Y);
        _player.Position = LinearInterpolation(_player.Position, nextPosition, _time);
        _player.Rotation = Mathf.Deg2Rad(_playerRotation);

    }
    
    private Vector2 LinearInterpolation(Vector2 S, Vector2 E, float t) {
        return (1 - t) * S + t * E;
    }

    private float LinearInterpolation(float s, float e, float t) {
        return (1 - t) * s + t * e;
    }

    private void UpdatePlayer() {
        Vector2Int nextPosition = NextPosition(playerGridPosition);
        if (IsInGrid(nextPosition)) {
            playerGridPosition = nextPosition;
        }

        _playerRotation = (_playerRotation + 90f) % 360;
    }

    private Vector2Int NextPosition(Vector2Int current) {
        return new Vector2Int(current.X + 1, current.Y);
    }

    private bool IsInGrid(Vector2Int pos) {
        return (0 <= pos.X && pos.X < _numCellsX) &&
               (0 <= pos.Y && pos.Y < _numCellsY);
    }

    private void InitializePlayer() {
        
        // The grid position of the player
        playerGridPosition = new Vector2Int(0, 0);
        
        _player = new Sprite();
        _player.Texture = _playerTexture;
        _player.Name = "player";
        _player.Scale = ScaleFromTexture(_playerTexture);
        _player.Position = ScreenCoordFromGridCoord(playerGridPosition.X, playerGridPosition.Y);
        
        AddChild(_player);
    }
    
    private void InitializeBoard() {
        
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
        return new Vector2(
            -(_numCellsX / 2) * _cellSize + _cellSize * x,
            -(_numCellsY / 2) * _cellSize + _cellSize * y
            );
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
}
