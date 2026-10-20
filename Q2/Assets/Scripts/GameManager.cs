using TMPro;
using UnityEngine;

// Controls the Roll-a-Ball game. The level objects are saved directly in SampleScene.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene References")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject powerUpCapsule;
    [SerializeField] private GameObject specialWall;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI winText;

    [Header("Gameplay")]
    [SerializeField] private int totalCubes = 10;
    [SerializeField] private float powerUpSpeedMultiplier = 1.9f;
    [SerializeField] private Vector3 startingPosition = new Vector3(-1f, 1.2f, -7f);

    private int cubesCollected;
    private bool gameStarted;
    private bool poweredUp;
    private bool wallBroken;
    private bool gameWon;
    private Renderer playerRenderer;

    public bool HasStarted => gameStarted;
    public bool HasPowerUp => poweredUp;
    public bool HasWon => gameWon;

    private void Awake()
    {
        Instance = this;
        if (playerRigidbody == null || playerControl == null || startButton == null ||
            powerUpCapsule == null || specialWall == null || counterText == null ||
            statusText == null || winText == null)
        {
            Debug.LogError("GameManager: Assign the scene references in the Inspector.");
            enabled = false;
            return;
        }

        playerRenderer = playerRigidbody.GetComponent<Renderer>();

        // The ball stays frozen until Start is clicked.
        playerRigidbody.useGravity = false;
        playerRigidbody.isKinematic = true;
        playerRigidbody.position = startingPosition;
        playerControl.enabled = false;

        powerUpCapsule.SetActive(false);
        specialWall.SetActive(true);
        startButton.SetActive(true);
        winText.gameObject.SetActive(false);
        statusText.text = "";
        UpdateCounter();
    }

    public void StartGame()
    {
        if (!enabled || gameStarted || gameWon) return;

        gameStarted = true;
        playerRigidbody.isKinematic = false;
        playerRigidbody.useGravity = true;
        playerControl.enabled = true;
        startButton.SetActive(false);
    }

    public void CollectCube()
    {
        if (!gameStarted || gameWon) return;

        cubesCollected++;
        UpdateCounter();
        if (cubesCollected == totalCubes)
        {
            powerUpCapsule.SetActive(true);
            statusText.text = "Power-up ready!";
        }
    }

    private void UpdateCounter()
    {
        counterText.text = "Cubes: " + cubesCollected + " / " + totalCubes;
    }

    public void CollectPowerUp()
    {
        if (!gameStarted || gameWon || poweredUp || cubesCollected != totalCubes) return;

        poweredUp = true;
        powerUpCapsule.SetActive(false);
        playerControl.SetSpeedMultiplier(powerUpSpeedMultiplier);

        if (playerRenderer != null)
        {
            Material newMaterial = new Material(playerRenderer.material);
            Color pink = new Color(1f, 0.58f, 0.77f);
            if (newMaterial.HasProperty("_BaseColor")) newMaterial.SetColor("_BaseColor", pink);
            if (newMaterial.HasProperty("_Color")) newMaterial.SetColor("_Color", pink);
            playerRenderer.material = newMaterial;
        }
        statusText.text = "Powered Up!";
    }

    public void BreakSpecialWall()
    {
        if (!gameStarted || gameWon || !poweredUp || wallBroken) return;
        wallBroken = true;
        specialWall.SetActive(false);
    }

    public void ReachGoal()
    {
        if (!gameStarted || gameWon || !wallBroken) return;

        gameWon = true;
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        playerControl.enabled = false;
        playerRigidbody.gameObject.SetActive(false);
        winText.gameObject.SetActive(true);
    }

    // Falling into the gap always returns the ball to the original starting position.
    public void RespawnBall()
    {
        if (!gameStarted || gameWon) return;

        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        playerRigidbody.position = startingPosition;
        playerRigidbody.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (gameStarted && !gameWon && playerRigidbody.position.y < -8f)
            RespawnBall();
    }
}
