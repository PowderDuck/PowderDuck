using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingValve : MonoBehaviour
{
    [SerializeField] private float closingExtent = 0.4f;
    [SerializeField] private GameObject wall;
    [SerializeField] private Transform destinationWallPosition;
    [SerializeField] private GameObject closingWheel;
    [SerializeField] private float rotationAmount = 720f;

    private Player player;
    private float closingTime = 0f;
    private float currentClosingTime = 0f;
    private Vector3 initialWheelRotation = Vector3.zero;
    private Vector3 initialWallPosition = Vector3.zero;
    
    private void Start()
    {
        player = FindObjectOfType<Player>();
        Vector3 direction = player.transform.position - transform.position;
        closingTime = (direction.magnitude / player.movementSpeed) / (1f - closingExtent);
        initialWallPosition = wall.transform.localPosition;
        initialWheelRotation = closingWheel.transform.localEulerAngles;
    }

    private void Update()
    {
        if(currentClosingTime < closingTime)
        {
            currentClosingTime += Time.deltaTime;
            currentClosingTime = Mathf.Min(currentClosingTime, closingTime);

            float closingPercentage = currentClosingTime / closingTime;
            closingWheel.transform.localEulerAngles = new Vector3(initialWheelRotation.x, initialWheelRotation.y + rotationAmount * closingPercentage, initialWheelRotation.z);
            wall.transform.localPosition = Vector3.Lerp(initialWallPosition, destinationWallPosition.localPosition, closingPercentage);

            if(currentClosingTime >= closingTime)
            {
                //DustAnimation;
            }
        }
    }
}
