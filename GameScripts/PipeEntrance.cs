using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEntrance : MonoBehaviour
{
    [SerializeField] private float referenceEntranceDuration = 2f;
    [SerializeField] private float referencePlayerSpeed = 10f;
    [SerializeField] private List<Transform> playerTransforms = new List<Transform>();
    [SerializeField] private List<Transform> cameraTransforms = new List<Transform>();
    [SerializeField] private Transform pointPrefab;
    [SerializeField] private AnimationCurve entranceCurve;

    private float entranceDuration = 0f;
    private float currentEntranceTime = 0f;
    private Player player;
    private bool initiated = false;

    private void Update()
    {
        if(currentEntranceTime < entranceDuration)
        {
            currentEntranceTime += Time.deltaTime;
            currentEntranceTime = Mathf.Min(currentEntranceTime, entranceDuration);

            UpdateEntranceAnimation(entranceCurve.Evaluate(currentEntranceTime / entranceDuration));

            if(currentEntranceTime >= entranceDuration)
            {
                player.RegainCameraControl();
            }
        }
    }

    private void UpdateEntranceAnimation(float completion)
    {
        Vector3[] playerPositions = new Vector3[playerTransforms.Count];
        Quaternion[] playerRotations = new Quaternion[playerTransforms.Count];

        for (int i = 0; i < playerTransforms.Count; i++)
        {
            playerPositions[i] = playerTransforms[i].position;
            playerRotations[i] = playerTransforms[i].rotation;
        }

        for (int o = 0; o < playerTransforms.Count - 1f; o++)
        {
            for (int i = 0; i < playerTransforms.Count - 1f - o; i++)
            {
                playerPositions[i] = Vector3.Lerp(playerPositions[i], playerPositions[i + 1], completion);
                playerRotations[i] = Quaternion.Slerp(playerRotations[i], playerRotations[i + 1], completion);
            }
        }

        player.transform.position = playerPositions[0];
        player.transform.rotation = playerRotations[0];

        Vector3[] cameraPositions = new Vector3[playerTransforms.Count];
        Quaternion[] cameraRotations = new Quaternion[playerTransforms.Count];

        for (int i = 0; i < cameraTransforms.Count; i++)
        {
            cameraPositions[i] = cameraTransforms[i].position;
            cameraRotations[i] = cameraTransforms[i].rotation;
        }

        for (int o = 0; o < cameraTransforms.Count - 1f; o++)
        {
            for (int i = 0; i < cameraTransforms.Count - 1f - o; i++)
            {
                cameraPositions[i] = Vector3.Lerp(cameraPositions[i], cameraPositions[i + 1], completion);
                cameraRotations[i] = Quaternion.Slerp(cameraRotations[i], cameraRotations[i + 1], completion);
            }
        }

        Camera.main.transform.position = cameraPositions[0];
        Camera.main.transform.rotation = cameraRotations[0];
    }

    public Transform GetWorldDirection()
    {
        return playerTransforms[playerTransforms.Count - 1];
    }

    private void OnTriggerEnter(Collider other)
    {
        Player collidedPlayer = other.GetComponent<Player>();

        if(collidedPlayer && !initiated)
        {
            player = collidedPlayer;
            player.controlCamera = false;
            entranceDuration = referenceEntranceDuration / (player.movementSpeed / referencePlayerSpeed);

            playerTransforms.Insert(0, Instantiate(pointPrefab, player.transform.position, player.transform.rotation));
            cameraTransforms.Insert(0, Instantiate(pointPrefab, Camera.main.transform.position, Camera.main.transform.rotation));

            initiated = true;
        }
    }
}
