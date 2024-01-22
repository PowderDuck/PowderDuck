using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reducer : MonoBehaviour
{
    [SerializeField] private float reductionDuration = 3f;
    [SerializeField] private float sourceMultiplier = 1f;
    [SerializeField] private AnimationCurve reductionCurve;

    private float currentReductionTime = 0f;
    private float reductionAmount = 1f;
    private Interactable[] interactables;

    public void StartReduction(float rAmount)
    {
        reductionAmount = rAmount;
        interactables = FindObjectsOfType<Interactable>();
    }
    private void Update()
    {
        if(currentReductionTime < reductionDuration)
        {
            currentReductionTime += Time.deltaTime;
            currentReductionTime = Mathf.Min(currentReductionTime, reductionDuration);

            float reductionPercentage = reductionCurve.Evaluate(currentReductionTime / reductionDuration);

            for (int i = 0; i < interactables.Length; i++)
            {
                if(interactables[i] && interactables[i].GetComponent<Mine>())
                {
                    interactables[i].UpdateScale(Mathf.Lerp(sourceMultiplier, reductionAmount, reductionPercentage));
                }
            }

            if(currentReductionTime >= reductionDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
