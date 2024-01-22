using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboHandler : MonoBehaviour
{
    public float streak = 0f;
    [SerializeField] private float comboDuration = 2f;
    [SerializeField] private List<float> multipliers = new List<float>();
    [SerializeField] private Image comboIndicator;
    [SerializeField] private GameObject comboHolder;
    [SerializeField] private GameObject comboDestination;
    [SerializeField] private float indicatorHeight = 390f;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;

    [Header("SoundSettings\n")]
    [SerializeField] private Vector2 pitchInterpolation = new Vector2(1f, 2f);

    private float comboTime = 0f;
    private float currentComboIndex = 0f;
    private Vector3 comboInitialPosition = Vector3.zero;
    private CameraShake shaker;
    private int extraSoundIndex = 0;

    private void Start()
    {
        comboInitialPosition = comboIndicator.transform.localPosition;
        shaker = FindObjectOfType<CameraShake>();
    }

    private void Update()
    {
        if(comboTime > 0f)
        {
            comboTime -= Time.deltaTime;
            comboTime = Mathf.Max(0f, comboTime);

            if(comboTime <= 0f)
            {
                streak = 0f;
            }

            currentComboIndex = streak;
            ComboAnimation();
        }

        comboHolder.SetActive(!mainMenu.activeInHierarchy && !gameOverMenu.activeInHierarchy);
    }

    public float GetMultiplier()
    {
        int index = (int)Mathf.Min(streak, multipliers.Count - 1f);
        float multiplier = multipliers[index];
        shaker.Shake(Mathf.Clamp01(streak / multipliers.Count));
        //int randomExtraSound = Mathf.RoundToInt(Random.Range(0f, 1f) * (SoundManager.manager.extraComboSounds.Count - 1f));
        int randomExtraSound = (int)currentComboIndex % SoundManager.manager.extraComboSounds.Count;
        extraSoundIndex = streak % multipliers.Count == 0f ? Mathf.RoundToInt(Random.Range(0f, 1f) * (SoundManager.manager.extraComboSounds.Count - 1f)) : extraSoundIndex;
        AudioClip extraClip = Mathf.Floor(currentComboIndex / multipliers.Count) > 0f ? SoundManager.manager.extraComboSounds[extraSoundIndex] : SoundManager.manager.combo;
        //float pitch = Mathf.Lerp(1f + ((float)index / (multipliers.Count - 1f)), 1f, Mathf.Floor(currentComboIndex / multipliers.Count));
        float pitch = 1f + ((streak % multipliers.Count) / (multipliers.Count - 1f));
        //AudioClip extraClip = streak < multipliers.Count ? null : SoundManager.manager.extraComboSounds[randomExtraSound];
        //SoundManager.manager.PlaySound(SoundManager.manager.combo, 1, Mathf.Lerp(pitchInterpolation.x, pitchInterpolation.y, (float)index / (multipliers.Count - 1f)));
        SoundManager.manager.PlaySound(extraClip, 1, pitch);
        //SoundManager.manager.PlaySound(extraClip, 0);
        streak++;
        comboTime = comboDuration;

        return multiplier;
    }

    private void ComboAnimation()
    {
        float fillPercentage = comboTime / comboDuration;
        float period = Mathf.Clamp01(currentComboIndex / (multipliers.Count - 1f));

        comboIndicator.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, indicatorHeight * period * fillPercentage);
        comboIndicator.transform.localPosition = Vector3.Lerp(comboDestination.transform.localPosition, comboInitialPosition, period * fillPercentage);
    }
}
