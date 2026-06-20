using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIController : MonoBehaviour
{
    [Header("Interaction Hint")]
    [SerializeField] private TextMeshPro aiText;
    [SerializeField] private GameObject chatPanel;

    [SerializeField] private bool _showingAiText = false;

    public bool isAiOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiText.gameObject.SetActive(_showingAiText);
        chatPanel.SetActive(isAiOpen);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenAIWindow()
    {
        chatPanel.SetActive(true);
        isAiOpen = true;
    }

    public void CloseAIWindow()
    {
        chatPanel.SetActive(false);
        isAiOpen = false;
    }

    public void ShowText()
    {
        aiText.gameObject.SetActive(true);
        _showingAiText = true;
    }

    public void HideText()
    {
        aiText.gameObject.SetActive(false);
        _showingAiText = false;
    }
}