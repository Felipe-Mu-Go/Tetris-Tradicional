using UnityEngine;

public class Tetromino : MonoBehaviour
{
    // Local coordinates for each block that forms this tetromino.
    public Vector2Int[] cells;

    // Current anchor position of the tetromino on the board.
    public Vector2Int boardPosition;

    // Time (in seconds) between automatic downward moves.
    public float fallInterval = 1f;

    // Tracks elapsed time since the last automatic fall step.
    private float fallTimer = 0f;

    private void Start()
    {
        Debug.Log($"Tetromino component ready on '{gameObject.name}' at board position {boardPosition}.");
    }


    private void Update()
    {
        fallTimer += Time.deltaTime;

        if (fallTimer >= fallInterval)
        {
            Move(Vector2Int.down);
            fallTimer = 0f;
        }
    }

    public void Move(Vector2Int direction)
    {
        boardPosition += direction;
        Debug.Log($"Tetromino '{gameObject.name}' moved to board position {boardPosition}.");
    }

    /// <summary>
    /// Initializes this tetromino as the classic I shape (4 blocks in a horizontal line).
    /// </summary>
    public void InitializeAsIShape()
    {
        cells = new Vector2Int[]
        {
            new Vector2Int(-1, 0),
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(2, 0)
        };

        Debug.Log($"Initialized tetromino '{gameObject.name}' as I shape with {cells.Length} cells.");
    }

    /// <summary>
    /// Returns all board positions currently occupied by this tetromino.
    /// </summary>
    public Vector2Int[] GetCellPositions()
    {
        if (cells == null)
        {
            return new Vector2Int[0];
        }

        Vector2Int[] positions = new Vector2Int[cells.Length];

        for (int i = 0; i < cells.Length; i++)
        {
            positions[i] = boardPosition + cells[i];
        }

        return positions;
    }
}
