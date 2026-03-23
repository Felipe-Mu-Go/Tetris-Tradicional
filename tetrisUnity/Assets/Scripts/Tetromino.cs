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

    // Runtime visual cubes, one per tetromino cell.
    private GameObject[] blockVisuals;

    private void Start()
    {
        Debug.Log($"Tetromino component ready on '{gameObject.name}' at board position {boardPosition}.");

        // Build visuals when the object starts so the piece is visible in Play mode.
        BuildVisuals();
        UpdateVisuals();
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

        // Keep visuals synchronized with logical board movement.
        UpdateVisuals();

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

        // Rebuild visuals when shape data changes.
        BuildVisuals();
        UpdateVisuals();

        Debug.Log($"Initialized tetromino '{gameObject.name}' as I shape with {cells.Length} cells.");
    }

    /// <summary>
    /// Creates one cube child GameObject for each tetromino cell.
    /// </summary>
    public void BuildVisuals()
    {
        if (cells == null || cells.Length == 0)
        {
            return;
        }

        // Clear previously generated block visuals before rebuilding.
        if (blockVisuals != null)
        {
            for (int i = 0; i < blockVisuals.Length; i++)
            {
                if (blockVisuals[i] != null)
                {
                    Destroy(blockVisuals[i]);
                }
            }
        }

        blockVisuals = new GameObject[cells.Length];

        for (int i = 0; i < cells.Length; i++)
        {
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = $"Block_{i}";

            // Parent each cube to this tetromino and keep scale simple.
            block.transform.SetParent(transform, false);
            block.transform.localScale = Vector3.one;

            blockVisuals[i] = block;
        }
    }

    /// <summary>
    /// Updates each visual cube position from board position + local cell offset.
    /// </summary>
    public void UpdateVisuals()
    {
        if (cells == null || blockVisuals == null)
        {
            return;
        }

        int count = Mathf.Min(cells.Length, blockVisuals.Length);

        for (int i = 0; i < count; i++)
        {
            if (blockVisuals[i] == null)
            {
                continue;
            }

            Vector2Int boardCellPosition = boardPosition + cells[i];
            blockVisuals[i].transform.localPosition = new Vector3(boardCellPosition.x, boardCellPosition.y, 0f);
        }
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
