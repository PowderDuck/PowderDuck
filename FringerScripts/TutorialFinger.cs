using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFinger : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 1f;

    private Vector2 initialPosition = Vector2.zero;
    private Vector2 destinationPosition = Vector2.zero;
    private SpriteRenderer sRenderer;
    private float currentTransitionTime = 0f;

    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        initialPosition = FindObjectOfType<SourceCircle>().transform.position;
        destinationPosition = FindObjectOfType<DestinationCircle>().transform.position;
    }

    private void Update()
    {
        currentTransitionTime += Time.deltaTime;
        currentTransitionTime = Mathf.Min(currentTransitionTime, transitionDuration);

        transform.position = Vector3.Lerp(new Vector3(initialPosition.x, initialPosition.y, -5f), new Vector3(destinationPosition.x, destinationPosition.y, -5f), currentTransitionTime / transitionDuration);
        sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, 1f - (currentTransitionTime / transitionDuration));

        currentTransitionTime %= transitionDuration;
    }
}
