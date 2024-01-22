using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PickableObject : MonoBehaviour
{
    public delegate void PickAction(Player player);
    public List<PickAction> actions = new List<PickAction>();

    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private GameObject effect;
    [SerializeField] private Vector3 rotationVelocity = Vector3.zero;
    [SerializeField] private bool continuousRotation = true;
    [SerializeField] private Vector3 displacement = Vector3.zero;
    [SerializeField] private bool continuousDisplacement = false;
    [SerializeField] private float offset = 0.6f;

    [SerializeField] private DisposableEffect pickEffect;

    [Header("PickAnimation\n")]
    [SerializeField] private float disappearanceDuration = 0.5f;
    [SerializeField] private AnimationCurve disappearanceCurve;

    private float durationRatio = 0f;
    private float multiplier = 0f;
    private Vector3 dynamicRotation = Vector3.zero;
    private bool activated = false;
    private float currentDisappearanceTime = 0f;
    private Vector3 initialScale = Vector3.zero;

    private void Start()
    {
        durationRatio = TimeManager.manager.referenceAnimationDuration / animationDuration;
        transform.eulerAngles += continuousRotation ? rotationVelocity * Random.Range(0f, 4f) : Vector3.zero;
        dynamicRotation = transform.eulerAngles;
        initialScale = transform.localScale;
        offset = Random.Range(0f, offset);
        currentDisappearanceTime = 0f;

        if(effect)
        {
            effect.transform.parent = null;
        }
    }

    private void Update()
    {
        float percentage = (TimeManager.manager.AnimationPercentage * durationRatio + offset) % 1f;
        multiplier = (1f - percentage) - 0.5f;
        float rotationSign = continuousRotation ? 1f : multiplier;
        float displacementSign = continuousDisplacement ? 1f : multiplier;

        transform.position += (displacement * (Time.deltaTime / animationDuration)) * Mathf.Sign(displacementSign);

        dynamicRotation += (rotationVelocity * (Time.deltaTime / animationDuration)) * Mathf.Sign(rotationSign);
        transform.eulerAngles = dynamicRotation;

        DisappearanceTimer();
    }

    private void DisappearanceTimer()
    {
        if(currentDisappearanceTime > 0f)
        {
            currentDisappearanceTime -= Time.deltaTime;
            currentDisappearanceTime = Mathf.Max(0f, currentDisappearanceTime);

            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, initialScale, disappearanceCurve.Evaluate(currentDisappearanceTime / disappearanceDuration));

            if(currentDisappearanceTime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if(player && !activated)
        {
            if(pickEffect)
            {
                Instantiate(pickEffect, transform.position, pickEffect.transform.rotation);
            }

            for (int i = 0; i < actions.Count; i++)
            {
                actions[i](player);
            }

            currentDisappearanceTime = disappearanceDuration;
            activated = true;
        }
    }
}