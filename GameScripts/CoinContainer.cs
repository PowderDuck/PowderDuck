using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinContainer : MonoBehaviour
{
    [SerializeField] private List<GameObject> coins = new List<GameObject>();
    [SerializeField] private Vector2 stretchExtent = new Vector2(0f, 1f);
    [SerializeField] private GameObject pivot;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        StretchCoins();
    }

    private void StretchCoins()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            Vector3 direction = coins[i].transform.position - pivot.transform.position;
            coins[i].transform.position += direction.normalized * Mathf.Lerp(stretchExtent.x, stretchExtent.y, player.progress);
        }
    }
}