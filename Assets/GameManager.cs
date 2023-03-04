using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isGame = true;
    public static event System.Action onGameStart;
    public static event System.Action onGameOver;
    public static float highScore = 3f;
    public static float currentScore = 0f;

    [SerializeField] RectTransform highScoreBar = default;
    [SerializeField] TextMeshProUGUI highScoreText = default;
    [SerializeField] RectTransform currentScoreBar = default;
    [SerializeField] TextMeshProUGUI currentScoreText = default;

    public static List<TetrisCat> freeCats = new List<TetrisCat>();

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    private void Start() {
        if(!isGame){
            isGame = true;
            onGameStart?.Invoke();
        }
    }

    private void Update() {
        currentScore = 0;
        foreach(TetrisCat cat in freeCats){
            currentScore = Mathf.Max(currentScore, cat.Height() + 3);
        }

        if(currentScore > highScore){
            highScore = currentScore;
        }
        float p = 10;
        highScoreBar.localPosition = (highScore * 100 - 300) * Vector3.up;
        highScoreText.text = "High Score: " + Mathf.Floor(highScore*p)/p;

        currentScoreBar.localPosition = (currentScore * 100 - 300) * Vector3.up;
        currentScoreText.text = "Current Score: " + Mathf.Floor(currentScore*p)/p;
    }
}
