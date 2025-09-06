
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordValidator : MonoBehaviour
{
    [Header("References")]
    public PasswordBar bar;

    [Header("Checklist Icons")]
    public Image lengthIcon;
    public Image caseIcon;
    public Image numberIcon;
    public Image symbolIcon;
    public Image commonIcon;

    [Header("Sprites")]
    public Sprite emptySprite;
    public Sprite tickSprite;
    public Sprite crossSprite;

    [Header("Submit")]
    public Button submitButton;   
    public TextMeshProUGUI builtText;   

    private int submittedCount = 0;

    private bool allValid = false;

    // Weak/common passwords
    private string[] weakPasswords = { "password", "123456", "qwerty", "letmein", "dog123" };

    private void Start()
    {
        // default: disabled
        //submitButton.interactable = false;
    }

    private void Update()
    {
        Validate(bar.CurrentPassword);
    }

    private void Validate(string password)
    {
        // Rule 1: Length
        bool lengthOk = password.Length >= 12;
        UpdateCheck(lengthIcon, lengthOk);

        // Rule 2: Upper + lower
        bool hasUpper = Regex.IsMatch(password, "[A-Z]");
        bool hasLower = Regex.IsMatch(password, "[a-z]");
        bool caseOk = hasUpper && hasLower;
        UpdateCheck(caseIcon, caseOk);

        // Rule 3: Numbers
        bool hasNumber = Regex.IsMatch(password, "[0-9]");
        UpdateCheck(numberIcon, hasNumber);

        // Rule 4: Symbols
        bool hasSymbol = Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]");
        UpdateCheck(symbolIcon, hasSymbol);

        // Rule 5: Not too common
        bool notCommon = true;
        foreach (string weak in weakPasswords)
        {
            if (password.ToLower().Contains(weak))
            {
                notCommon = false;
                break;
            }
        }
        UpdateCheck(commonIcon, notCommon);

        // ✅ Enable button only if ALL conditions pass
        allValid = lengthOk && caseOk && hasNumber && hasSymbol && notCommon;
      
    }

    private void UpdateCheck(Image icon, bool passed)
    {
        if (string.IsNullOrEmpty(bar.CurrentPassword))
        {
            icon.sprite = crossSprite;  // neutral at start
        }
        else
        {
            icon.sprite = passed ? tickSprite : crossSprite;
        }
    }


    // Call this from the Submit Button’s OnClick()
    public void SubmitPassword()
    {
        UnityEngine.Debug.Log($"Password submitted: {bar.CurrentPassword}");
        if (allValid)
        {

            submittedCount++;
            UpdateBuiltText();

            // ✅ Tell the GameManager
            GameManager.Instance.PasswordSubmitted();
            // ✅ Reset builder for next password
            ResetPasswordBuilder();
            AudioManager.Instance.PlaySound(AudioManager.Instance.positiveFeedback);
        };
    }

    private void UpdateBuiltText()
    {
        
           builtText.text = $"Passwords Built: {submittedCount}";
    }
    private void ResetPasswordBuilder()
    {
        // Clear current password in bar
        bar.ClearPassword();

        // Reset icons to empty
        lengthIcon.sprite = emptySprite;
        caseIcon.sprite = emptySprite;
        numberIcon.sprite = emptySprite;
        symbolIcon.sprite = emptySprite;
        commonIcon.sprite = emptySprite;

        // Reset all draggable tiles
        foreach (var tile in FindObjectsOfType<DraggableTile>())
        {
            tile.ResetToDefault();
        }

        UnityEngine.Debug.Log("Password builder reset. Ready for next password!");
    }


}
