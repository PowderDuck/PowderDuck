using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public int itemIndex = 0;
    public float requiredScore = 0f;
    [SerializeField] private Image priceImage;
    [SerializeField] private Color[] priceHolderColors = new Color[2];
    [SerializeField] private Image borderImage;
    [SerializeField] private Color[] borderColors = new Color[2];
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject newLabel;

    public void UpdateItem(bool purchased, bool newlyPurchased, Color priceColor)
    {
        int mode = MapHandler.handler.hardcore ? 1 : 0;

        priceImage.color = priceHolderColors[mode];
        borderImage.color = borderColors[mode];

        priceText.text = requiredScore.ToString();
        priceText.color = priceColor;
        priceImage.gameObject.SetActive(!purchased);
        newLabel.SetActive(newlyPurchased);
    }
}
