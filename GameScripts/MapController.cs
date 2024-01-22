using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapController : MonoBehaviour
{
    //SetTheWorldRotationAccordingToTheRotationOfTheLastPipeEntranceInstance;
    public static MapController controller;

    [SerializeField] private PipeEntrance pipePrefab;
    public Vector3 worldDirection = Vector3.forward;
    public Vector3 worldUp = Vector3.up;
    public Vector3 worldRight = Vector3.right;
    [SerializeField] private List<Chunk> mapChunks = new List<Chunk>(); //CreateTheChunkClassForChanceInstantiation;
    [SerializeField] private float chunkSize = 10f;
    [SerializeField] private float pipeFrequency = 7f;
    [SerializeField] private float initialChunkQuantity = 10f;
    [SerializeField] private float defaultGameSpeed = 1f;
    private List<int> chunkIndices = new List<int>();
    private Vector3 worldPoint = Vector3.zero;
    private float totalChunkQuantity = 0f;
    private float currentChunkQuantity = 0f;
    private Quaternion worldRotation = Quaternion.identity;
    private List<Multiplier> gameSpeedMultipliers = new List<Multiplier>();

    public float GameSpeed
    {
        get
        {
            return GetGameSpeed();
        }

        set
        {
            GameSpeed = value;
        }
    }

    private void Awake()
    {
        if(controller == null)
        {
            controller = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float GetGameSpeed()
    {
        float speed = defaultGameSpeed;

        for (int i = 0; i < gameSpeedMultipliers.Count; i++)
        {
            speed *= gameSpeedMultipliers[i].multiplier;
        }

        return speed;
    }

    public void SetMultiplier(Multiplier mPlier, bool add)
    {
        if(add)
        {
            if(!gameSpeedMultipliers.Contains(mPlier))
            {
                gameSpeedMultipliers.Add(mPlier);
            }
        }
        else
        {
            gameSpeedMultipliers.Remove(mPlier);
        }
    }

    private void Start()
    {
        /*for (int i = 0; i < initialChunkQuantity; i++)
        {
            Chunk currentChunk = GetRandomChunk();
            Chunk chunkInstance = Instantiate(currentChunk);
            chunkInstance.InitializeChunk(0f);
            chunkInstance.transform.position = worldPoint + worldDirection * currentChunkQuantity * chunkSize;
            chunkInstance.transform.rotation = worldRotation;
        }*/
    }

    private Chunk GetRandomChunk()
    {
        if(chunkIndices.Count <= 0f)
        {
            for (int i = 0; i < mapChunks.Count; i++)
            {
                chunkIndices.Add(i);
            }
        }

        int randomIndex = Mathf.RoundToInt(Random.Range(0f, 1f) * (chunkIndices.Count - 1f));
        Chunk returnChunk = mapChunks[chunkIndices[randomIndex]];
        chunkIndices.RemoveAt(randomIndex);
        totalChunkQuantity++;
        currentChunkQuantity++;

        if(totalChunkQuantity > 0f && totalChunkQuantity % pipeFrequency == 0f)
        {
            PlacePipe();
        }

        return returnChunk;
    }

    private void PlacePipe()
    {
        PipeEntrance currentEntrance = Instantiate(pipePrefab);
        currentEntrance.transform.position = worldPoint + worldDirection * chunkSize * currentChunkQuantity;
        currentEntrance.transform.rotation = worldRotation;
        //Set worldRotation to the rotation of the PipeEntranceIndicator;
        //Set worldDirection to the direction of the PipeEntranceIndicator;
        //Set worldPoint to the position of the PipeEntranceIndicator;

        currentChunkQuantity = 0f;
    }
}

[System.Serializable]
public class Multiplier
{
    public float multiplier = 1f;
}