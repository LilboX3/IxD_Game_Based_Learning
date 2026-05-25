using System.Collections.Generic;
using UnityEngine;

public class ChefRat : MonoBehaviour
{
    [SerializeField] private IngredientStack deliveryStack;
    [SerializeField] private int recipeLength = 3;

    private readonly List<string> _recipe = new();

    public IReadOnlyList<string> Recipe => _recipe.AsReadOnly();

    public void GenerateRecipe(IReadOnlyList<string> availableIngredients)
    {
        _recipe.Clear();

        var pool = new List<string>(availableIngredients);
        int count = Mathf.Min(recipeLength, pool.Count);

        for (int i = 0; i < count; i++)
        {
            int idx = Random.Range(0, pool.Count);
            _recipe.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        if (deliveryStack != null)
        {
            deliveryStack.MaxSize = _recipe.Count;
        }

        Debug.Log($"[ChefRat] New recipe: {string.Join(" → ", _recipe)}");
    }

    public bool ValidateDeliveryStack()
    {
        if (_recipe.Count == 0 || deliveryStack == null)
        {
            return false;
        }

        var items = deliveryStack.GetItems();

        if (items.Count != _recipe.Count)
        {
            return false;
        }

        for (int i = 0; i < _recipe.Count; i++)
        {
            int stackIndex = items.Count - 1 - i;
            if (items[stackIndex].ingredientName != _recipe[i])
            {
                return false;
            }
        }

        return true;
    }

    public IngredientStack DeliveryStack => deliveryStack;
}
