using System.Collections.Generic;
using UnityEngine;

public class StackInitializer : MonoBehaviour
{
    [Header("Stacks")]
    [Tooltip("All ingredient source stacks in the scene (not the delivery stack).")]
    [SerializeField] private IngredientStack[] sourceStacks;
    [SerializeField] private IngredientStack deliveryStack;

    [Header("Prefab")]
    [SerializeField] private GameObject ingredientPrefab;

    [Header("Fill Settings")]
    [SerializeField, Range(0f, 1f)] private float stackFillChance = 0.7f;
    [SerializeField] private int minFillCount = 1;
    [SerializeField] private int maxFillCount = 3;

    public void InitializeStacks(string[] allIngredients, IReadOnlyList<string> requiredIngredients)
    {
        ClearAllStacks();

        var shuffledStacks = new List<IngredientStack>(sourceStacks);
        Shuffle(shuffledStacks);

        var requiredTargets = new List<IngredientStack>(shuffledStacks);
        foreach (string required in requiredIngredients)
        {
            IngredientStack target;
            if (requiredTargets.Count > 0)
            {
                target = requiredTargets[0];
                requiredTargets.RemoveAt(0);
            }
            else
            {
                target = shuffledStacks.Find(s => !s.IsFull);
            }

            if (target != null)
            {
                SpawnIngredient(required, target);
            }
            else
            {
                Debug.LogWarning($"[StackInitializer] All stacks full — could not place required ingredient '{required}'.");
            }
        }

        foreach (IngredientStack stack in sourceStacks)
        {
            if (Random.value > stackFillChance)
            {
                continue;
            }

            int fillCount = Random.Range(minFillCount, maxFillCount + 1);
            for (int i = 0; i < fillCount; i++)
            {
                if (stack.IsFull)
                {
                    break;
                }

                string name = allIngredients[Random.Range(0, allIngredients.Length)];
                SpawnIngredient(name, stack);
            }
        }
    }

    private void ClearAllStacks()
    {
        foreach (IngredientStack stack in sourceStacks)
        {
            ClearStack(stack);
        }

        if (deliveryStack != null)
        {
            ClearStack(deliveryStack);
        }
    }

    private void ClearStack(IngredientStack stack)
    {
        while (!stack.IsEmpty)
        {
            Ingredient ingredient = stack.Pop();
            if (ingredient != null)
            {
                Destroy(ingredient.gameObject);
            }
        }
    }

    private void SpawnIngredient(string ingredientName, IngredientStack stack)
    {
        GameObject go = Instantiate(
            ingredientPrefab,
            stack.transform.position,
            Quaternion.identity,
            stack.transform
        );

        Ingredient ingredient = go.GetComponent<Ingredient>();
        ingredient.ingredientName = ingredientName;
        ingredient.SetVisible(false);

        stack.Push(ingredient);
    }

    private static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
