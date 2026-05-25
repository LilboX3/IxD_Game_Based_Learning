using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientName = "TEST";

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetVisible(bool visible)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = visible;
        }
    }
}