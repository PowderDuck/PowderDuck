using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager : MonoBehaviour
{
    [SerializeField] private List<Color> cameraBackgroundColors = new List<Color>();
    [SerializeField] private float transitionDuration = 1.5f;
    private float currentTransitionTime = 0f;
    private int index = 0;

    private void Start()
    {
        index = Mathf.RoundToInt(Random.Range(0f, 1f) * (cameraBackgroundColors.Count - 1f));
    }
    private void Update()
    {
        if(currentTransitionTime < transitionDuration)
        {
            currentTransitionTime += Time.deltaTime;
            currentTransitionTime = Mathf.Min(currentTransitionTime, transitionDuration);


            if(currentTransitionTime >= transitionDuration)
            {
                index = (index + 1) % cameraBackgroundColors.Count;
                currentTransitionTime = 0f;
            }
        }

        Camera.main.backgroundColor = Color.Lerp(cameraBackgroundColors[index], cameraBackgroundColors[(index + 1) % cameraBackgroundColors.Count], currentTransitionTime / transitionDuration);
    }
}
