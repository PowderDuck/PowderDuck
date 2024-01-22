using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private AnimationCurve positionCurve;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 destinationScale = Vector2.one;

    private float currentTransitionTime = 0f;
    private Vector3 initialPosition = Vector3.zero;
    private Vector3 initialScale = Vector3.zero;


    private void Update()
    {
        if(currentTransitionTime < transitionDuration)
        {
            currentTransitionTime += Time.deltaTime;
            currentTransitionTime = Mathf.Min(currentTransitionTime, transitionDuration);

            float transitionPercentage = currentTransitionTime / transitionDuration;

            transform.position = Vector3.Lerp(initialPosition, initialPosition + positionOffset, positionCurve.Evaluate(transitionPercentage));
            transform.localScale = Vector3.Lerp(initialScale, destinationScale, scaleCurve.Evaluate(transitionPercentage));

            if(currentTransitionTime >= transitionDuration)
            {
                Destroy(gameObject);
            }
        }
    }
    public void InitiateTransition(Vector3 iPos, Vector3 iScale)
    {
        initialPosition = iPos;
        initialScale = iScale;
        currentTransitionTime = 0f;
    }
}
