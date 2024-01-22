using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealBoost : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 1.5f;
    [SerializeField] private GameObject eyePrefab;
    [SerializeField] private Vector3 destinationPosition = new Vector3(0f, 0.2f, -5f);
    [SerializeField] private Vector3 destinationScale = new Vector3(1.9f, 1.9f, 1f);

    private Boost boost;
    private GameObject eye;
    private SpriteRenderer spriteRenderer;
    private Vector3 initialPosition = Vector3.zero;
    private Vector3 initialScale = Vector3.one;
    private Color initialColor = Color.blue;
    private float currentTransitionTime = 0f;

    private void Start()
    {
        currentTransitionTime = transitionDuration;

        boost = GetComponent<Boost>();
        boost.actions.Add(RevealPath);
    }

    private void Update()
    {
        if(currentTransitionTime < transitionDuration)
        {
            currentTransitionTime += Time.deltaTime;
            currentTransitionTime = Mathf.Min(currentTransitionTime, transitionDuration);

            float transitionPercentage = currentTransitionTime / transitionDuration;

            if(eye)
            {
                eye.transform.position = Vector3.Lerp(initialPosition, destinationPosition, transitionPercentage);
                eye.transform.localScale = Vector3.Lerp(initialScale, destinationScale, transitionPercentage);
                spriteRenderer.color = Color.Lerp(initialColor, new Color(initialColor.r, initialColor.g, initialColor.b, 1f - transitionPercentage), transitionPercentage);
            }

            if(currentTransitionTime >= transitionDuration)
            {
                Destroy(eye.gameObject);
            }
        }
    }

    private void RevealPath()
    {
        currentTransitionTime = 0f;
        eye = Instantiate(eyePrefab, new Vector3(0f, 0f, -5f), Quaternion.identity);
        spriteRenderer = eye.GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
        initialPosition = boost.transform.position;
        initialScale = eye.transform.localScale;

        //RevealPath;
    }
}
