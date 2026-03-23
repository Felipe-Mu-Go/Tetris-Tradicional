using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Standard classic Tetris board size.
    public int boardWidth = 10;
    public int boardHeight = 20;

    // True means that board cell is occupied.
    private bool[,] board;

    private void Start()
    {
        InitializeBoard();
    }

    /// <summary>
    /// Creates a fresh board based on current width and height values.
    /// </summary>
    private void InitializeBoard()
    {
        board = new bool[boardWidth, boardHeight];

        Debug.Log($"Tetris board initialized successfully: {boardWidth}x{boardHeight}");
    }

    /// <summary>
    /// Returns true if the position is inside the board limits.
    /// </summary>
    public bool IsInsideBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < boardWidth &&
               position.y >= 0 && position.y < boardHeight;
    }

    /// <summary>
    /// Clears every board cell and resets occupancy to empty.
    /// </summary>
    public void ClearBoard()
    {
        if (board == null || board.GetLength(0) != boardWidth || board.GetLength(1) != boardHeight)
        {
            board = new bool[boardWidth, boardHeight];
            return;
        }

        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                board[x, y] = false;
            }
        }
    }
}
