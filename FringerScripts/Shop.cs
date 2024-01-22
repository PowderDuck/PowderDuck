using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private Color[] priceColors = new Color[2] { Color.red, Color.yellow };

    private void Start()
    {
        UpdateShop();
    }
    public void UpdateShop()
    {
        List<string> availableHats = PlayerPrefs.GetString("PurchasedHats").Split(',').ToList();
        float highestScore = PlayerPrefs.GetFloat("HighestScore");

        for (int i = 0; i < items.Count; i++)
        {
            bool newlyPurchased = false;

            if(highestScore >= items[i].requiredScore && !availableHats.Contains(i.ToString()))
            {
                PurchaseItem(i);
                newlyPurchased = true;
            }

            int colorIndex = Mathf.FloorToInt(Mathf.Clamp01(highestScore / items[i].requiredScore));
            items[i].UpdateItem(availableHats.Contains(i.ToString()) || newlyPurchased, newlyPurchased, priceColors[colorIndex]);
        }
    }

    public void PurchaseItem(int itemIndex)
    {
        string purchasedHats = PlayerPrefs.GetString("PurchasedHats");
        purchasedHats = purchasedHats == "" ? itemIndex.ToString() : $"{purchasedHats},{itemIndex}";
        PlayerPrefs.SetString("PurchasedHats", purchasedHats);
    }
}
