using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public static class Actions
{
    //action events
    public static Action onMissileHit;
    public static Action onPlaneCrash;
    public static Action onGameStart;
    public static Action onCoinCollected;
    public static Action onBoostPicked;
    public static Action onPoweUp;
}

public class GameManager : MonoBehaviour
{
    //instance
    public static GameManager instance;

    [Header("PREFABS")]
    public GameObject plane;
    public GameObject misssile;
    public GameObject coin;
    public GameObject boost;
    public GameObject powerUp;
    [Header("SCORE")]
    public int score = 0;
    private int highScore;
    [Space(10)]
    //checks wheather game started or not
    private bool gameStarted = false;
    //calculation to spawn boost
    int boostSpawnCal;
    //calculation to spawn powerUp
    int powerUpSpawnCal;

    private void OnEnable()
    {
        Actions.onPlaneCrash += StopSpawning;
        Actions.onCoinCollected += CoinCollected;
        Actions.onPoweUp += PoweUpPickedUp;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameStarted = false;
        //spawning missiles at random intervals
        InvokeRepeating("SpawnMissiles", 1, Random.Range(3, 8));
        InvokeRepeating("SpawnCoins",1 , Random.Range(7, 11));
    }

    public void StartGame()
    {
        Actions.onGameStart();
        gameStarted = true;
    }

    void SpawnMissiles()
    {
        if(gameStarted)
        {
            int spawnDir = Random.Range(0,3);
            Vector2 pos = new Vector2();

            switch (spawnDir)
            {
                case 0:
                    //setting random X position on the screen and a static Y position (FROM TOP)
                    pos = new Vector2(Random.Range(-1.9f, 2f), 5.7f);
                    break;

                case 1:
                    //setting random Y position on the screen and a static X position (FROM LEFT)
                    pos = new Vector2(-3, Random.Range(-3f, 4f));
                    break;

                case 2:
                    //setting random Y position on the screen and a static X position (FROM RIGHT)
                    pos = new Vector2(3, Random.Range(-3f, 4f));
                    break;
            }

            GameObject _missile = Instantiate(misssile, pos, Quaternion.identity);
            MissileScrpt missileScrpt = _missile.GetComponent<MissileScrpt>();
            missileScrpt.target = plane.transform;
            Destroy(_missile, 1.5f);

            //checks for spawning power Up
            powerUpSpawnCal++;
            if (powerUpSpawnCal >= 9)
            {
                powerUpSpawnCal = 0;
                SpawnBoost();
            }
        }    
    }

    void SpawnCoins()
    {
        if (gameStarted)
        {
            //setting random X position on the screen and a static Y position
            Vector2 pos = new Vector2(Random.Range(-1.9f, 2f), 5.7f);
            GameObject _coin = Instantiate(coin, pos, Quaternion.identity);
            Destroy(_coin,10f);
            //checks for spawning boost
            boostSpawnCal++;
            if(boostSpawnCal >= 7)
            {
                boostSpawnCal = 0;
                SpawnPoweUps();
            }
        }
    }

    void SpawnBoost()
    {
        if (gameStarted)
        {
            //setting random X position on the screen and a static Y position
            Vector2 pos = new Vector2(Random.Range(-1.9f, 2f), 5.7f);
            GameObject _boost = Instantiate(boost, pos, Quaternion.identity);
            Destroy(_boost,10f);
        }
    }

    void SpawnPoweUps()
    {
        if (gameStarted)
        {
            //setting random X position on the screen and a static Y position
            Vector2 pos = new Vector2(Random.Range(-1.9f, 2f), 5.7f);
            GameObject _power = Instantiate(powerUp, pos, Quaternion.identity);
            Destroy(_power, 10f);
        }
    }

    void StopSpawning()
    {
        //checks wheather the current score is high score
        highScore = PlayerPrefs.GetInt("HighScore");
        if (highScore <= score)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        //cancelling all invoke
        CancelInvoke();
    }

    void PoweUpPickedUp()
    {
        StartCoroutine(PowerUp());
    }

    IEnumerator PowerUp()
    {
        //sets the plane's speed double
        PlaneController planeController = plane.GetComponent<PlaneController>();
        planeController.moveSpeed = planeController.moveSpeed * 2;
        //sets the plane's speed to normal
        yield return new WaitForSeconds(5);
        planeController.moveSpeed = planeController.moveSpeed / 2;
    }

    void CoinCollected()
    {
        //coin collect
        score = score + 10;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        Actions.onPlaneCrash -= StopSpawning;
        Actions.onCoinCollected -= CoinCollected;
        Actions.onPoweUp -= PoweUpPickedUp;
    }

}
