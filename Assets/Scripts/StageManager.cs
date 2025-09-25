using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    // 조명 관련
    public Light2D globalLight;

    public float minLight = 0.2f;
    public float maxLight = 1.0f;

    // 몹 관련
    public int totalE;
    private int deathE = 0;

    private int[] stageList = { 1, 3, 5 };

    private int i = 0;

    public GameObject enemyPrefabs;

    // 플레이어
    public GameObject player;
    PlayerMove pm;

    public float flashlightDamage = 1f;

    // ---

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } 
        Instance = this;

        if (globalLight != null)
            globalLight.intensity = minLight;
    }

    private void Start()
    {
        totalE = stageList[i];
        pm = player.GetComponent<PlayerMove>();
        flashlightDamage = 1f;
    }

    public void EnemyDefeated()
    {
        deathE++;
        UpdateLight();

        if(deathE == totalE)
        {
            StageClear();
        }
    }

    void UpdateLight()
    {
        if (globalLight == null) return;

        float progress = Mathf.Clamp01((float)deathE / totalE);
        globalLight.intensity = Mathf.Lerp(minLight, maxLight, progress);
    }

    void StageClear()
    {
        pm.battery = pm.maxBattery;
        i++;
        if (i < 3)
        {
            GenStage();
        }
    }

    void GenStage()
    {
        deathE = 0;
        totalE = stageList[i];

        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float xMin = cam.transform.position.x - camWidth / 2f;
        float xMax = cam.transform.position.x + camWidth / 2f;
        float yMin = cam.transform.position.y - camHeight / 2f;
        float yMax = cam.transform.position.y + camHeight / 2f;

        for (int n = 0; n < totalE; n++)
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(xMin, xMax),
                Random.Range(yMin, yMax)
            );

            Instantiate(enemyPrefabs, spawnPos, Quaternion.identity);
        }

        if (globalLight != null)
            globalLight.intensity = minLight;
    }
}
