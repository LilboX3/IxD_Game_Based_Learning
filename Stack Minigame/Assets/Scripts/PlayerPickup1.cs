using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;

    private Ingredient _carriedIngredient;
    private IngredientStack _nearbyStack;

    public bool IsCarrying => _carriedIngredient != null;

    public event Action<string> OnInteractionHintChanged;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_carriedIngredient == null && _nearbyStack != null && _nearbyStack.CanPop)
            {
                PopFromStack(_nearbyStack);
            }
            else if (_carriedIngredient != null && _nearbyStack != null && _nearbyStack.CanPush)
            {
                PushToStack(_nearbyStack);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stack"))
        {
            _nearbyStack = collision.GetComponent<IngredientStack>();
            UpdateInteractionHint();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stack") && _nearbyStack != null
            && collision.gameObject == _nearbyStack.gameObject)
        {
            _nearbyStack.HideHint();
            _nearbyStack = null;
        }
    }

    private void PopFromStack(IngredientStack stack)
    {
        Ingredient ingredient = stack.Pop();
        if (ingredient == null)
        {
            return;
        }

        _carriedIngredient = ingredient;
        ingredient.transform.SetParent(carryPoint);
        ingredient.transform.localPosition = Vector3.zero;
        ingredient.SetVisible(true);

        Debug.Log($"[PlayerPickup] Picked up {ingredient.ingredientName} from stack");
        UpdateInteractionHint();
    }

    private void PushToStack(IngredientStack stack)
    {
        if (!stack.Push(_carriedIngredient))
        {
            return;
        }

        StartCoroutine(ShowPushHint(stack, _carriedIngredient.ingredientName));
        _carriedIngredient.transform.SetParent(stack.transform);
        _carriedIngredient.SetVisible(false);
        _carriedIngredient = null;

        Debug.Log($"[PlayerPickup] Pushed ingredient onto stack");
        UpdateInteractionHint();
    }

    private IEnumerator ShowPushHint(IngredientStack stack, string name)
    {
        stack.ShowPushHint($"{name} pushed");
        yield return new WaitForSeconds(2f);
        stack.HidePushHint();

    }

    private void UpdateInteractionHint()
    {
        if (_nearbyStack == null) return;

        if (_carriedIngredient == null && _nearbyStack.CanPop)
        {
            _nearbyStack.ShowHint($"[E] Pop {_nearbyStack.Peek().ingredientName}");
        }
        else if (_carriedIngredient != null && _nearbyStack.CanPush)
        {
            _nearbyStack.ShowHint($"[E] Push {_carriedIngredient.ingredientName}");
        }
        else if (_carriedIngredient != null && _nearbyStack.IsFull)
        {
            _nearbyStack.ShowHint("Stack full");
        }
        else if (_carriedIngredient == null && _nearbyStack.IsEmpty
            && _nearbyStack.Mode == IngredientStack.StackMode.Free)
        {
            _nearbyStack.ShowHint("Stack empty");
        }
        else
        {
            _nearbyStack.HideHint();
        }
    }
}