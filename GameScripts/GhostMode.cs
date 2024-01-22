using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostMode : MonoBehaviour
{
    public List<ColorizedMaterial> playerMaterials = new List<ColorizedMaterial>();
    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private AnimationCurve transitionCurve;
    
    private float currentIndex = 0f;
    private float destinationIndex = 0f;
    private float previousIndex = 0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        float difference = destinationIndex - currentIndex;

        if(Mathf.Abs(difference) > 0f)
        {
            bool reached = Mathf.Clamp01(Mathf.Abs(difference) / (Time.deltaTime * transitionSpeed)) < 1f;
            //currentIndex = Mathf.Clamp01(Mathf.Abs(difference) / Time.deltaTime) < 1f ? destinationIndex : currentIndex + Mathf.Sign(difference) * Time.deltaTime;
            currentIndex += reached ? difference : Mathf.Sign(difference) * Time.deltaTime * transitionSpeed;

            //float percentage = Mathf.Abs(currentIndex - (difference < 0f ? Mathf.Ceil(currentIndex - difference) : Mathf.Floor(currentIndex - difference)));
            float percentage = Mathf.Abs(Mathf.Clamp01(Mathf.Sign(-difference)) - Mathf.Abs(currentIndex - previousIndex));

            for (int i = 0; i < playerMaterials.Count; i++)
            {
                //playerMaterials[i].targetMaterial.color = Color.Lerp(playerMaterials[i].initialColor, playerMaterials[i].destinationColor, transitionCurve.Evaluate(Mathf.Abs(Mathf.Clamp01(-difference) - percentage)));
                playerMaterials[i].targetMaterial.color = Color.Lerp(playerMaterials[i].initialColor, playerMaterials[i].destinationColor, transitionCurve.Evaluate(percentage));
            }

            //difference = destinationIndex - currentIndex;

            //if(Mathf.Abs(difference) <= 0f)
            if(reached)
            {
                previousIndex = currentIndex;
                //currentIndex = destinationIndex;
            }
        }
    }

    public void InitiateTransition(float increment)
    {
        destinationIndex += increment;
        destinationIndex = Mathf.Min(Mathf.Max(0f, destinationIndex), 1f);
    }
}

[System.Serializable]
public class ColorizedMaterial
{
    public Material targetMaterial;
    public Color initialColor = Color.white;
    public Color destinationColor = Color.black;
}
