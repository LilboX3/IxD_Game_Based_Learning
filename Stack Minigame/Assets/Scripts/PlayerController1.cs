using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;

    private float moveInputHorizontal;
    private float moveInputVertical;

    private LevelSelector _nearbyLevel;
    private AIController _nearbyAI;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInputHorizontal = Input.GetAxisRaw("Horizontal");
        moveInputVertical = Input.GetAxisRaw("Vertical");

        // for Hub Level Select
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_nearbyLevel != null) _nearbyLevel.LoadLevel();
            else if (_nearbyAI != null)
            {
                if(!_nearbyAI.isAiOpen) _nearbyAI.OpenAIWindow();
                else _nearbyAI.CloseAIWindow();
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInputHorizontal * moveSpeed, moveInputVertical * moveSpeed);
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