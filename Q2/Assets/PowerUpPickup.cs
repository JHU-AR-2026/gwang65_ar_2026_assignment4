using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    private Vector3 basePosition;

    private void OnEnable()
    {
        basePosition = transform.position;
    }

    private void Update()
    {
        // Tilted spin makes the capsule's rotation easy to see.
        transform.Rotate(new Vector3(0.6f, 1f, 0.25f).normalized,
            110f * Time.deltaTime, Space.World);
        transform.position = basePosition + Vector3.up * (0.12f * Mathf.Sin(Time.time * 2.4f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance != null)
            GameManager.Instance.CollectPowerUp();
    }
}
