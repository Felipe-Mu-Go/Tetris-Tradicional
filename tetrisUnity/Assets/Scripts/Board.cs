using UnityEngine;

public class Board : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;

    public static Transform[,] grid = new Transform[width, height];

    public static bool IsInside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0;
    }

    public static bool IsOccupied(Vector2Int pos)
    {
        if (pos.y >= height) return false;
        return grid[pos.x, pos.y] != null;
    }

    public static void AddToGrid(Transform tetromino)
    {
        foreach (Transform block in tetromino)
        {
            Vector2Int pos = Vector2Int.RoundToInt(block.position);
            grid[pos.x, pos.y] = block;
        }
    }
}
