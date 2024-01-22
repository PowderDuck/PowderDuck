using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float multiplier = 1f;

    [Header("NormalModeSettings\n")]
    [SerializeField] private float dotOffset = 0.05f;
    [SerializeField] private float checkWidth = 0.1f;
    [SerializeField] private LineRenderer lineEffect;
    [SerializeField] private List<Color> tileColors = new List<Color>();

    [Header("HardcoreModeSettings\n")]
    [SerializeField] private float radiusExpansionDuration = 2f;
    [SerializeField] private float radius = 50f;
    [SerializeField] private GameObject indicator;
    [SerializeField] private float indicatorRadiusReference = 3.5f;

    [Header("SoundSettings\n")]
    [SerializeField] private float coveredTileThreshold = 20f;
    [SerializeField] private Vector2 pitchInterpolation = new Vector2(1f, 2f);

    private const int interactableMask = 256;
    private const int mineMask = 512;
    private List<Tile> coveredTiles = new List<Tile>();
    private Tile previousTile = null;
    private Vector3 previousMousePosition = Vector3.zero;
    private Vector3 currentMousePosition = Vector3.zero;
    private Vector3 currentDirection = Vector3.zero;
    private Vector3 previousDirection = Vector3.forward;
    private Vector3 dynamicLinePosition = Vector3.zero;
    private RaycastHit2D hitInfo;
    private Vector2 orthographicRatio = Vector2.one;
    private Collider2D[] colliders;
    private List<Vector3> linePositions = new List<Vector3>();
    private bool dragging = false;
    private bool updateLine = false;
    private Vector2 warningDirection = Vector2.zero;
    private ComboHandler combo;
    private MapHandler mapController;
    private Vector2 radiusExpansionPosition = Vector2.zero;
    private float currentFadeIndex = -1f;
    private float currentExpansionTime = 0f;
    private Touch initialTouch;

    private void Start()
    {
        currentExpansionTime = radiusExpansionDuration;
        mapController = FindObjectOfType<MapHandler>();
        combo = FindObjectOfType<ComboHandler>();
        CameraRatio();
    }
    private void Update()
    {
        if(mapController.isPlaying)
        {
            TileManagement();
            DragLogic();
            RadiusExpansion();
        }
    }

    private void CameraRatio()
    {
        orthographicRatio = new Vector2((float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight, (float)Camera.main.pixelHeight / (float)Camera.main.pixelHeight);
    }

    private void DragLogic()
    {
        if(dragging && mapController.isPlaying)
        {
            currentDirection = currentMousePosition - previousMousePosition;
            float dot = Vector3.Dot(previousDirection, currentDirection);
            dynamicLinePosition = GetRelativePosition(currentMousePosition);
            
            if(dot >= dotOffset)
            {
                linePositions.Add(dynamicLinePosition);
            }
            else
            {
                linePositions[linePositions.Count - 1] = dynamicLinePosition;
            }

            updateLine = dot >= dotOffset;
            previousMousePosition = currentMousePosition;
            previousDirection = currentDirection;

            if(linePositions.Count > 2f)
            {
                linePositions[0] = linePositions[1];
            }

            if(updateLine)
            {
                lineEffect.positionCount = linePositions.Count;
                lineEffect.SetPositions(linePositions.ToArray());
            }
        }
    }

    private void Indicator()
    {
        if(MapHandler.handler.hardcore)
        {
            indicator.SetActive(true);
            currentExpansionTime = 0f;
            currentFadeIndex++;
        }
    }

    private void RadiusExpansion()
    {
        if(currentExpansionTime < radiusExpansionDuration)
        {
            currentExpansionTime += Time.deltaTime;
            currentExpansionTime = Mathf.Min(currentExpansionTime, radiusExpansionDuration);

            float expansionPercentage = currentExpansionTime / radiusExpansionDuration;

            Collider2D[] mines = Physics2D.OverlapCircleAll(radiusExpansionPosition, radius * expansionPercentage, mineMask);

            for (int i = 0; i < mines.Length; i++)
            {
                Mine currentMine = mines[i].GetComponent<Mine>();

                if(currentMine.fadeIndex != currentFadeIndex)
                {
                    currentMine.Fade(currentFadeIndex);
                }
            }

            indicator.transform.position = new Vector3(radiusExpansionPosition.x, radiusExpansionPosition.y, -4f);
            indicator.transform.localScale = new Vector3(radius * expansionPercentage / indicatorRadiusReference, radius * expansionPercentage / indicatorRadiusReference, 1f);

            if(currentExpansionTime >= radiusExpansionDuration)
            {
                indicator.SetActive(false);
            }
        }
    }

    private void TileManagement()
    {
        currentMousePosition = Input.mousePosition;
        bool pressed = Input.GetMouseButtonDown(0);
        /*bool pressed = dragging;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch[] touches = Input.touches;
            if(touches[i].phase == TouchPhase.Began && !pressed)
            {
                initialTouch = touches[i];
                pressed = true;
                break;
            }
        }
        */
        if(pressed || dragging)
        {
            hitInfo = Physics2D.Raycast(Camera.main.transform.position, currentMousePosition);
            colliders = Physics2D.OverlapCircleAll(GetRelativePosition(currentMousePosition), checkWidth);
            for (int i = 0; i < colliders.Length; i++)
            {
                if(pressed)
                {
                    SourceCircle source = colliders[i].transform.GetComponent<SourceCircle>();
                    if(source)
                    {
                        radiusExpansionPosition = source.transform.position;
                        dragging = true;
                        lineEffect.transform.position = GetRelativePosition(currentMousePosition);
                        linePositions.Add(Vector3.zero);
                        Indicator();
                    }
                }

                DestinationCircle destination = colliders[i].GetComponent<DestinationCircle>();
                if(destination && dragging)
                {
                    destination.SuccessAnimation();
                    float increment = 0f;
                    float comboMultiplier = combo.GetMultiplier();

                    for (int t = 0; t < coveredTiles.Count; t++)
                    {
                        coveredTiles[t].InitializeTransition(tileColors[3], tileColors[0]);
                        increment += coveredTiles[t].value * multiplier * comboMultiplier;
                    }

                    linePositions = new List<Vector3>();
                    lineEffect.positionCount = 0;
                    dragging = false;

                    mapController.Success(increment, multiplier * comboMultiplier);

                    multiplier = 1f;
                }

                Tile tile = colliders[i].transform.GetComponent<Tile>();
                if(tile && previousTile != tile)
                {
                    previousTile = previousTile == null ? tile : previousTile;

                    if(previousTile != tile && !coveredTiles.Contains(tile))
                    {
                        tile.InitializeTransition(tileColors[1], tileColors[1]);
                        tile.ActivateActions();
                        coveredTiles.Add(tile);
                        SoundManager.manager.PlaySound(SoundManager.manager.tileSelection, 3, Mathf.Lerp(pitchInterpolation.x, pitchInterpolation.y, Mathf.Clamp01(coveredTiles.Count / coveredTileThreshold)));

                        //warningDirection = Vector2.zero;
                        //leadingTile = tile;
                    }

                    /*if(!coveredTiles.Contains(tile) && previousTile == leadingTile)
                    {
                        if(Vector2.Dot(warningDirection.normalized, (tile.transform.position - previousTile.transform.position).normalized) >= 0f)
                        {
                            tile.InitializeTransition(tileColors[0], tileColors[1]);
                            coveredTiles.Add(tile);
                            warningDirection = Vector2.zero;
                            leadingTile = tile;
                        }
                    }
                    else
                    {
                        warningDirection = previousTile.transform.position - tile.transform.position;
                    }*/
                    previousTile = tile;
                }

                Mine mine = colliders[i].GetComponent<Mine>();
                
                if(mine)
                {
                    GameOver();
                }

                Boost boost = colliders[i].GetComponent<Boost>();

                if(boost)
                {
                    boost.ActivateBoost();
                }
            }
        }

        //if(initialTouch.phase == TouchPhase.Ended)
        if(Input.GetMouseButtonUp(0))
        {
            if(dragging)
            {
                for (int i = 0; i < coveredTiles.Count; i++)
                {
                    coveredTiles[i].InitializeTransition(tileColors[2], tileColors[0]);
                }
            }

            previousTile = null;
            dragging = false;
            coveredTiles = new List<Tile>();
            linePositions = new List<Vector3>();
            lineEffect.positionCount = 0;
            //lineEffect.SetPositions(linePositions.ToArray());
        }
    }

    public void GameOver()
    {
        lineEffect.positionCount = 0;
        indicator.SetActive(false);
        mapController.GameOver();
    }

    private Vector3 GetRelativePosition(Vector3 screenPosition)
    {
        Vector3 direction = new Vector3((screenPosition.x - (float)Camera.main.pixelWidth / 2f) / ((float)Camera.main.pixelWidth / 2f), (screenPosition.y - (float)Camera.main.pixelHeight / 2f) / ((float)Camera.main.pixelHeight / 2f), screenPosition.z);
        Vector3 relativePosition = new Vector3(direction.x * Camera.main.orthographicSize * orthographicRatio.x, direction.y * orthographicRatio.y * Camera.main.orthographicSize, -5f);

        return relativePosition;
    }
}
