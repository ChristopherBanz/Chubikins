using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController _instance;
    public static GameController Instance { get { return _instance; } }
   

    [SerializeField] private GameObject gameOverUI = null;


    public GameObject[] enemySpawnArray = new GameObject[4];

    public GameObject enemyPrefab;

    public bool isGameOver = false;
    AudioSource audioSource;


    public int score = 0;
    public Text scoreText;
    public float lastSpawn;
    public float enemySpawnDelay = 5f;



    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void SpawnEnemy(Vector3 spawnLocation)
    {
        GameObject.Instantiate(enemyPrefab, spawnLocation, transform.rotation);
        if (enemySpawnDelay > 0.5f)
        {
            enemySpawnDelay -= 0.25f;
        }
        lastSpawn = Time.time;
    }


    private void Update()
    {

        if (Time.time > lastSpawn + enemySpawnDelay)
        { int spawnAt = Random.Range(0, 4);
            SpawnEnemy(enemySpawnArray[spawnAt].transform.position);
        }

        scoreText.text = "Bahn Mi: " + score;
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            gameOverUI.SetActive(true);
            isGameOver = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnLevelWasLoaded(int level)
    {
        score = 0;
        enemySpawnDelay = 5f; 
        gameOverUI.SetActive(false);
    }
}
