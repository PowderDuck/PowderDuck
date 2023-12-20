using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableEffect : MonoBehaviour
{
    private ParticleSystem effect;
    private float duration = 0f;

    private void Start()
    {
        effect = GetComponent<ParticleSystem>();
        duration = effect.main.startLifetime.constant + effect.main.duration;
        effect.Play();

        StartCoroutine(DisposeEffect(duration));
    }

    private IEnumerator DisposeEffect(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
