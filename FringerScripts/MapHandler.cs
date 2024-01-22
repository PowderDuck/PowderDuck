using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    public static MapHandler handler;

    [HideInInspector] public float finishedLevel = 0f;
    public bool isPlaying = false;
    public bool hardcore = false;

    [Header("GameSettings\n")]
    [SerializeField] private float maxLevelComplexity = 20f;
    [SerializeField] private float difficulty = 0f;
    [SerializeField] private float scoreMultiplierThreshold = 30f;
    [SerializeField] private TutorialFinger tutorial;
    //[SerializeField] private ParticleSystem comboEffect;
    [SerializeField] private SpriteRenderer backgroundImage;
    [SerializeField] private Sprite[] backgroundTextures = new Sprite[2];
    [SerializeField] private Shop shop;
    [SerializeField] private GameObject infoTab;

    [Header("TileSpawnSettings\n")]
    [SerializeField] private Vector2 gridSize = new Vector2(10f, 6f);
    [SerializeField] private Vector2 tileOffset = new Vector2(1.45f, 1.45f);
    [SerializeField] private Vector2 minDistanceX = new Vector2(5f, 8f);
    [SerializeField] private Vector2 minDistanceY = new Vector2(3f, 5f);
    [SerializeField] private List<TileContainer> tiles = new List<TileContainer>();
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private List<SpecialTile> specialTiles = new List<SpecialTile>();
    [SerializeField] private SourceCircle sourcePrefab;
    [SerializeField] private DestinationCircle destinationPrefab;
    
    [Header("MineSpawnSettings\n")]
    [SerializeField] private float mineSpawnThreshold = 15f;
    [SerializeField] private Vector2 mineSpawnRate = new Vector2(2f, 0.5f);
    [SerializeField] private Vector2 mineOffset = new Vector2(0.6f, 0.6f);
    [SerializeField] private Mine minePrefab;
    [SerializeField] private float successExplosionRadius = 3f;

    [Header("BoostSpawnSettings\n")]
    [SerializeField] private float boostSpawnRate = 5f;
    [SerializeField] private List<Boost> boostPrefabs = new List<Boost>();
    [SerializeField] private Vector2 boostOffset = new Vector2(0.3f, 0.3f);

    public List<List<int>> indices = new List<List<int>>() { new List<int>(), new List<int>() };
    private Boost currentBoost;
    private List<Mine> presentMines = new List<Mine>();
    private SourceCircle currentSource;
    private DestinationCircle currentDestination;
    private UIManager uiManager;
    private ScoreCounter counter;
    private bool tutorialLevel = false;
    private InitialPress press;
    private AdManager adManager;
    private int sourceIndex = 0;
    private int destinationIndex = 0;

    private float currentSpawnRate = 0f;
    private float currentSpawnCooldown = 0f;
    private float currentBoostCooldown = 0f;
    private float currentLevelIndex = -1f;

    private void Awake()
    {
        if(handler == null)
        {
            handler = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        counter = FindObjectOfType<ScoreCounter>();
        press = FindObjectOfType<InitialPress>();
        adManager = FindObjectOfType<AdManager>();
        finishedLevel = 1f;
        hardcore = PlayerPrefs.GetInt("HardcoreMode") == 1;
        backgroundImage.sprite = backgroundTextures[PlayerPrefs.GetInt("HardcoreMode")];
    }

    public void StartGame()
    {
        currentLevelIndex++;
        tutorialLevel = currentLevelIndex == 0 && PlayerPrefs.GetInt("Tutorial") == 0;
        tutorial.gameObject.SetActive(tutorialLevel);
        currentBoostCooldown = boostSpawnRate;
        //PlayerPrefs.SetInt("Tutorial", 1);
        difficulty = Mathf.Clamp01(currentLevelIndex / maxLevelComplexity);
        currentSpawnRate = Mathf.Lerp(mineSpawnRate.x, mineSpawnRate.y, difficulty); //Curve;
        SpawnPath();
        InitializeGrid();
        isPlaying = true;
    }

    public void GameOver()
    {
        isPlaying = false;
        tutorial.gameObject.SetActive(false);
        SoundManager.manager.PlaySound(SoundManager.manager.gameOver, 1);
        ClearMap(true);
        finishedLevel = 1f;
        uiManager.GameOver();
        adManager.ShowAD("Interstitial");
    }
    
    private void LateUpdate()
    {
        if(isPlaying)
        {
            MineSpawner();
            BoostSpawner();
            //comboEffect.transform.position = new Vector3(currentDestination.transform.position.x, currentDestination.transform.position.y + 0.25f, -4f);
        }
    }

    public float GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public void SelectGameMode(int mode)
    {
        hardcore = mode == 1;
        PlayerPrefs.SetInt("HardcoreMode", mode);
        backgroundImage.sprite = backgroundTextures[mode];
        press.Press();
    }

    private void InitializeGrid()
    {
        indices = isPlaying ? indices : new List<List<int>>() { new List<int>(), new List<int>() };
        List<int> pathIndices = GetPath();

        List<int> specialTileAvailableIndices = new List<int>();

        for (int i = 0; i < specialTiles.Count; i++)
        {
            specialTileAvailableIndices.Add(i);
        }

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Tile currentPrefab = tilePrefab;

                for (int i = 0; i < specialTileAvailableIndices.Count; i++)
                {
                    if(Random.Range(0f, 1f) <= specialTiles[specialTileAvailableIndices[i]].chance && specialTiles[specialTileAvailableIndices[i]].chance > 0f)
                    {
                        currentPrefab = specialTiles[specialTileAvailableIndices[i]].specialTilePrefab;
                        specialTileAvailableIndices.RemoveAt(i);
                        break;
                    }
                }

                Vector2 gridPos = GridToWorld(new Vector2(x, y), tileOffset, true);

                int iteration = GetGridIndex(new Vector2(x, y));
                int indexIndex = pathIndices.Contains(iteration) ? 1 : 0;
                currentPrefab = iteration == sourceIndex || iteration == destinationIndex ? tilePrefab : currentPrefab;

                Tile currentTile = Instantiate(currentPrefab, gridPos, Quaternion.identity);

                if(!isPlaying && iteration != sourceIndex && iteration != destinationIndex)
                {
                    indices[indexIndex].Add(iteration);
                }
            }
        }
    }

    private List<int> GetPath()
    {
        return new List<int>();
    }

    private void InitializePath()
    {
        Vector2 randomOffset = new Vector2(Mathf.Round(Random.Range(minDistanceX.x, minDistanceX.y)), Mathf.Round(Random.Range(minDistanceY.y, minDistanceY.y)));
        Vector2 destinationGridPos = new Vector2(Mathf.Round(Random.Range(0f, gridSize.x - 1f)), Mathf.Round(Random.Range(0f, gridSize.y - 1f)));
        Vector2 sourceGridPos = new Vector2((destinationGridPos.x + randomOffset.x) % gridSize.x, (destinationGridPos.y + randomOffset.y) % gridSize.y);

        Vector2 destinationPos = GridToWorld(destinationGridPos, tileOffset, true);
        Vector2 sourcePos = GridToWorld(sourceGridPos, tileOffset, true);
        /*destinationGridPos = GridToWorld(destinationGridPos, tileOffset, true);
        sourceGridPos = GridToWorld(sourceGridPos, tileOffset, true);*/

        currentSource = Instantiate(sourcePrefab, new Vector3(sourcePos.x, sourcePos.y, -3f), Quaternion.identity);
        currentDestination = Instantiate(destinationPrefab, new Vector3(destinationPos.x, destinationPos.y, -3f), Quaternion.identity);

        /*currentSource = Instantiate(sourcePrefab, new Vector3(sourceGridPos.x, sourceGridPos.y, -3f), Quaternion.identity);
        currentDestination = Instantiate(destinationPrefab, new Vector3(destinationGridPos.x, destinationGridPos.y, -3f), Quaternion.identity);*/

        for (int i = 0; i < 2f; i++)
        {
            //Vector2 explosionPosition = i == 0 ? sourceGridPos : destinationGridPos;
            Vector2 explosionPosition = i == 0 ? sourcePos : destinationPos;
            Collider2D[] interactables = Physics2D.OverlapCircleAll(explosionPosition, successExplosionRadius);

            for (int m = 0; m < interactables.Length; m++)
            {
                if(interactables[m].GetComponent<Mine>())
                {
                    Interactable mine = interactables[m].GetComponent<Interactable>();
                    mine.InitiateTransition(true, true, true);
                }
            }
        }

        /*sourceIndex = GetGridIndex(GridToWorld(sourceGridPos, tileOffset, false));
        destinationIndex = GetGridIndex(GridToWorld(destinationGridPos, tileOffset, false));*/

        sourceIndex = GetGridIndex(sourceGridPos);
        destinationIndex = GetGridIndex(destinationGridPos);

        indices[0].Remove(sourceIndex);
        indices[0].Remove(destinationIndex);
        indices[1].Remove(sourceIndex);
        indices[1].Remove(destinationIndex);
    }

    public void Retry(int mode)
    {
        hardcore = mode < 0 ? hardcore : mode == 1;
        PlayerPrefs.SetInt("HardcoreMode", hardcore ? 1 : 0);
        backgroundImage.sprite = backgroundTextures[PlayerPrefs.GetInt("HardcoreMode")];
        uiManager.Retry();
        counter.IncrementScore(-counter.GetScore(), 0f);
        StartGame();
        isPlaying = true;
        PlayModeSound();
    }

    public void PlayModeSound()
    {
        int soundIndex = hardcore ? 1 : 0;
        SoundManager.manager.PlaySound(SoundManager.manager.gameModes[soundIndex], 0);
    }

    public void OpenShop()
    {
        SoundManager.manager.PlaySound(SoundManager.manager.uiClick, 0);
        shop.gameObject.SetActive(true);
        shop.UpdateShop();
    }

    public void CloseShop()
    {
        SoundManager.manager.PlaySound(SoundManager.manager.exitUIClick, 0);
        shop.gameObject.SetActive(false);
    }

    public void OpenInfoTab()
    {
        SoundManager.manager.PlaySound(SoundManager.manager.uiClick, 0);
        infoTab.SetActive(true);
    }

    public void CloseInfoTab()
    {
        SoundManager.manager.PlaySound(SoundManager.manager.exitUIClick, 0);
        infoTab.SetActive(false);
    }

    private Vector2 GridToWorld(Vector2 gridPos, Vector2 offset, bool world)
    {
        float sign = world ? 1f : -1f;
        Vector2 tileOffsetMultiplier = world ? new Vector2(offset.x, offset.y) : new Vector2(1f, 1f);
        Vector2 gridP = world ? new Vector2(gridPos.x, gridPos.y) : new Vector2(gridPos.x / offset.x, gridPos.y / offset.y);
        Vector2 worldPos = new Vector2(((-(gridSize.x - 1f) / 2f) * sign + gridP.x) * tileOffsetMultiplier.x, ((-(gridSize.y - 1f) / 2f) * sign + gridP.y) * tileOffsetMultiplier.y);

        return worldPos;
    }

    private void ClearMap(bool finalSwipe)
    {
        Interactable[] interactables = FindObjectsOfType<Interactable>();

        for (int i = 0; i < interactables.Length; i++)
        {
            if(!interactables[i].GetComponent<Mine>() || finalSwipe)
            {
                interactables[i].InitiateTransition(true, true, true);
            }
        }
    }

    private void MineSpawner()
    {
        if(currentSpawnCooldown < currentSpawnRate)
        {
            float tutorialMultiplier = tutorialLevel ? 0f : 1f;
            currentSpawnCooldown += Time.deltaTime * tutorialMultiplier;
            currentSpawnCooldown = Mathf.Min(currentSpawnCooldown, currentSpawnRate);
        }
        else
        {
            SpawnMine();
            currentSpawnCooldown = 0f;
        }
    }

    public void Success(float scoreIncrement, float multiplier)
    {
        //float scoreMultiplier = Mathf.Max(1f, currentLevelIndex / scoreMultiplierThreshold);
        counter.IncrementScore(scoreIncrement, multiplier);
        ClearMap(false);
        StartGame();
    }

    public void SpawnPath()
    {
        if(currentSource && currentDestination)
        {
            currentSource.GetComponent<Interactable>().InitiateTransition(true, true, true);
            currentDestination.GetComponent<Interactable>().InitiateTransition(true, true, true);
        }

        InitializePath();
    }

    private void BoostSpawner()
    {
        if(currentBoostCooldown < boostSpawnRate)
        {
            currentBoostCooldown += Time.deltaTime;
            currentBoostCooldown = Mathf.Min(currentBoostCooldown, boostSpawnRate);
        }
        else
        {
            SpawnBoost();

            currentBoostCooldown = 0f;
        }
    }
    private void SpawnBoost()
    {
        if(currentBoost)
        {
            currentBoost.GetComponent<Interactable>().InitiateTransition(true, true, true);
            //Destroy(currentBoost.gameObject);
        }

        int randomBoostIndex = Mathf.RoundToInt(Random.Range(0f, 1f) * (boostPrefabs.Count - 1f));
        int randomGridIndex = Mathf.RoundToInt(Random.Range(0f, 1f) * (gridSize.x * gridSize.y - 1f));
        Vector2 randomBoostPosition = GetGridPosition(randomGridIndex, tileOffset);
        Vector2 randomOffset = new Vector2(Random.Range(-boostOffset.x, boostOffset.x), Random.Range(-boostOffset.y, boostOffset.y));

        currentBoost = Instantiate(boostPrefabs[randomBoostIndex], new Vector3(randomBoostPosition.x + randomOffset.x, randomBoostPosition.y + randomOffset.y, -5.1f), Quaternion.identity);
        currentBoost.GetComponent<Interactable>().InitiateTransition(false, false, false);
    }

    private void SpawnMine()
    {
        float tileSetIndex = Mathf.Clamp01(Mathf.Floor(presentMines.Count / mineSpawnThreshold));

        if(indices[(int)tileSetIndex].Count > 0f) //PotentialDisposal;
        {
            int randomIndex = Mathf.RoundToInt(Random.Range(0f, 1f) * (indices[(int)tileSetIndex].Count - 1f));
            Vector2 randomMineOffset = new Vector2(Random.Range(-mineOffset.x, mineOffset.x), Random.Range(-mineOffset.y, mineOffset.y));
            Vector2 minePosition = GetGridPosition(indices[(int)tileSetIndex][randomIndex], tileOffset);

            Mine currentMine = Instantiate(minePrefab);
            currentMine.SetIndex((int)tileSetIndex, indices[(int)tileSetIndex][randomIndex]);

            currentMine.transform.position = new Vector3(minePosition.x + randomMineOffset.x, minePosition.y + randomMineOffset.y, -5f);

            indices[(int)tileSetIndex].RemoveAt(randomIndex);
        }
    }

    public Vector2 GetGridPosition(int index, Vector2 offset)
    {
        float x = Mathf.Floor((float)index / gridSize.y);
        float y = (index % gridSize.y);

        Vector2 gridToWorld = GridToWorld(new Vector2(x, y), offset, true);

        return gridToWorld;
    }

    public int GetGridIndex(Vector2 gridPos)
    {
        float x = gridPos.x * gridSize.y;
        float y = gridPos.y;

        return (int)x + (int)y; //ChangeTheFormulaAccordingly;
    }
}

[System.Serializable]
public class SpecialTile
{
    public Tile specialTilePrefab;
    public float chance = 0.3f;
}

public class TileContainer
{
    public List<Tile> tiles = new List<Tile>();
}
 