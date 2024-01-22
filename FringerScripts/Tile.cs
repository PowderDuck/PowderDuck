using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public delegate void TileAction();
    public List<TileAction> tileActions = new List<TileAction>();

    [SerializeField] private float colorTransitionDuration = 1f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> tileSprites = new List<Sprite>();
    public float value = 10f;
    private float currentTransitionTime = 0f;
    private float transitionPercentage = 0f;
    private Color initialColor = Color.gray;
    private Color destinationColor = Color.white;



    private void Start()
    {
        spriteRenderer = spriteRenderer ? spriteRenderer : GetComponent<SpriteRenderer>();
        currentTransitionTime = colorTransitionDuration;
        //SelectRandomSprite();
    }

    private void SelectRandomSprite()
    {
        if(tileSprites.Count > 0f)
        {
            System.Random r = new System.Random((int)MapHandler.handler.GetCurrentLevelIndex());
            int index = r.Next(0, tileSprites.Count - 1);
            spriteRenderer.sprite = tileSprites[index];
        }
    }

    private void Update()
    {
        ColorManagement();
    }

    public void ActivateActions()
    {
        for (int i = 0; i < tileActions.Count; i++)
        {
            tileActions[i]();
        }
    }

    private void ColorManagement()
    {
        if(currentTransitionTime < colorTransitionDuration)
        {
            currentTransitionTime += Time.deltaTime;
            currentTransitionTime = Mathf.Min(currentTransitionTime, colorTransitionDuration);
            transitionPercentage = currentTransitionTime / colorTransitionDuration;

            spriteRenderer.color = Color.Lerp(initialColor, destinationColor, transitionPercentage);
        }
    }
    public void InitializeTransition(Color source, Color destination)
    {
        initialColor = source;
        destinationColor = destination;
        currentTransitionTime = 0f;
    }
}
