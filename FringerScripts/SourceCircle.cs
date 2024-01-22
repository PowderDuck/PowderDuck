using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceCircle : MonoBehaviour
{
    [SerializeField] private List<GameObject> hats = new List<GameObject>();
    [SerializeField] private string defaultHats = "";

    private void Start()
    {
        string[] purchasedHats = defaultHats == "" ? PlayerPrefs.GetString("PurchasedHats").Split(',') : defaultHats.Split(',');

        int randomIndex = Mathf.RoundToInt(Random.Range(0f, 1f) * (purchasedHats.Length - 1f)); //ExclusionMechanics;

        hats[int.Parse(purchasedHats[randomIndex])].SetActive(true);
    }
}
