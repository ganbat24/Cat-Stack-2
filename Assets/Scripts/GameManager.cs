using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static float highScore = 3f;
    public static float currentScore = 0f;
    public static float currentHighScore = 0f;
    public static int deadCats = 0;
    static bool firstBreak = true;
    public static GameState currentState = GameState.Menu;

    [SerializeField] RectTransform highScoreBar = default;
    [SerializeField] TextMeshProUGUI highScoreText = default;
    [SerializeField] RectTransform currentScoreBar = default;
    [SerializeField] TextMeshProUGUI currentScoreText = default;
    [SerializeField] TextMeshProUGUI gameOverText = default;

    [SerializeField] GameObject gameScreen = default;
    [SerializeField] GameObject gameOverScreen = default;
    [SerializeField] GameObject pauseScreen = default;
    [SerializeField] GameObject menuScreen = default;

    [SerializeField] List<GameObject> lives = new List<GameObject>();

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
        Invoke("StartGame", 0.1f);
    }

    public void UnPauseGame(){
        onGameUnPause?.Invoke();
        pauseScreen.SetActive(false);
        currentState = GameState.Game;
    }

    public void PauseGame(){
        onGamePause?.Invoke();
        pauseScreen.SetActive(true);
        currentState = GameState.Pause;
    }

    public void StartGame(){
        onGameStart?.Invoke();
        freeCats.Clear();
        gameScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        menuScreen.SetActive(false);
        highScoreText.gameObject.SetActive(true);
        highScoreBar.gameObject.SetActive(true);
        currentScoreBar.gameObject.SetActive(true);
        currentScoreText.gameObject.SetActive(true);
        foreach(GameObject life in lives){
            life.SetActive(true);
        }
        currentScore = 0;
        currentHighScore = 0;
        deadCats = 0;
        firstBreak = true;
        currentState = GameState.Game;
    }

    public static void DeadCat(){
        instance.lives[deadCats].SetActive(false);
        deadCats++;
        if(deadCats >= 9){
            instance.EndGame();
        }
    }
    float p = 10;

    public void EndGame(){
        onGameOver?.Invoke();
        gameOverScreen.SetActive(true);
        gameOverText.text = "Score: " + Mathf.Floor(currentHighScore*p)/p + "\n You have used up all your 9 cat lives :(";
        currentState = GameState.GameOver;
    }

    private void Update() {
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
                Debug.Log("congrats");
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
        currentScoreText.text = "Current Score: " + Mathf.Floor(currentHighScore*p)/p;
    }
}

public enum GameState{
    Game, Pause, Menu, GameOver
}
