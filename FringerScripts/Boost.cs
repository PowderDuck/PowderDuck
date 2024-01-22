using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public delegate void BoostAction();
    public List<BoostAction> actions = new List<BoostAction>();

    [SerializeField] private float fadeDuration = 0.5f;

    private float currentFadeTime = 0f;


    private void Start()
    {
        currentFadeTime = fadeDuration;
    }

    public void ActivateBoost()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i]();
        }

        //currentFadeTime = 0f;
        GetComponent<Interactable>().InitiateTransition(true, true, true);
    }
}
