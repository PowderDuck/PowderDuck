using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Jobs;

public class DynamicObstacle : MonoBehaviour
{
    public delegate void AnimationEvent();

    public List<List<AnimationEvent>> events = new List<List<AnimationEvent>>()
    {
        new List<AnimationEvent>(), 
        new List<AnimationEvent>()
    };

    [SerializeField] private GameObject positionOffset;
    [SerializeField] private GameObject rotationOffset;
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private AnimationCurve curve;
    
    private float currentAnimationTime = 0f;
    private float extent = 0f;
    private Vector3 initialLocalPosition = Vector3.zero;
    private Vector3 initialLocalRotation = Vector3.zero;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        if(currentAnimationTime < animationDuration)
        {
            currentAnimationTime += Time.deltaTime * MapController.controller.GameSpeed;
            currentAnimationTime = Mathf.Min(currentAnimationTime, animationDuration);

            float animationPercentage = curve.Evaluate(currentAnimationTime / animationDuration);

            transform.localPosition = Vector3.Lerp(initialLocalPosition, positionOffset.transform.localPosition, animationPercentage);
            transform.localEulerAngles = Vector3.Lerp(initialLocalRotation, rotationOffset.transform.localEulerAngles, animationPercentage);

            if(Mathf.Abs(extent - animationPercentage) >= 1f)
            {
                extent = 1f - extent;

                for (int i = 0; i < events[(int)extent].Count; i++)
                {
                    events[(int)extent][i]();
                }
            }

            currentAnimationTime %= animationDuration;
        }
    }
}
