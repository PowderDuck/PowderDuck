using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image fingerIcon;
    [SerializeField] private Vector3 decrementPosition = new Vector3(0f, -10f, 0f);
    [SerializeField] private GameObject promptText;
    [SerializeField] private TextMeshProUGUI highestScoreText;
    [SerializeField] private List<AnimatableObject> animObjects = new List<AnimatableObject>();
    [SerializeField] private Vector3 animationRotation = new Vector3(0f, 0f, 10f);
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float scoreIncrement = 100f;

    private float currentAnimationTime = 0f;
    private Vector3 initialFingerPosition = Vector3.zero;
    private Vector3 initialTextRotation = Vector3.zero;
    private List<Vector3> initialScales = new List<Vector3>();

    private void Start()
    {
        initialFingerPosition = fingerIcon.transform.localPosition;
        initialTextRotation = promptText.transform.eulerAngles;

        for (int i = 0; i < animObjects.Count; i++)
        {
            initialScales.Add(animObjects[i].animatiedObject.transform.localScale);
        }

        highestScoreText.text = Mathf.Floor(PlayerPrefs.GetFloat("HighestScore")).ToString();
    }

    private void Update()
    {
        currentAnimationTime += Time.deltaTime;
        currentAnimationTime = Mathf.Min(currentAnimationTime, animationDuration);

        float animationPercentage = currentAnimationTime / animationDuration;
        animationPercentage = Mathf.Sin(180f * animationPercentage * Mathf.Deg2Rad);

        fingerIcon.transform.localPosition = Vector3.Lerp(initialFingerPosition, initialFingerPosition + decrementPosition, animationPercentage);
        promptText.transform.eulerAngles = Vector3.Lerp(initialTextRotation - animationRotation, initialTextRotation + animationRotation, animationPercentage);

        for (int i = 0; i < animObjects.Count; i++)
        {
            if(!animObjects[i].animatiedObject.GetComponent<UIPressedAnimation>().isPressing)
            {
                animObjects[i].animatiedObject.transform.localScale = Vector3.Lerp(initialScales[i], initialScales[i] * animObjects[i].scaleMultiplier, animationPercentage);
            }
        }

        currentAnimationTime %= animationDuration;
    }

    public void HiddenScoreIncrement()
    {
        AudioClip hiddenClip = MapHandler.handler.finishedLevel > 0f ? SoundManager.manager.multiplierBoost : null;
        SoundManager.manager.PlaySound(hiddenClip, 0);
        PlayerPrefs.SetFloat("HighestScore", PlayerPrefs.GetFloat("HighestScore") + scoreIncrement * MapHandler.handler.finishedLevel);
        MapHandler.handler.finishedLevel = 0f;
        highestScoreText.text = Mathf.Floor(PlayerPrefs.GetFloat("HighestScore")).ToString();
    }
}
[System.Serializable]
public class AnimatableObject
{
    public GameObject animatiedObject;
    public float scaleMultiplier = 1f;
}
