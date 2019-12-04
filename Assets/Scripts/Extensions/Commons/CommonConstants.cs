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

    public static Vector2 SOUTH_VECTOR = Vector2.down;
    public static Vector2 WEST_VECTOR = Vector2.left;
    public static Vector2 NORTH_VECTOR = Vector2.up;
    public static Vector2 EAST_VECTOR = Vector2.right;
    public static Vector2 NORTH_EAST_VECTOR = new Vector2(1, 1);  
    public static Vector2 SOUTH_EAST_VECTOR = new Vector2(1, -1);
    public static Vector2 NORTH_WEST_VECTOR = new Vector2(-1, 1);
    public static Vector2 SOUTH_WEST_VECTOR = new Vector2(-1, -1);

    public static Vector2[] DIRECTION_VECTORS = new Vector2[]{
        Vector2.down, 
        Vector2.left, 
        Vector2.up,
        Vector2.right, 
        new Vector2(1,1), 
        new Vector2(1,-1), 
        new Vector2(-1,1),
        new Vector2(-1,-1)
    };
}
