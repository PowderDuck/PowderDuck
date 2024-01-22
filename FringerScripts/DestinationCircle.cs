using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationCircle : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float transparencyDuration = 0.5f;
    [SerializeField] private Vector3 destinationScale = new Vector3(6f, 6f, 1f);
    [SerializeField] private SpriteRenderer displayCircle;

    private float currentAnimationTime = 0f;
    private float currentTransparencyTime = 0f;
    private Vector3 initialScale = Vector3.one;
    private Color initialColor = Color.blue;

    private void Start()
    {
        currentAnimationTime = animationDuration;
        currentTransparencyTime = transparencyDuration;
        initialColor = displayCircle.color;
        initialScale = displayCircle.transform.localScale;
    }

    private void Update()
    {
        if(currentTransparencyTime < transparencyDuration)
        {
            if(currentAnimationTime < animationDuration)
            {
                currentAnimationTime += Time.deltaTime;
                currentAnimationTime = Mathf.Min(currentAnimationTime, animationDuration);

                float animationPercentage = currentAnimationTime / animationDuration;

                displayCircle.transform.localScale = Vector3.Lerp(initialScale, destinationScale, animationPercentage);
            }

            currentTransparencyTime += Time.deltaTime * Mathf.Floor(currentAnimationTime / animationDuration);
            currentTransparencyTime = Mathf.Min(currentTransparencyTime, transparencyDuration);

            float transparencyPercentage = currentTransparencyTime / transparencyDuration;
            displayCircle.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f - transparencyPercentage);
        }
    }

    public void SuccessAnimation()
    {
        currentTransparencyTime = 0f;
        currentAnimationTime = 0f;
    }
}
