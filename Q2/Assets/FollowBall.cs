using UnityEngine;

public class FollowBall : MonoBehaviour
{
    public Transform ballTransform; // reference to the ball's transform
    private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ballTransform != null)
            offset = transform.position - ballTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ballTransform != null)
            transform.position = ballTransform.position + offset;
    }
}
