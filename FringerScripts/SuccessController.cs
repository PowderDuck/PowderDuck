using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuccessController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private ParticleSystem successEffect;
    [SerializeField] private float fadeDuration = 0.5f;

    private float currentFadeTime = 0f;

    private void Update()
    {
        if(currentFadeTime > 0f)
        {
            currentFadeTime -= Time.deltaTime;
            currentFadeTime = Mathf.Max(0f, currentFadeTime);

            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, currentFadeTime / fadeDuration);
            multiplierText.color = new Color(multiplierText.color.r, multiplierText.color.g, multiplierText.color.b, currentFadeTime / fadeDuration);
        }
    }

    public void SuccessAnimation(float currentScore, float multiplier)
    {
        currentFadeTime = fadeDuration;

        scoreText.text = Mathf.Floor(currentScore).ToString();
        multiplierText.text = $"x{multiplier}";

        DestinationCircle destination = FindObjectOfType<DestinationCircle>();
        if(destination)
        {
            ParticleSystem effect = Instantiate(successEffect, destination.transform.position, successEffect.transform.rotation);
            effect.Play();

            StartCoroutine(EffectDisposal(effect));
        }

    }

    private IEnumerator EffectDisposal(ParticleSystem effect)
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(effect.gameObject);
    }
}
