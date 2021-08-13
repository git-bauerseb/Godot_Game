namespace EngineLearning.scripts.pushy {
    public enum TileType {
        
        
        Empty = 0,
        
        // Binary: 0000 0001
        // Static wall that limits the grid
        Wall = 1,
        
        // Binary: 0000 0010
        // The player who moves around
        Player = 2,
        
        // Binary: 0000 0100
        // House of the player; Needs to be visited in order to complete level
        House = 4,
        
        // Binary: 0000 1000
        // Red ball that can be moved around
        RedBall = 8,
        
        // Binary: 0001 0000
        // Place where a red ball must be placed
        RedBallHole = 16,
        
        // Binary: 0010 0000
        // Box that can be moved around
        WoodBox = 32,
        
        
        
    }
}