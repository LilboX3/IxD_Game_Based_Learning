using System.Collections;
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

    public IEnumerator InitializeStacks(string[] allIngredients, IReadOnlyList<string> requiredIngredients)
    {
        ClearAllStacks();

        var shuffledStacks = new List<IngredientStack>(sourceStacks);
        Shuffle(shuffledStacks);

        var shuffledRequired = new List<string>(requiredIngredients);
        Shuffle(shuffledRequired);

        for (int i = 0; i < shuffledRequired.Count; i++)
        {
            IngredientStack target = null;
            int startIdx = i % shuffledStacks.Count;
            for (int attempt = 0; attempt < shuffledStacks.Count; attempt++)
            {
                var candidate = shuffledStacks[(startIdx + attempt) % shuffledStacks.Count];
                if (!candidate.IsFull)
                {
                    target = candidate;
                    break;
                }
            }

            if (target != null)
            {
                SpawnIngredient(shuffledRequired[i], target);

                target.ShowPushHint(
                    $"{i + 1}. {shuffledRequired[i]} pushed"
                );
                yield return new WaitForSeconds(2f);
                target.HidePushHint();
            }
            else
            {
                Debug.LogWarning($"[StackInitializer] All stacks full — could not place required ingredient '{shuffledRequired[i]}'.");
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
