using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public float fallTime = 1f;
    private float timer;

    void Start()
    {
        timer = fallTime;
    }

    void Update()
    {
        HandleInput();
        HandleFall();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }
    }

    void HandleFall()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = fallTime;

            if (!Move(Vector2Int.down))
            {
                LockPiece();
            }
        }
    }

    bool Move(Vector2Int direction)
    {
        transform.position += (Vector3)direction;

        if (IsValidPosition())
        {
            return true;
        }
        else
        {
            transform.position -= (Vector3)direction;
            return false;
        }
    }

    bool IsValidPosition()
    {
        foreach (Transform block in transform)
        {
            Vector2Int pos = Vector2Int.RoundToInt(block.position);

            if (!Board.IsInside(pos))
                return false;

            if (Board.IsOccupied(pos))
                return false;
        }

        return true;
    }

    void LockPiece()
    {
        Board.AddToGrid(transform);
        Debug.Log("Piece locked");

        FindObjectOfType<GameManager>().SpawnTetromino();

        enabled = false;
    }
}
