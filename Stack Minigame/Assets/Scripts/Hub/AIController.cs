using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    [Header("Interaction Hint")]
    [SerializeField] private TextMeshPro aiText;
    [SerializeField] private GameObject chatPanel;

    private bool _showingAiText = false;

    public bool isAiOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        aiText.gameObject.SetActive(_showingAiText);
        chatPanel.SetActive(isAiOpen);
    }

    public void OpenAIWindow()
    {
        isAiOpen = true;
    }

    public void CloseAIWindow()
    {
        isAiOpen = false;
    }

    public void ShowText()
    {
        _showingAiText = true;
    }

    public void HideText()
    {
        _showingAiText = false;
    }
}