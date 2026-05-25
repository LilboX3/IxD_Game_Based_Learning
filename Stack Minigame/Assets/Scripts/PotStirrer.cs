using System.Collections;
using UnityEngine;

public class PotStirrer : MonoBehaviour
{
    [SerializeField]
    private bool isBeingStirred = false;
    [SerializeField]
    private Transform leftEnd;
    [SerializeField]
    private Transform rightEnd;
    [SerializeField]
    private float speed = 4.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = leftEnd.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingStirred)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightEnd.position, speed * Time.deltaTime);
            StartCoroutine(WaitSeconds(1f));
            transform.position = Vector3.MoveTowards(transform.position, leftEnd.position, speed * Time.deltaTime);
            StartCoroutine(WaitSeconds(1f));
        }
    }

    private IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void StartStirring()
    {
        isBeingStirred = true;
    }

    public void StopStirring()
    {
        isBeingStirred = false;
    }
}
