using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPressedAnimation : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Vector2 scaleMultiplier = new Vector2(0.95f, 0.95f);
    [SerializeField] private float animationDuration = 0.1f;
    private bool isPressing = false;
    private Vector3 initialScale = Vector3.one;
    private float currentAnimationTime = 0f;

    private void Start()
    {
        initialScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
    }

    private void Update()
    {
        isPressing = Input.GetMouseButtonUp(0) ? false : isPressing;
        currentAnimationTime += isPressing ? Time.deltaTime : -Time.deltaTime;
        currentAnimationTime = Mathf.Max(0f, Mathf.Min(currentAnimationTime, animationDuration));

        transform.localScale = Vector3.Lerp(initialScale, initialScale * scaleMultiplier, currentAnimationTime / animationDuration);
    }

}
