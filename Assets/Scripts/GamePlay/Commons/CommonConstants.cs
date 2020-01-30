using UnityEngine;

public class CommonConstants 
{
    /// <summary>
    /// Number of cells in the map
    /// </summary>
    public const int NUMBER_OF_CELLS = 100;

    /// <summary>
    /// Number of columns 
    /// </summary>
    public const int NUMBER_OF_COLUMNS = 10;

    /// <summary>
    /// Number of rows
    /// </summary>
    public const int NUMBER_OF_ROWS = 10;

    /// <summary>
    /// Tile size
    /// </summary>
    public const int TILE_SIZE = 1;

    public const int MAX_CHECK_COLLIDER_SIZE = 15;

    public static readonly int MAX_LEVEL_PER_CHECKPOINT = 3;
    public static readonly int MAX_CHECKPOINT = 7;

    public static Vector2 SOUTH_VECTOR = Vector2.down;
    public static Vector2 WEST_VECTOR = Vector2.left;
    public static Vector2 NORTH_VECTOR = Vector2.up;
    public static Vector2 EAST_VECTOR = Vector2.right;
    public static Vector2 NORTH_EAST_VECTOR = new Vector2(1, 1);  
    public static Vector2 SOUTH_EAST_VECTOR = new Vector2(1, -1);
    public static Vector2 NORTH_WEST_VECTOR = new Vector2(-1, 1);
    public static Vector2 SOUTH_WEST_VECTOR = new Vector2(-1, -1);

    public static Vector2[]  DIRECTION_VECTORS = new Vector2[]{
        EAST_VECTOR,
        NORTH_EAST_VECTOR,
        NORTH_VECTOR,
        NORTH_WEST_VECTOR,
        WEST_VECTOR,
        SOUTH_WEST_VECTOR,
        SOUTH_VECTOR,
        SOUTH_EAST_VECTOR
       
    };
    public class Instruction
    {
        public static string WRONG_DIRECTION = "WRONG DIRECTION";
        public static string NEAR_YOU = "YOU ARE VERY CLOSE TO THE TREASURE";
        public static string IN_DIRECTION = "THE TREASURE IS TO THE ";
        public static string WIN = "YOU WIN";
        public static string CLICK_TO_CONTINUE = "CLICK TO CONTINUE";
    }

    public class SceneName
    {
        public static string CheckPointMapScene = "CheckPointMapScene";
        public static string PlayScene = "PlayScene";
        public static string TreasureMapScene = "TreasureMapScene";
    }

    public class DirectionName
    {
        public static string SOUTH_NAME = "SOUTH";
        public static string WEST_NAME = "WEST";
        public static string NORTH_NAME = "NORTH";
        public static string EAST_NAME = "EAST";
        public static string NORTH_EAST_NAME = "NORTH EAST";
        public static string SOUTH_EAST_NAME = "SOUTH EAST";
        public static string NORTH_WEST_NAME = "NORTH WEST";
        public static string SOUTH_WEST_NAME = "SOUTH WEST";
    }
}
