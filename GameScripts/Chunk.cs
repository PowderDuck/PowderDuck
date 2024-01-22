using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private List<ObstacleRow> obstacleRows = new List<ObstacleRow>();

    public void InitializeChunk(float difficulty)
    {
        for (int i = 0; i < obstacleRows.Count; i++)
        {
            ObstacleRow currentRow = obstacleRows[i];
            float obstacleCount = Mathf.Lerp(currentRow.countRange.x, currentRow.countRange.y, difficulty > 0f ? (1f - Mathf.Clamp01(Mathf.Floor(Random.Range(0f, 1f) / difficulty))) : 0f);
            float randomOffset = Random.Range(0f, 1f) * (obstacleCount - 1f);

            for (int o = 0; o < obstacleCount; o++)
            {
                currentRow.obstacles[(o + (int)randomOffset) % currentRow.obstacles.Count].SetActive(true);
            }
        }
    }
}

[System.Serializable]
public class ObstacleRow
{
    public List<GameObject> obstacles = new List<GameObject>();
    public Vector2 countRange = new Vector2(0f, 2f);
}