using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIContainer : MonoBehaviour
{
    [SerializeField] private List<Image> uiComponents = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> textComponents = new List<TextMeshProUGUI>();
    [SerializeField] private Image exitButton;
    [SerializeField] private GameObject[] gameModes = new GameObject[2];
    [SerializeField] private AnimationCurve transparencyCurve;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private bool reverse = false;

    private List<List<float>> uiTransparency = new List<List<float>>() { new List<float>(), new List<float>() };
    private float currentTransitionTime = 0f;

    private void Start()
    {
        for (int i = 0; i < uiComponents.Count; i++)
        {
            uiTransparency[0].Add(uiComponents[i].color.a);
        }

        for (int i = 0; i < textComponents.Count; i++)
        {
            uiTransparency[1].Add(textComponents[i].color.a);
        }
    }

    private void Update()
    {
        bool condition = reverse ? currentTransitionTime > 0f : currentTransitionTime < transitionDuration;

        if(condition)
        {
            currentTransitionTime += reverse ? -Time.deltaTime : Time.deltaTime;
            currentTransitionTime = Mathf.Min(Mathf.Max(0f, currentTransitionTime), transitionDuration);

            float transitionPercentage = currentTransitionTime / transitionDuration;

            for (int i = 0; i < uiComponents.Count; i++)
            {
                uiComponents[i].color = new Color(uiComponents[i].color.r, uiComponents[i].color.g, uiComponents[i].color.b, uiTransparency[0][i] * transparencyCurve.Evaluate(transitionPercentage));
            }

            for (int i = 0; i < textComponents.Count; i++)
            {
                textComponents[i].color = new Color(textComponents[i].color.r, textComponents[i].color.g, textComponents[i].color.b, uiTransparency[1][i] * transparencyCurve.Evaluate(transitionPercentage));
            }

            if(exitButton)
            {
                exitButton.color = new Color(exitButton.color.r, exitButton.color.g, exitButton.color.b, transparencyCurve.Evaluate(transitionPercentage));
            }

            condition = reverse ? currentTransitionTime > 0f : currentTransitionTime < transitionDuration;

            if(!condition && reverse)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void InitiateTransition(bool rev)
    {
        reverse = rev;
        int mode = PlayerPrefs.GetInt("HardcoreMode");

        if(gameModes.Length > 0f)
        { 
            for (int i = 0; i < gameModes.Length; i++)
            {
                gameModes[i].gameObject.SetActive(mode == i);
            }
        }
    }
}
