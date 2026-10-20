using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || GameManager.Instance == null || !GameManager.Instance.HasStarted)
            return;

        GameManager.Instance.CollectCube();
        gameObject.SetActive(false);
    }
}
