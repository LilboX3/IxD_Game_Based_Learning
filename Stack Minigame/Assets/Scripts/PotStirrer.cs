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

    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = leftEnd.position;
        targetPosition = rightEnd.position;
        StartCoroutine(WaitAndFlipEverySeconds(1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingStirred)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    private IEnumerator WaitAndFlipEverySeconds(float seconds)
    {
        while (isBeingStirred)
        {
            targetPosition = targetPosition == leftEnd.position 
                ? rightEnd.position 
                : leftEnd.position;
            yield return new WaitForSeconds(seconds);
        }
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
