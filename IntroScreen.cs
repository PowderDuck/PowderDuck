using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    public delegate void TransitionAction();

    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private bool reverse = false;
    [SerializeField] private List<Image> screens = new List<Image>();

    private float currentTransitionTime = 0f;
    private Image screen;
    private TransitionAction action;

    private void Start()
    {
        currentTransitionTime = transitionDuration;
        screen = screens[LevelManager.manager.chunkQuantityRange.x > 0f ? 1 : 0];
    }

    public void InitiateTransition(bool rev, TransitionAction act = null)
    {
        screen = screen == null ? screens[LevelManager.manager.chunkQuantityRange.x > 0f ? 1 : 0] : screen;
        screen.gameObject.SetActive(true);
        reverse = rev;
        currentTransitionTime = reverse ? transitionDuration : 0f;
        action = act;
    }

    private void Update()
    {
        bool condition = reverse ? currentTransitionTime > 0f : currentTransitionTime < transitionDuration;

        if(condition)
        {
            currentTransitionTime += reverse ? -Time.unscaledDeltaTime : Time.unscaledDeltaTime;
            currentTransitionTime = Mathf.Min(Mathf.Max(0f, currentTransitionTime), transitionDuration);

            float transitionPercentage = currentTransitionTime / transitionDuration;

            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, transitionPercentage);

            condition = reverse ? currentTransitionTime > 0f : currentTransitionTime < transitionDuration;
            
            if(!condition && reverse)
            {
                screen.gameObject.SetActive(false);
            }

            if(!condition && !reverse)
            {
                action?.Invoke();
            }
        }
    }
}
