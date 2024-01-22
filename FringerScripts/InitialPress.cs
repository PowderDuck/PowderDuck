using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InitialPress : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UIContainer mainMenuContainer;

    public void OnPointerDown(PointerEventData eventData)
    {
        ModifiedPress();
    }

    private void ModifiedPress()
    {
        MapHandler.handler.SelectGameMode(PlayerPrefs.GetInt("HardcoreMode"));
    }
    public void Press()
    {
        mainMenuContainer.InitiateTransition(true);
        MapHandler.handler.PlayModeSound();
        FindObjectOfType<MapHandler>().StartGame();
    }
}
