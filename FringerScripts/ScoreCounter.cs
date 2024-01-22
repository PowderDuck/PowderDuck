using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private float score = 0f;
    private bool newRecord = false;
    private SuccessController success;

    private void Start()
    {
        //PlayerPrefs.SetFloat("HighestScore", 0f);
        success = GetComponent<SuccessController>();
    }

    public void IncrementScore(float increment, float multiplier)
    {
        score += increment;

        float highestScore = PlayerPrefs.GetFloat("HighestScore");
        PlayerPrefs.SetFloat("HighestScore", Mathf.Max(highestScore, score));
        success.SuccessAnimation(score, multiplier);

        newRecord = highestScore < score;
    }

    public float GetScore()
    {
        return score;
    }

    public bool IsRecord()
    {
        return newRecord;
    }
}
