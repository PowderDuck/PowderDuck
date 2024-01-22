using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenTile : MonoBehaviour
{
    [SerializeField] private float goldenMultiplier = 2f;

    private Tile tile;

    private void Start()
    {
        tile = GetComponent<Tile>();

        tile.value *= goldenMultiplier;
    }
}
