using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //hit screne
    [SerializeField] private GameObject hitScreen;
    //gameover screen
    [SerializeField] private GameObject gameOverScreen;
    //menu screen
    [SerializeField] private GameObject gameMenuScreen;
    //plane life
    [SerializeField] private TextMeshProUGUI planeLifePercentagetxt;
    private int planeLifePercentage;
    //score
    [SerializeField] private TextMeshProUGUI scoreText;
    //high score
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        Actions.onPlaneCrash +=  PlaneCrash;
        Actions.onMissileHit += MissileHit;
        Actions.onGameStart += StartGame;
        Actions.onCoinCollected += CoinCollected;
        Actions.onBoostPicked += Boost;
        gameMenuScreen.SetActive(true);
        highScoreText.text = "HIGH SCORE : " + PlayerPrefs.GetInt("HighScore");
    }

    public void StartGame()
    {
        planeLifePercentage = 100;
        planeLifePercentagetxt.text = planeLifePercentage + "%".ToString();
        gameMenuScreen.SetActive(false);
    }

    void PlaneCrash()
    {
        StartCoroutine(Crash());
    }

    void CoinCollected()
    {
        scoreText.text = GameManager.instance.score.ToString();
    }

    IEnumerator Crash()
    {
        yield return new WaitForSeconds(2f);
        gameOverScreen.SetActive(true);
    }

    void MissileHit()
    {
        StartCoroutine(Hit());
    }

    IEnumerator Hit()
    {
        planeLifePercentage = planeLifePercentage - 30;
        planeLifePercentagetxt.text = planeLifePercentage + "%".ToString();
        hitScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        hitScreen.SetActive(false);
    }

    void Boost()
    {
        planeLifePercentage = 100;
        planeLifePercentagetxt.text = planeLifePercentage + "%".ToString();
    }

    private void OnDisable()
    {
        Actions.onPlaneCrash -= PlaneCrash;
        Actions.onMissileHit -= MissileHit;
        Actions.onGameStart -= StartGame;
        Actions.onCoinCollected -= CoinCollected;
    }
}
