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

    // Controls whether the piece should continue its automatic fall.
    private bool isFalling = true;

    // Cached board manager used for bounds checks.
    private GameManager gameManager;

    private void Start()
    {
        // Cache the board manager once to keep movement checks simple and fast.
        gameManager = FindFirstObjectByType<GameManager>();

        Debug.Log($"Tetromino component ready on '{gameObject.name}' at board position {boardPosition}.");

        // Build visuals when the object starts so the piece is visible in Play mode.
        BuildVisuals();
        UpdateVisuals();
    }

    private void Update()
    {
        // Horizontal movement is always handled as one cell per key press.
        HandleHorizontalInput();

        // Only advance automatic falling while this tetromino is active.
        if (!isFalling)
        {
            return;
        }

        fallTimer += Time.deltaTime;

        if (fallTimer >= fallInterval)
        {
            // Stop before moving if any cell would go below the board bottom (y < 0).
            if (!CanMoveDown())
            {
                isFalling = false;
                Debug.Log($"Tetromino reached the bottom: '{gameObject.name}' at board position {boardPosition}.");
                fallTimer = 0f;
                return;
            }

            Move(Vector2Int.down);
            fallTimer = 0f;
        }
    }

    /// <summary>
    /// Reads left/right keyboard input and attempts one-cell grid movement.
    /// </summary>
    private void HandleHorizontalInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CanMove(Vector2Int.left))
            {
                Move(Vector2Int.left);
                Debug.Log($"Tetromino moved LEFT to board position {boardPosition}.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CanMove(Vector2Int.right))
            {
                Move(Vector2Int.right);
                Debug.Log($"Tetromino moved RIGHT to board position {boardPosition}.");
            }
        }
    }

    /// <summary>
    /// Returns true when every cell can move one row downward without crossing y = 0.
    /// </summary>
    private bool CanMoveDown()
    {
        if (cells == null)
        {
            return false;
        }

        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int nextCellPosition = boardPosition + cells[i] + Vector2Int.down;

            if (nextCellPosition.y < 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns true if every cell stays inside board limits after applying a direction.
    /// </summary>
    private bool CanMove(Vector2Int direction)
    {
        if (cells == null)
        {
            return false;
        }

        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int nextCellPosition = boardPosition + cells[i] + direction;

            // Keep horizontal movement inside board bounds.
            if (nextCellPosition.x < 0)
            {
                return false;
            }

            // Use the configured board width when GameManager exists.
            if (gameManager != null && nextCellPosition.x >= gameManager.boardWidth)
            {
                return false;
            }

            // Fallback board width if no manager is found.
            if (gameManager == null && nextCellPosition.x >= 10)
            {
                return false;
            }

            // Keep bottom boundary behavior unchanged.
            if (nextCellPosition.y < 0)
            {
                return false;
            }
        }

        return true;
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
            block.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

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

        // Keep the tetromino transform aligned to its logical board anchor.
        transform.position = new Vector3(boardPosition.x, boardPosition.y, transform.position.z);

        int count = Mathf.Min(cells.Length, blockVisuals.Length);

        for (int i = 0; i < count; i++)
        {
            if (blockVisuals[i] == null)
            {
                continue;
            }

            blockVisuals[i].transform.localPosition = new Vector3(cells[i].x, cells[i].y, 0f);
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
