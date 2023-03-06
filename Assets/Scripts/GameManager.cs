using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static float highScore = 4f;
    public static float currentScore = 0f;
    public static float currentHighScore = 0f;
    static bool firstBreak = true;
    public static GameState currentState = GameState.Menu;

    [SerializeField] RectTransform highScoreBar = default;
    [SerializeField] TextMeshProUGUI highScoreText = default;
    [SerializeField] RectTransform currentScoreBar = default;
    [SerializeField] TextMeshProUGUI currentScoreText = default;
    [SerializeField] TextMeshProUGUI gameOverText = default;
    [SerializeField] TextMeshProUGUI catLivesText = default;

    [SerializeField] GameObject gameScreen = default;
    [SerializeField] GameObject gameOverScreen = default;
    [SerializeField] GameObject pauseScreen = default;
    [SerializeField] GameObject menuScreen = default;

    public static List<TetrisCat> freeCats = new List<TetrisCat>();
    
    public static event System.Action onGameStart;
    public static event System.Action onGamePause;
    public static event System.Action onGameUnPause;
    public static event System.Action onGameOver;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    private void Start() {
    }

    public static void UnPauseGame(){
        onGameUnPause?.Invoke();
        instance.pauseScreen.SetActive(false);
        currentState = GameState.Game;
    }

    public static void PauseGame(){
        onGamePause?.Invoke();
        instance.pauseScreen.SetActive(true);
        currentState = GameState.Pause;
    }

    public static void StartGame(){
        onGameStart?.Invoke();
        freeCats.Clear();
        instance.gameScreen.SetActive(true);
        instance.gameOverScreen.SetActive(false);
        instance.pauseScreen.SetActive(false);
        instance.menuScreen.SetActive(false);
        instance.highScoreText.gameObject.SetActive(true);
        instance.highScoreBar.gameObject.SetActive(true);
        instance.currentScoreBar.gameObject.SetActive(true);
        instance.currentScoreText.gameObject.SetActive(true);
        deadCats = 0;
        currentScore = 0;
        currentHighScore = 0;
        firstBreak = true;
        currentState = GameState.Game;
    }

    public static int deadCats = 0;

    public static void DeadCat(){
        deadCats++;
        instance.catLivesText.text = "" + (9-deadCats);
        if(deadCats >= 9){
            EndGame();
        }
    }
    static float p = 10;

    public static void EndGame(){
        onGameOver?.Invoke();
        instance.gameOverScreen.SetActive(true);
        instance.gameOverText.text = "Score: " + Mathf.Floor(currentHighScore*p)/p;
        currentState = GameState.GameOver;
    }

    private void Update() {
        instance.catLivesText.text = "" + (9-deadCats);
        if(currentState == GameState.Game){
            UpdateScores();
            UpdateScoreUI();
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
                PauseGame();
            }
        }else if(currentState == GameState.Pause){
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
                UnPauseGame();
            }
        }
    }


    void UpdateScores(){
        currentScore = 0;
        foreach(TetrisCat cat in freeCats){
            currentScore = Mathf.Max(currentScore, cat.Height());
        }
        currentHighScore = Mathf.Max(currentHighScore, currentScore);

        if(currentScore > highScore){
            highScore = currentScore;
            if(firstBreak){
                AudioManager.Reward();
                firstBreak = false;
                highScoreText.gameObject.SetActive(false);
                highScoreBar.gameObject.SetActive(false);
            }
        }
    }

    void UpdateScoreUI(){
        highScoreBar.localPosition = highScore * Vector3.up;
        highScoreText.text = "High Score: " + Mathf.Floor(highScore*p)/p;

        currentScoreBar.localPosition = currentHighScore * Vector3.up;
        currentScoreText.text = "Score: " + Mathf.Floor(currentHighScore*p)/p;

    }
}

public enum GameState{
    Game, Pause, Menu, GameOver
}
