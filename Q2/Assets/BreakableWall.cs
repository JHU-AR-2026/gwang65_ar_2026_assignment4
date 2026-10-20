using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance != null)
            GameManager.Instance.BreakSpecialWall();
    }
}
