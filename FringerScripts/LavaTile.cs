using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTile : MonoBehaviour
{
    private Tile tile;

    private void Start()
    {
        tile = GetComponent<Tile>();

        tile.tileActions.Add(LavaEntrance);
    }

    private void LavaEntrance()
    {
        FindObjectOfType<Player>().GameOver();
    }
}
