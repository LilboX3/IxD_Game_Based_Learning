using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 60f;

    [Header("References")]
    [SerializeField] private ChefRat chefRat;

    [Header("Ingredients")]
    [Tooltip("All ingredient names that can appear in the game.")]
    [SerializeField] private string[] allIngredientNames;

    public enum GameState { Idle, Playing, Won, Lost }

    public GameState State { get; private set; } = GameState.Idle;
    public float TimeRemaining { get; private set; }
    public float GameDuration => gameDuration;

    public event Action OnGameStarted;
    public event Action<GameState> OnGameEnded;
    public event Action<float> OnTimerUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        if (chefRat.ValidateDeliveryStack())
        {
            EndGame(GameState.Won);
            return;
        }

        TimeRemaining -= Time.deltaTime;
        OnTimerUpdated?.Invoke(TimeRemaining / gameDuration);

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndGame(chefRat.ValidateDeliveryStack() ? GameState.Won : GameState.Lost);
        }
    }

    public void StartGame()
    {
        TimeRemaining = gameDuration;
        State = GameState.Playing;

        chefRat.GenerateRecipe(new List<string>(allIngredientNames));

        OnGameStarted?.Invoke();
    }

    public void RestartGame() => StartGame();

    private void EndGame(GameState result)
    {
        State = result;
        Debug.Log($"[GameManager] Game ended: {result}");
        OnGameEnded?.Invoke(result);
    }
}
