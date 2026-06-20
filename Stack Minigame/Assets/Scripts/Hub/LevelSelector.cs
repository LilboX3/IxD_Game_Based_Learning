using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [Header("Interaction Hint")]
    [SerializeField] private TextMeshPro levelText;

    [SerializeField] private string textToShow;

    private bool _showingLevelText = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelText.text = textToShow;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.gameObject.SetActive(_showingLevelText);
    }

    public void LoadLevel()
    {
        // Loads Main Scene (stack) per default for now
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowText()
    {
        _showingLevelText = true;
    }

    public void HideText()
    {
        _showingLevelText = false;
    }
}

