using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransition : MonoBehaviour
{
    public bool reverse = false;
    [SerializeField] private List<GameObject> UIElements = new List<GameObject>();
    [SerializeField] private float dispositionExtent = 1.25f;
    [SerializeField] private float transitionTime = 2f;
    [SerializeField] private float currentTransitionTime = 0f;
    [SerializeField] private Vector2 transitionCompletionRange = new Vector2(0.75f, 1f);
    [SerializeField] private ScoreCounter counter;
    [SerializeField] private GameObject scorePlaceholder;
    [SerializeField] private GameObject scoreRealTimePlaceholder;
    private List<TransitionData> transitions = new List<TransitionData>();
    private const float divisionDegree = 90f;
    private bool endCondition = false;

    private void Start()
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            Vector3 initialPosition = UIElements[i].transform.localPosition;
            Vector2 cameraDimensions = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

            float randomAngle = Random.Range(0f, 360f);
            float side = Mathf.Round(randomAngle / divisionDegree);

            Vector3 offset = new Vector3(Mathf.Sin(divisionDegree * side * Mathf.Deg2Rad) * cameraDimensions.x / 2f, Mathf.Cos(divisionDegree * side * Mathf.Deg2Rad) * cameraDimensions.y / 2f, 0f);

            //Vector3 offBorderPosition = initialPosition + offset * dispositionExtent;
            Vector3 offBorderPosition = offset * dispositionExtent;

            TransitionData transition = new TransitionData(offBorderPosition, initialPosition, Random.Range(transitionCompletionRange.x, transitionCompletionRange.y));

            transitions.Add(transition);
        }
    }

    private void Update()
    {
        InitializeTransitions();
    }
    public void Reverse(bool rev)
    {
        reverse = rev;
        if(reverse && counter != null)
        {
            counter.transform.localPosition = scoreRealTimePlaceholder.transform.localPosition;
        }
    }
    public void ResetTransitions() //??????????;
    {
        currentTransitionTime = 0f;
    }

    private void InitializeTransitions()
    {
        endCondition = reverse ? currentTransitionTime > 0f : currentTransitionTime < transitionTime;

        if(endCondition)
        {
            currentTransitionTime += reverse ? -Time.deltaTime : Time.deltaTime;
            currentTransitionTime = reverse ? Mathf.Max(0f, currentTransitionTime) : Mathf.Min(currentTransitionTime, transitionTime);

            for (int i = 0; i < UIElements.Count; i++)
            {
                GameObject currentUIElement = UIElements[i];

                currentUIElement.transform.localPosition = transitions[i].GetTransitionPosition(currentTransitionTime / transitionTime);
            }

            if(counter != null)
            {
                float fade = Mathf.Sin(180f * (currentTransitionTime / transitionTime) * Mathf.Deg2Rad);
                counter.UpdateScoreColor(fade);
            
                counter.transform.localPosition = (currentTransitionTime / transitionTime) >= 0.5f ? scorePlaceholder.transform.localPosition : scoreRealTimePlaceholder.transform.localPosition; //PotentiallTheLocalPosition;
            }

            if(currentTransitionTime >= transitionTime)
            {
                //counter.StartCounter(Mathf.Floor(GameManager.manager.score));
                if(counter != null)
                {
                    counter.StartHighestScore();
                }
                //DisableTheScoreTextAndPlaceItInTheProperLocation;
            }
        }

        if(currentTransitionTime <= 0f)
        {
            //counter.StartCounter(0f);
            gameObject.SetActive(false);
        }
    }
}

public class TransitionData
{
    public Vector3 startingPosition = Vector3.zero;
    public Vector3 destinationPosition = Vector3.one;
    public float multiplier = 1f;
    //ThePotentialInclusionOfTheScalingForTheDilationEffect;
    public TransitionData(Vector3 initialPosition, Vector3 destination, float completionMultiplier)
    {
        startingPosition = initialPosition;
        destinationPosition = destination;
        multiplier = completionMultiplier;
    }

    public Vector3 GetTransitionPosition(float transitionPercentage)
    {
        Vector3 transitionPosition = Vector3.Lerp(startingPosition, destinationPosition, Mathf.Clamp(transitionPercentage / multiplier, 0f, 1f));

        return transitionPosition;
    }
}