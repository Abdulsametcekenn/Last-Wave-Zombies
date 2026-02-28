using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie Prefabs")]
    public GameObject normalZombiePrefab;
    public GameObject bomberZombiePrefab;
    public GameObject fasterZombiePrefab;
    public GameObject bossZombiePrefab;

    [Header("Spawn Settings")]
    public float spawnDistanceFromCamera = 2f; 
    public int spawnBatchSize = 4;             
    public float spawnBatchDelay = 2f;         
    [Header("UI")]
    public TMP_Text wavetext;
    public TMP_Text ZombieCount;
    public GameObject TradeShop;
    [Header("Drop")]
    public GameObject SaðlýkPrefab;
    public GameObject MermiPrefab;

    private Camera mainCamera;

    private int wave = 0;                     
    private int zombiesPerWave = 0;            
    private int aliveZombies = 0;              
    private float bomberChance = 0f;           
    private float fasterChance = 0f;         

    public bool waveActive = false;           
    private int totalSpawnedZombiesThisWave = 0;  

    private GunsManager gunsManager;
    Vector2 MermiSpawn=new Vector2(0, 0); 
    Vector2 SaðlýkSpawn=new Vector2(2, 0);
    private bool hasSpawned = false;

    void Start()
    {
        mainCamera = Camera.main;
        TradeShop.SetActive(false);
        gunsManager = FindAnyObjectByType<GunsManager>();

    }

    void Update()
    {
       
        ZombieCount.text = aliveZombies + " / " + (zombiesPerWave - totalSpawnedZombiesThisWave);

        if (!waveActive && aliveZombies == 0)
        {
            TradeShop.SetActive(true); 

        }
        else
        {
            TradeShop.SetActive(false);
        }
        if (!waveActive && wave >= 1 && !hasSpawned)
        {
            Instantiate(MermiPrefab,MermiSpawn, Quaternion.identity);
            Instantiate(SaðlýkPrefab, SaðlýkSpawn, Quaternion.identity);
            hasSpawned = true;
        }
    }

    public void StartNewWave()
    {
        wave++;                    
        zombiesPerWave += 4;       
        bomberChance += 5f;       
        fasterChance += 3f;       
        totalSpawnedZombiesThisWave = 0;
        waveActive = true;

        wavetext.text = "Wave: " + wave;
        StartCoroutine(SpawnWaveBatches());

        if (gunsManager != null)
        {
            gunsManager.ResetHealthWeapon();
        }
    }

    IEnumerator SpawnWaveBatches()
    {
        int bossCount = 0;

        if (wave >= 10)
            bossCount = 1 + (wave / 5); 

        for (int i = 0; i < bossCount; i++)
        {
            SpawnZombie(bossZombiePrefab);
        }

        int zombiesSpawned = 0;

        while (zombiesSpawned < zombiesPerWave)
        {
            int batchCount = Mathf.Min(spawnBatchSize, zombiesPerWave - zombiesSpawned);

            for (int i = 0; i < batchCount; i++)
            {
                float rand = Random.Range(0f, 100f);

                if (wave >= 7 && rand < fasterChance && fasterZombiePrefab != null)
                    SpawnZombie(fasterZombiePrefab);
                else if (wave >= 3 && rand < bomberChance && bomberZombiePrefab != null)
                    SpawnZombie(bomberZombiePrefab);
                else
                    SpawnZombie(normalZombiePrefab);

                zombiesSpawned++;
                totalSpawnedZombiesThisWave++;
            }

            yield return new WaitForSeconds(spawnBatchDelay);
        }

        waveActive = false; 
    }

    void SpawnZombie(GameObject prefab)
    {
        Vector2 spawnPos = GetRandomPositionOutsideCamera(); 
        Instantiate(prefab, spawnPos, Quaternion.identity);
        aliveZombies++; 
    }

    Vector2 GetRandomPositionOutsideCamera()
    {
        Vector3 camPos = mainCamera.transform.position;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float minX = camPos.x - camWidth / 2 - spawnDistanceFromCamera;
        float maxX = camPos.x + camWidth / 2 + spawnDistanceFromCamera;
        float minY = camPos.y - camHeight / 2 - spawnDistanceFromCamera;
        float maxY = camPos.y + camHeight / 2 + spawnDistanceFromCamera;

        Vector2 spawnPos = Vector2.zero;
        int side = Random.Range(0, 4); // 0: sol, 1: sað, 2: alt, 3: üst

        switch (side)
        {
            case 0:
                spawnPos.x = minX;
                spawnPos.y = Random.Range(minY, maxY);
                break;
            case 1:
                spawnPos.x = maxX;
                spawnPos.y = Random.Range(minY, maxY);
                break;
            case 2:
                spawnPos.x = Random.Range(minX, maxX);
                spawnPos.y = minY;
                break;
            case 3:
                spawnPos.x = Random.Range(minX, maxX);
                spawnPos.y = maxY;
                break;
        }

        return spawnPos;
    }

    public void ZombieDied()
    {
        aliveZombies--;
        if (aliveZombies < 0) aliveZombies = 0; 
    }
}
