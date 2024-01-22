using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector2 spawnRates = new Vector2(2f, 0.5f);
    [SerializeField] private Mine minePrefab;

    private float currentSpawnRate = 0f;
    private float currentSpawnTime = 0f;
    private List<Mine> currentMines = new List<Mine>();

    private void Start()
    {
        
    }
}
