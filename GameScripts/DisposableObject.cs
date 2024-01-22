using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisposableObject : MonoBehaviour
{
    [SerializeField] private float timeToLive = 1f;
    [SerializeField] private float scaleMultiplier = 2f;
    [SerializeField] private float destinationTransparency = 0f;

    private float currentLifeTime = 0f;
    private Image disposableIcon;
    private float initialTransparency = 1f;
    private Vector3 initialScale = Vector3.one;

    private void Start()
    {
        disposableIcon = GetComponent<Image>();
        initialTransparency = disposableIcon.color.a;
        initialScale = transform.localScale;
        currentLifeTime = 0f;
    }

    private void Update()
    {
        if(currentLifeTime < timeToLive)
        {
            currentLifeTime += Time.deltaTime;
            currentLifeTime = Mathf.Min(currentLifeTime, timeToLive);

            float percentage = currentLifeTime / timeToLive;
            disposableIcon.color = new Color(disposableIcon.color.r, disposableIcon.color.g, disposableIcon.color.b, Mathf.Lerp(initialTransparency, destinationTransparency, percentage));
            transform.localScale = Vector3.Lerp(initialScale, initialScale * scaleMultiplier, percentage);

            if(currentLifeTime >= timeToLive)
            {
                Destroy(gameObject);
            }
        }
    }
}
