using System.Collections;
using UnityEngine;

public class SaboteurRat : MonoBehaviour
{
    [Header("Stacks")]
    [Tooltip("All ingredient source stacks in the scene (not the delivery stack).")]
    [SerializeField] private IngredientStack[] sourceStacks;

    [Header("References")]
    [SerializeField] private StackInitializer stackInitializer;

    [Header("Sabotage Settings")]
    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 12f;

    [Header("Prefab")]
    [SerializeField] private GameObject ingredientPrefab;


    public IEnumerator SabotageLoop(string[] allIngredients)
    {
        while (GameManager.Instance.State == GameManager.GameState.Playing)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            Sabotage(allIngredients);

        }
    }

    private void Sabotage(string[] allIngredients)
    {
        IngredientStack targetStack =
            sourceStacks[Random.Range(0, sourceStacks.Length)];

        //Debug.Log($"[SaboteurRat] New stack: {targetStack}");

        if (targetStack.IsFull)
        {
            return;
        }

        string ingredient =
            allIngredients[Random.Range(0, allIngredients.Length)];

        //Debug.Log($"[SaboteurRat] New ingredient: {ingredient}");

        bool success = SpawnIngredient(ingredient, targetStack);

        if (success)
        {
            Debug.Log($"Saboteur added {ingredient} to {targetStack.name}");

            targetStack.ShowPushHint(
                $"{targetStack.Count}. {ingredient} sabotaged!"
            );

            StartCoroutine(HideHintAfterDelay(targetStack));
        }
    }

    private IEnumerator HideHintAfterDelay(IngredientStack stack)
    {
        yield return new WaitForSeconds(2f);
        stack.HidePushHint();
    }

    private bool SpawnIngredient(string ingredientName, IngredientStack stack)
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

        return stack.Push(ingredient);
    }
}