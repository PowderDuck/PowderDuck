using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScoreBoost : MonoBehaviour
{
    [SerializeField] private float scoreMultiplier = 2f;
    [SerializeField] private Vector3 initialScale = new Vector3(0.1f, 0.1f, 1f);
    [SerializeField] private Fader doubleScorePrefab;
    private Boost boost;

    private void Start()
    {
        boost = GetComponent<Boost>();

        boost.actions.Add(DoubleMultiplier);
    }

    private void DoubleMultiplier()
    {
        FindObjectOfType<Player>().multiplier = scoreMultiplier;
        Fader fader = Instantiate(doubleScorePrefab);
        fader.InitiateTransition(transform.position, initialScale);
        SoundManager.manager.PlaySound(SoundManager.manager.multiplierBoost, 0);
    }
}
