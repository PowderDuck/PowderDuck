using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager manager;

    public float referenceAnimationDuration = 10f;
    public float AnimationPercentage { get { return currentAnimationTime / referenceAnimationDuration; } }

    private float currentAnimationTime = 0f;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        currentAnimationTime += Time.deltaTime;
        currentAnimationTime = Mathf.Min(currentAnimationTime, referenceAnimationDuration);

        currentAnimationTime %= referenceAnimationDuration;
    }
}
