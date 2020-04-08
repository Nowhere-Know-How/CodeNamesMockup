using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates {
    INTRO, SPHERE, CAPSULE, CUBE
}

public delegate void OnStateChangeHandler();

public class GameStateManager : Object
{
    protected GameStateManager() {}
    private static GameStateManager instance=null;
    public event OnStateChangeHandler OnStateChange;
    public GameStates gameState { get; private set;}

    public static GameStateManager Instance{
        get {
            if (GameStateManager.instance == null){
                GameStateManager.instance = new GameStateManager();
                // DontDestroyOnLoad(GameStateManager.instance);
            }
            return GameStateManager.instance;
        }
    }

    public void SetGameState(GameStates state){
        this.gameState = state;
        OnStateChange();
    }

    public void OnApplicationQuit(){
        GameStateManager.instance = null;
    }

}
