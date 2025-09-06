using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton

    [Header("Game Rules")]
    public int requiredPasswords = 3;
    public float totalTime = 300f; // 5 minutes = 300 seconds

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public GameObject  WinPannel;
   

    private int submittedPasswords = 0;
    private float timeRemaining;
    private bool gameOver = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        timeRemaining = totalTime;
        UpdateTimerUI();
        WinPannel.gameObject.SetActive(false); // hide at start
    }

    private void Update()
    {
        if (gameOver) return;

        // Countdown
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame(false);
        }
        UpdateTimerUI();
    }

    public void PasswordSubmitted()
    {
        if (gameOver) return;

        submittedPasswords++;

        if (submittedPasswords >= requiredPasswords)
        {
            EndGame(true);
        }
    }

    private void EndGame(bool success)
    {
        gameOver = true;

        if (success)
        {
           //open won pannel
            // resultText.text = "🎉 Well done, Inspector! You built all passwords!";
        }
        else
        {
           //loss pannel
            // resultText.text = "⏳ Time's up! Case failed!";
        }

       WinPannel.gameObject.SetActive(true);
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
