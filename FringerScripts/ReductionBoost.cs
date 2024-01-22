using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReductionBoost : MonoBehaviour
{
    [SerializeField] private Vector3 initialScale = new Vector3(7f, 7f, 1f);
    [SerializeField] private float reductionAmount = 0.7f;
    [SerializeField] private Reducer reducerPrefab;
    [SerializeField] private Fader arrowPrefab;

    private Boost boost;
    private Fader arrow;

    private void Start()
    {
        boost = GetComponent<Boost>();

        boost.actions.Add(Reduction);
    }

    private void Update()
    {
        /*if (currentTransitionTime < transitionDuration)
        {
            currentTransitionTime += Time.deltaTime;
            currentTransitionTime = Mathf.Min(currentTransitionTime, transitionDuration);

            if(arrow)
            {
                arrow.transform.localScale = Vector3.Lerp(initialScale, destinationScale, currentTransitionTime / transitionDuration);
            }

            if(currentTransitionTime >= transitionDuration)
            {
                Destroy(arrow.gameObject);
            }

        }*/

        //ScaleTimer();
    }
    /*private void ScaleTimer()
    {
        if(currentReductionTime < reductionDuration)
        {
            currentReductionTime += Time.deltaTime;
            currentReductionTime = Mathf.Min(currentReductionTime, reductionDuration);

            for (int i = 0; i < interactables.Length; i++)
            {
                if(interactables[i].GetComponent<Mine>())
                {
                    interactables[i].UpdateScale(Mathf.Lerp(sourceMultiplier, destinationMultiplier, currentReductionTime / reductionDuration));
                }
            }
        }
    }*/

    private void Reduction()
    {
        arrow = Instantiate(arrowPrefab, new Vector3(0f, 0f, -5f), Quaternion.identity);
        arrow.InitiateTransition(new Vector3(0f, 0f, -5f), initialScale);
        Reducer reducer = Instantiate(reducerPrefab, Vector3.zero, Quaternion.identity);
        reducer.StartReduction(reductionAmount);
        SoundManager.manager.PlaySound(SoundManager.manager.reductionBoost, 0);
    }
}
