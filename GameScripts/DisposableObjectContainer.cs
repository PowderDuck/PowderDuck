using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableObjectContainer : MonoBehaviour
{
    [SerializeField] private float timeToLive = 1f;
    [SerializeField] private float scaleMultiplier = 2f;
    [SerializeField] private float destinationTransparency = 0f;
    [SerializeField] private List<SpriteRenderer> disposableScale = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> disposableColor = new List<SpriteRenderer>();

    private float currentLifeTime = 0f;
    //private SpriteRenderer sRenderer;
    //private float initialTransparency = 1f;
    //private Vector3 initialScale = Vector3.one;
    private List<float> initialTransparencies = new List<float>();
    private List<Vector3> initialScales = new List<Vector3>();

    private void Start()
    {
        //sRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < disposableColor.Count; i++)
        {
            initialTransparencies.Add(disposableColor[i].color.a);
        }

        for (int i = 0; i < disposableScale.Count; i++)
        {
            initialScales.Add(disposableScale[i].transform.localScale);
        }

        //initialTransparency = sRenderer.color.a;
        //initialScale = transform.localScale;
        currentLifeTime = 0f;
    }

    private void Update()
    {
        if (currentLifeTime < timeToLive)
        {
            currentLifeTime += Time.deltaTime;
            currentLifeTime = Mathf.Min(currentLifeTime, timeToLive);

            float percentage = currentLifeTime / timeToLive;
            //sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, Mathf.Lerp(initialTransparency, destinationTransparency, percentage));

            for (int i = 0; i < disposableColor.Count; i++)
            {
                disposableColor[i].color = new Color(disposableColor[i].color.r, disposableColor[i].color.g, disposableColor[i].color.b, Mathf.Lerp(initialTransparencies[i], destinationTransparency, percentage));
            }

            //transform.localScale = Vector3.Lerp(initialScale, initialScale * scaleMultiplier, percentage);
            for (int i = 0; i < disposableScale.Count; i++)
            {
                disposableScale[i].transform.localScale = Vector3.Lerp(initialScales[i], initialScales[i] * scaleMultiplier, percentage);
            }

            if (currentLifeTime >= timeToLive)
            {
                Destroy(gameObject);
            }
        }
    }
}
