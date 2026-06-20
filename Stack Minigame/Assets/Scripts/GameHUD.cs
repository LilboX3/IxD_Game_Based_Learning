using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("Recipe")]
    [SerializeField] private TextMeshProUGUI recipeText;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider timerSlider;

    [Header("Result Screen")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button restartButton;

    private GameManager _gm;
    private ChefRat _chefRat;

    private void Awake()
    {
        _gm = GameManager.Instance;
        _chefRat = FindFirstObjectByType<ChefRat>();

        _gm.OnGameStarted += HandleGameStarted;
        _gm.OnGameEnded += HandleGameEnded;
        _gm.OnTimerUpdated += HandleTimerUpdated;
    }

    private void Start()
    {
        resultPanel.SetActive(false);
        restartButton.onClick.AddListener(_gm.RestartGame);

        if (_gm.State == GameManager.GameState.Playing)
        {
            RefreshRecipeDisplay();
        }
    }

    private void OnDestroy()
    {
        if (_gm == null)
        {
            return;
        }
        _gm.OnGameStarted -= HandleGameStarted;
        _gm.OnGameEnded -= HandleGameEnded;
        _gm.OnTimerUpdated -= HandleTimerUpdated;
    }

    private void HandleGameStarted()
    {
        resultPanel.SetActive(false);
        RefreshRecipeDisplay();
        SetTimer(1f); // full bar at start
    }

    private void HandleTimerUpdated(float normalised)
    {
        SetTimer(normalised);
    }

    private void HandleGameEnded(GameManager.GameState result)
    {
        resultPanel.SetActive(true);
        resultText.text = result switch
        {
            GameManager.GameState.Won        => "Perfect order!",
            GameManager.GameState.WrongOrder => "Wrong order - try again!",
            GameManager.GameState.Overflow => "Stack Overflow - try again!",
            _                                => "Time's up!"
        };
    }

    private void RefreshRecipeDisplay()
    {
        if (_chefRat == null || recipeText == null)
        {
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("<b>Chef's order:</b>");
        var recipe = _chefRat.Recipe;
        for (int i = 0; i < recipe.Count; i++)
        {
            sb.AppendLine($"  {i + 1}. {recipe[i]}");
        }

        recipeText.text = sb.ToString();
    }

    private void SetTimer(float normalised)
    {
        float seconds = _gm.TimeRemaining;
        if (timerText != null)
        {
            timerText.text = $"{Mathf.CeilToInt(seconds)}s";
        }
        if (timerSlider != null)
        {
            timerSlider.value = normalised;
        }
    }
}
