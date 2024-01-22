using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager manager;

    [SerializeField] private float steps = 10f;
    [SerializeField] private List<Color> bottleColors = new List<Color>();

    private float currentStep = 0f;
    private int currentColorIndex = 0;
    private int nextColorIndex = 0;
    private List<int> colorIndices = new List<int>();

    public ParticleSystem accelerationEffect;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private int GetNextColorIndex()
    {
        if(colorIndices.Count <= 0f)
        {
            for (int i = 0; i < bottleColors.Count; i++)
            {
                colorIndices.Add(i);
            }
        }

        colorIndices.Remove(currentColorIndex);

        int randomIndex = Mathf.RoundToInt(Random.Range(0f, 1f) * (colorIndices.Count - 1f));
        int nextColorIndex = colorIndices[randomIndex];
        colorIndices.RemoveAt(randomIndex);

        return nextColorIndex;
    }

    public Color GetColor()
    {
        currentStep %= steps;

        if(currentStep <= 0f)
        {
            currentColorIndex = nextColorIndex;
            nextColorIndex = GetNextColorIndex();
        }

        Color bottleColor = Color.Lerp(bottleColors[currentColorIndex], bottleColors[nextColorIndex], currentStep / steps);

        currentStep++;

        return bottleColor;
    }
}
