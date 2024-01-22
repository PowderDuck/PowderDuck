using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private Vector2 speedDurationRange = new Vector2(8f, 20f);
    [SerializeField] private float speedIncrement = 1f;

    private float currentSpeedTime = 0f;
    private ColorManager cManager;
    private Multiplier speedMultiplier;
    private Player runner;

    private void Start()
    {
        PickableObject boost = GetComponent<PickableObject>();

        if(boost)
        {
            boost.actions.Add(ActivateSpeed);
        }
    }

    private void ActivateSpeed(Player player)
    {
        SpeedBoost speedster = player.GetComponent<SpeedBoost>() ? player.GetComponent<SpeedBoost>() : player.AddComponent<SpeedBoost>();

        speedster.InitiateSpeed(player);
    }


    private void Update()
    {
        if(currentSpeedTime > 0f)
        {
            currentSpeedTime -= Time.deltaTime;
            currentSpeedTime = Mathf.Max(0f, currentSpeedTime);

            if(currentSpeedTime <= 0f)
            {
                cManager.accelerationEffect.Stop();
                runner.AddSpeedMultiplier(speedMultiplier, false);
            }
        }
    }
    public void InitiateSpeed(Player player)
    {
        cManager = FindObjectOfType<ColorManager>();
        runner = player;

        if (currentSpeedTime <= 0f)
        {
            speedMultiplier = new Multiplier() { multiplier = speedIncrement };
            cManager.accelerationEffect.Play();
            runner.AddSpeedMultiplier(speedMultiplier, true);
        }

        currentSpeedTime = Mathf.Lerp(speedDurationRange.x, speedDurationRange.y, PlayerPrefs.GetFloat("SpeedUpgrade"));
    }
}
