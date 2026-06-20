using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPlayerController : MonoBehaviour
{
    public float speed;

    private Animator animator;

    private LevelSelector _nearbyLevel;
    private AIController _nearbyAI;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        // for Hub Level Select
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_nearbyLevel != null) _nearbyLevel.LoadLevel();
            else if (_nearbyAI != null)
            {
                if (!_nearbyAI.isAiOpen) _nearbyAI.OpenAIWindow();
                else _nearbyAI.CloseAIWindow();
            }
        }
        if (_nearbyAI != null && _nearbyAI.isAiOpen) return;

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().linearVelocity = speed * dir;
    }


    //For hub level selection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LevelSelect"))
        {
            _nearbyLevel = collision.gameObject.GetComponent<LevelSelector>();
            _nearbyLevel.ShowText();
        }

        if (collision.gameObject.CompareTag("AI"))
        {
            _nearbyAI = collision.gameObject.GetComponent<AIController>();
            _nearbyAI.ShowText();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LevelSelect") && _nearbyLevel != null
            && collision.gameObject == _nearbyLevel.gameObject)
        {
            _nearbyLevel.HideText();
            _nearbyLevel = null;
        }

        if (collision.gameObject.CompareTag("AI") && _nearbyAI != null
            && collision.gameObject == _nearbyAI.gameObject)
        {
            _nearbyAI.HideText();
            _nearbyAI = null;
        }
    }
}