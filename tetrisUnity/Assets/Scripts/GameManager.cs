using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tetrominoPrefab;

    public void Start()
    {
        SpawnTetromino();
    }

    public void SpawnTetromino()
    {
        Instantiate(tetrominoPrefab, new Vector3(5, 18, 0), Quaternion.identity);
    }
}
