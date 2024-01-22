using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceTile : MonoBehaviour
{
    [SerializeField] private List<Tile> replaceableTiles = new List<Tile>();
    private Tile tile;

    private void Start()
    {
        tile = GetComponent<Tile>();

        tile.tileActions.Add(ChanceEntrance);
    }

    private void ChanceEntrance()
    {
        int randomTileIndex = Mathf.RoundToInt(Random.Range(0f, 1f) * (replaceableTiles.Count - 1f));

        Tile currentTile = Instantiate(replaceableTiles[randomTileIndex], transform.position, transform.rotation);

        currentTile.GetComponent<Interactable>().InitiateTransition(false, false, false);

        Destroy(tile.gameObject);
    }
}
