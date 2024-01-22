using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBoost : MonoBehaviour
{
    [SerializeField] private float boostDuration = 8f;

    private Player player;
    private float currentBoostTime = 0f;

    private void Start()
    {
        PickableObject boost = GetComponent<PickableObject>();

        if(boost)
        {
            currentBoostTime = boostDuration;
            boost.actions.Add(ActivateGhost);
        }

        player = GetComponent<Player>();

        if(player)
        {
            //DisableColliders;
        }
        //GhostMode(1f);
    }

    private void Update()
    {
        if(currentBoostTime < boostDuration)
        {
            currentBoostTime += Time.deltaTime;
            currentBoostTime = Mathf.Min(currentBoostTime, boostDuration);

            if(currentBoostTime >= boostDuration)
            {
                GhostMode(-1f);
            }
        }
    }

    private void GhostMode(float activation)
    {
        GhostMode[] modes = FindObjectsOfType<GhostMode>();

        for (int i = 0; i < modes.Length; i++)
        {
            modes[i].InitiateTransition(activation);
        }
    }

    private void ActivateGhost(Player player)
    {
        GhostMode(1f);
    }
}
