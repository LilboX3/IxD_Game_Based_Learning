using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class IngredientStack : MonoBehaviour
{
    public enum StackMode
    {
        Free,
        PushOnly
    }

    [SerializeField] private StackMode mode = StackMode.Free;
    [SerializeField] private int maxSize = 5;

    [Header("Interaction Hint")]
    [SerializeField] private TextMeshPro hintText;

    public bool CanPush => !IsFull;

    public bool CanPop => !IsEmpty && mode == StackMode.Free;

    public int Count => _items.Count;
    public bool IsEmpty => _items.Count == 0;
    public bool IsFull => _items.Count >= maxSize;
    public int MaxSize { get => maxSize; set => maxSize = value; }
    public StackMode Mode => mode;

    public UnityEvent OnStackChanged;

    private readonly List<Ingredient> _items = new();
    private bool _showingInteractionHint = false;

    private void OnEnable()
    {
        OnStackChanged.AddListener(UpdateCountDisplay);
    }

    private void OnDisable()
    {
        OnStackChanged.RemoveListener(UpdateCountDisplay);
    }

    private void Start()
    {
        UpdateCountDisplay();
    }

    public bool Push(Ingredient ingredient)
    {
        if (IsFull) return false;
        _items.Add(ingredient);
        OnStackChanged?.Invoke();
        return true;
    }

    public Ingredient Pop()
    {
        if (IsEmpty) return null;
        var top = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        OnStackChanged?.Invoke();
        return top;
    }

    public Ingredient Peek() => IsEmpty ? null : _items[^1];

    public IReadOnlyList<Ingredient> GetItems() => _items.AsReadOnly();

    public void ShowHint(string text)
    {
        if (hintText == null) return;
        _showingInteractionHint = true;
        hintText.text = text;
        hintText.gameObject.SetActive(true);
    }

    public void HideHint()
    {
        _showingInteractionHint = false;
        UpdateCountDisplay();
    }

    private void UpdateCountDisplay()
    {
        if (hintText == null || _showingInteractionHint) return;
        hintText.text = $"{Count}/{MaxSize}";
        hintText.gameObject.SetActive(true);
    }
}
