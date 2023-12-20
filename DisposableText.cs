using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisposableText : MonoBehaviour
{
    [SerializeField] private float timeToLive = 1f;
    [SerializeField] private float scaleMultiplier = 2f;
    [SerializeField] private float destinationTransparency = 0f;

    private float currentLifeTime = 0f;
    [HideInInspector] public TextMeshPro sRenderer;
    private float initialTransparency = 1f;
    private Vector3 initialScale = Vector3.one;

    private void Awake()
    {
        sRenderer = GetComponent<TextMeshPro>();
        initialTransparency = sRenderer.color.a;
        initialScale = transform.localScale;
        currentLifeTime = 0f;
    }

    private void Update()
    {
        if (currentLifeTime < timeToLive)
        {
            currentLifeTime += Time.deltaTime;
            currentLifeTime = Mathf.Min(currentLifeTime, timeToLive);

            float percentage = currentLifeTime / timeToLive;
            sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, Mathf.Lerp(initialTransparency, destinationTransparency, percentage));
            transform.localScale = Vector3.Lerp(initialScale, initialScale * scaleMultiplier, percentage);

            if (currentLifeTime >= timeToLive)
            {
                Destroy(gameObject);
            }
        }
    }
}
