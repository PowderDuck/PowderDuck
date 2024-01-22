using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HiddenScoreIncrement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float incrementAmount = 10f;

    private MapHandler mapHandler;
    private MainMenu menu;

    private void Start()
    {
        mapHandler = FindObjectOfType<MapHandler>();
        menu = FindObjectOfType<MainMenu>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        menu.HiddenScoreIncrement();
    }
}
