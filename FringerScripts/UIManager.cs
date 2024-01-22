using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager manager;

    [SerializeField] private Vector2 referenceDimensions = new Vector2(800f, 480f);

    [Header("MainMenuScreen\n")]
    [SerializeField] private UIContainer mainMenuContainer;

    [Header("GameOverScreen\n")]
    [SerializeField] private UIContainer gameOverContainer;
    [SerializeField] private TextMeshProUGUI finalText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject recordText;

    [Header("ExitScreen\n")]
    [SerializeField] private UIContainer exitContainer;

    [Header("AnimationSettings\n")]
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private List<GameObject> recordStars = new List<GameObject>();
    [SerializeField] private Vector2 fontSizeRange = new Vector2(68f, 72f);
    [SerializeField] private Vector2 gameOverFontSizeRange = new Vector2(58f, 62f);
    [SerializeField] private Vector2 starZRotationRange = new Vector2(-15f, 15f);

    private Vector2 currentDimensions = Vector2.zero;
    private ScoreCounter counter;
    private bool isRecord = false;
    private float currentAnimationTime = 0f;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentDimensions = new Vector2((float)Camera.main.pixelWidth, (float)Camera.main.pixelHeight);
    }

    private void Start()
    {
        counter = FindObjectOfType<ScoreCounter>();
        mainMenuContainer.gameObject.SetActive(true);
        mainMenuContainer.InitiateTransition(false);
        currentAnimationTime = animationDuration;
    }

    public Vector2 GetCameraRatio()
    {
        Vector2 ratio = new Vector2(currentDimensions.x / referenceDimensions.x, currentDimensions.y / referenceDimensions.y);

        return ratio;
    }

    public void ExitTransition()
    {
        SoundManager.manager.PlaySound(SoundManager.manager.exitGame, 0);
        exitContainer.gameObject.SetActive(true);
        exitContainer.InitiateTransition(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        SoundManager.manager.PlaySound(SoundManager.manager.uiClick, 0);
        exitContainer.InitiateTransition(true);
    }

    private void Update()
    {
        if(currentAnimationTime < animationDuration)
        {
            currentAnimationTime += Time.deltaTime;
            currentAnimationTime = Mathf.Min(currentAnimationTime, animationDuration);

            float animationPercentage = Mathf.Sin(180f * (currentAnimationTime / animationDuration) * Mathf.Deg2Rad);

            finalText.fontSize = Mathf.Lerp(fontSizeRange.x, fontSizeRange.y, animationPercentage);
            gameOverText.fontSize = Mathf.Lerp(gameOverFontSizeRange.x, gameOverFontSizeRange.y, animationPercentage);

            if(isRecord)
            {
                for (int i = 0; i < recordStars.Count; i++)
                {
                    float zRotation = Mathf.Lerp(starZRotationRange.x, starZRotationRange.y, animationPercentage);
                    recordStars[i].transform.eulerAngles = new Vector3(0f, 0f, zRotation);
                }
            }

            if(currentAnimationTime >= animationDuration)
            {
                currentAnimationTime = 0f;
            }
        }
    }
    public void Retry()
    {
        currentAnimationTime = animationDuration;
        gameOverContainer.InitiateTransition(true);
    }

    public void GameOver()
    {
        finalText.text = Mathf.Floor(counter.GetScore()).ToString();
        recordText.SetActive(counter.IsRecord());
        isRecord = counter.IsRecord();
        currentAnimationTime = 0f;
        //currentAnimationTime = counter.IsRecord() ? 0f : animationDuration;
        gameOverContainer.gameObject.SetActive(true);
        gameOverContainer.InitiateTransition(false);
    }
}
