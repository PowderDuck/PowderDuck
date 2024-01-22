using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public delegate void Interaction();
    public List<Interaction> actions = new List<Interaction>();

    public float transitionDuration = 0.5f;
    [SerializeField] private bool reverse = false;
    [SerializeField] private Vector3 scale = Vector3.one;
    [SerializeField] private float scaleMultiplier = 1f;
    [SerializeField] private AnimationCurve scaleCurve;

    private bool destruction = false;
    private float currentTransitionTime = 0f;
    private Vector3 initialScale = Vector3.zero;

    private void Update()
    {
        bool condition = reverse ? currentTransitionTime > 0f : currentTransitionTime < transitionDuration;

        if (condition)
        {
            currentTransitionTime += reverse ? -Time.deltaTime : Time.deltaTime;
            currentTransitionTime = Mathf.Min(Mathf.Max(0f, currentTransitionTime), transitionDuration);

            transform.localScale = LerpInterpolation(initialScale, scale * scaleMultiplier, scaleCurve.Evaluate(currentTransitionTime / transitionDuration));

            condition = reverse ? currentTransitionTime > 0f : currentTransitionTime < transitionDuration;

            if (!condition && destruction)
            {
                Destroy(gameObject);
            }
        }
    }

    private Vector3 LerpInterpolation(Vector3 source, Vector3 destination, float percentage)
    {
        Vector3 direction = destination - source;

        return source + direction * percentage;
    }
    public void Interact()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i]();
        }
    }

    public void UpdateScale(float multiplier)
    {
        scaleMultiplier = multiplier;

        transform.localScale = scale * scaleMultiplier;
    }

    public void InitiateTransition(bool isReverse, bool destroy = false, bool disableCollider = true)
    {
        reverse = isReverse;
        currentTransitionTime = reverse ? Mathf.Max(Time.deltaTime, currentTransitionTime) : Mathf.Min(0f, transitionDuration - Time.deltaTime);
        destruction = destroy;
        GetComponent<Collider2D>().enabled = !disableCollider;
    }
}
