using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [HideInInspector] public float fadeIndex = 0f;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    private float currentFadeTime = 0f;
    private Interactable core;
    private SpriteRenderer sRenderer;
    private int gridIndex = 0;
    private int typeIndex = 0;

    private void Start()
    {
        core = GetComponent<Interactable>();
        sRenderer = GetComponent<SpriteRenderer>();
        Fade(-1f);
    }

    public void SetIndex(int type, int index)
    {
        typeIndex = type;
        gridIndex = index;
    }

    private void Update()
    {
        if(currentFadeTime > 0f)
        {
            currentFadeTime -= Time.deltaTime;
            currentFadeTime = Mathf.Max(0f, currentFadeTime);

            sRenderer.color = new Color(sRenderer.color.r, sRenderer.color.g, sRenderer.color.b, currentFadeTime / fadeDuration);
            backgroundRenderer.color = new Color(backgroundRenderer.color.r, backgroundRenderer.color.g, backgroundRenderer.color.b, currentFadeTime / fadeDuration);
        }
    }

    private void OnDestroy()
    {
        MapHandler.handler.indices[typeIndex].Add(gridIndex);
    }

    public void Fade(float index)
    {
        fadeIndex = index;
        currentFadeTime = MapHandler.handler.hardcore ? fadeDuration : 0f;
    }

    public void ManipulateMine(bool reverse, bool destroy = false)
    {
        GetComponent<CircleCollider2D>().enabled = reverse;
        core.InitiateTransition(reverse, destroy, true);
    }
}
