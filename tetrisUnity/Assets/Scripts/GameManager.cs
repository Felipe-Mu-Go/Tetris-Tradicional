using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Standard classic Tetris board size.
    public int boardWidth = 10;
    public int boardHeight = 20;

    // Size of each debug cell in world units.
    public float cellSize = 1f;

    // World-space center point of the board debug view.
    public Vector3 boardOrigin = Vector3.zero;

    // True means that board cell is occupied.
    private bool[,] board;

    private void Start()
    {
        InitializeBoard();
        SpawnInitialTetromino();
    }


    /// <summary>
    /// Spawns the first tetromino used as the foundation for gameplay work.
    /// </summary>
    private void SpawnInitialTetromino()
    {
        GameObject tetrominoObject = new GameObject("Tetromino_I");
        Tetromino tetromino = tetrominoObject.AddComponent<Tetromino>();

        tetromino.boardPosition = new Vector2Int(boardWidth / 2, boardHeight - 1);
        tetromino.InitializeAsIShape();

        Vector2Int[] cellPositions = tetromino.GetCellPositions();
        Debug.Log($"Spawned {tetrominoObject.name} with cells: {string.Join(", ", cellPositions)}");
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

    /// <summary>
    /// Draws a centered wire grid in the Scene view to visualize the Tetris board.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (boardWidth <= 0 || boardHeight <= 0 || cellSize <= 0f)
        {
            return;
        }

        Gizmos.color = Color.cyan;

        // Start from bottom-left so the whole board stays centered around boardOrigin.
        Vector3 bottomLeft = boardOrigin + new Vector3(
            (-boardWidth * cellSize * 0.5f) + (cellSize * 0.5f),
            (-boardHeight * cellSize * 0.5f) + (cellSize * 0.5f),
            0f
        );

        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                Vector3 cellCenter = bottomLeft + new Vector3(x * cellSize, y * cellSize, 0f);
                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, cellSize, 0f));
            }
        }
    }
}
