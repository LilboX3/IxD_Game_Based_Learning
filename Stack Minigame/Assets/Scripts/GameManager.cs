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
    [SerializeField] private StackInitializer stackInitializer;

    [Header("Ingredients")]
    [Tooltip("All ingredient names that can appear in the game.")]
    [SerializeField] private string[] allIngredientNames;

    public enum GameState { Idle, Playing, Won, WrongOrder, Lost }

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
        chefRat.DeliveryStack.OnStackChanged.AddListener(OnDeliveryStackChanged);
        StartGame();
    }

    private void OnDestroy()
    {
        if (chefRat != null && chefRat.DeliveryStack != null)
        {
            chefRat.DeliveryStack.OnStackChanged.RemoveListener(OnDeliveryStackChanged);
        }
    }

    private void Update()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        TimeRemaining -= Time.deltaTime;
        OnTimerUpdated?.Invoke(TimeRemaining / gameDuration);

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndGame(GameState.Lost);
        }
    }

    public void StartGame()
    {
        TimeRemaining = gameDuration;
        State = GameState.Playing;

        chefRat.GenerateRecipe(new List<string>(allIngredientNames));
        StartCoroutine(stackInitializer.InitializeStacks(allIngredientNames, chefRat.Recipe));

        OnGameStarted?.Invoke();
    }

    public void RestartGame() => StartGame();

    private void OnDeliveryStackChanged()
    {
        if (State != GameState.Playing) return;
        if (!chefRat.DeliveryStack.IsFull) return;

        EndGame(chefRat.ValidateDeliveryStack() ? GameState.Won : GameState.WrongOrder);
    }

    private void EndGame(GameState result)
    {
        State = result;
        Debug.Log($"[GameManager] Game ended: {result}");
        OnGameEnded?.Invoke(result);
    }
}
