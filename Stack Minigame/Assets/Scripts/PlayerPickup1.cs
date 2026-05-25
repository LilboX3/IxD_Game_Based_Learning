using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform carryPoint;

    private Ingredient carriedIngredient = null;
    private Ingredient nearbyIngredient = null;

    void Update()
    {
        //pick ingredient up
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyIngredient != null && carriedIngredient == null)
            {
                PickupIngredient(nearbyIngredient);
            }
        }

        // drop ingredient
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (carriedIngredient != null)
            {
                DropIngredient();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            nearbyIngredient = collision.gameObject.GetComponent<Ingredient>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ingredient"))
        {
            nearbyIngredient = null;
        }
    }

    void PickupIngredient(Ingredient ingredient)
    {
        carriedIngredient = ingredient;

        ingredient.transform.position = carryPoint.position;
        ingredient.transform.SetParent(carryPoint);

        Debug.Log("Picked up " + ingredient.ingredientName);
    }

    void DropIngredient()
    {
        carriedIngredient.transform.SetParent(null);

        carriedIngredient = null;

        Debug.Log("Dropped ingredient");
    }
}