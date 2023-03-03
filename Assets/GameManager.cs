using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isGame = true;
    public static event System.Action onGameStart;
    public static event System.Action onGameOver;

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
}
