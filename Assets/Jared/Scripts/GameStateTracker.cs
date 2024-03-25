using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateTracker : MonoBehaviour
{
    public enum GameState { Playing, Win, Fail }
    [SerializeField]
    private GameState _state = GameState.Playing;

    [SerializeField]
    private float GameLength = 30;
    private float GameStart;

    [SerializeField]
    Transform BioMassParent;

    [SerializeField]
    Unit Capital;
    private void Awake()
    {
        GameStart = Time.time;
    }
    private void OnCapitalKilled(Unit destroyedUnit)
    {
        _state = GameState.Fail;
    }
    private void OnEnable()
    {
        Capital.OnKilled += OnCapitalKilled;
    }
    private void OnDisable()
    {
        Capital.OnKilled -= OnCapitalKilled;
    }
    void Update()
    {
        if(BioMassParent.childCount== 0)
        {
            _state = GameState.Win;
        }
        else if(GetGameTime()<=0)
        {
            _state = GameState.Fail;
        }


        switch(_state)
        {
            case GameState.Win:
                SceneManager.LoadScene("WinScene");
                break;
            case GameState.Fail:
                SceneManager.LoadScene("FailScene");
                break;
            case GameState.Playing:
                break;
        }
    }
    /// <summary>
    /// Get the current time left in the game.
    /// </summary>
    /// <returns>Game time left in seconds.</returns>
    public float GetGameTime()
    {
        return GameLength - (Time.time - GameStart);
    }
}
