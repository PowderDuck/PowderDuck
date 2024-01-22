using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Vector3 cameraRotationRange = new Vector3(10f, 10f, 0f);
    [SerializeField] private float shakeQuantity = 3f;
    [SerializeField] private float shakeDuration = 0.5f;

    private float currentShakeTime = 0f;
    private List<Vector3> shakePoints = new List<Vector3>();
    private Vector3 cameraRotation = Vector3.zero;
    private Vector3 initialRotation = Vector3.zero;

    private void Start()
    {
        currentShakeTime = shakeDuration;
        initialRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if(currentShakeTime < shakeDuration)
        {
            currentShakeTime += Time.deltaTime;
            currentShakeTime = Mathf.Min(currentShakeTime, shakeDuration);

            float shakePercentage = currentShakeTime / shakeDuration;

            int pointIndex = Mathf.FloorToInt(Mathf.Min(shakePercentage * (shakePoints.Count - 1f), shakePoints.Count - 2f));
            float remainder = 1f / (shakePoints.Count - 1f);

            float transitionPercentage = (shakePercentage - ((float)pointIndex * remainder)) / remainder;

            cameraRotation = Vector3.Lerp(shakePoints[pointIndex], shakePoints[pointIndex + 1], transitionPercentage);
            transform.eulerAngles = cameraRotation;
        }
    }

    public void Shake(float shakePower)
    {
        shakePoints = new List<Vector3>();

        shakePoints.Add(initialRotation);
        float side = Mathf.Sign(Random.Range(-1, 1f));

        for (int i = 0; i < shakeQuantity; i++)
        {
            Vector3 point = new Vector3(Random.Range(-cameraRotationRange.x, cameraRotationRange.x), cameraRotationRange.y * Random.Range(0f, 1f) * side, Random.Range(-cameraRotationRange.z, cameraRotationRange.z));

            shakePoints.Add(point * shakePower);
            side *= -1f;
        }

        shakePoints.Add(initialRotation);
        currentShakeTime = 0f;
    }
}
