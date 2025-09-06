using UnityEngine;
using TMPro;
using System.Text;

public class PasswordBar : MonoBehaviour
{
    public TextMeshProUGUI livePasswordText;
    public Transform slotsParent;

    public string CurrentPassword { get; private set; } = "";

    public void UpdatePassword()
    {
        StringBuilder sb = new StringBuilder();

        foreach (Transform slot in slotsParent)
        {
            PasswordSlot ps = slot.GetComponent<PasswordSlot>();
            if (ps != null && ps.CurrentTile != null) // ✅ use getter
            {
                sb.Append(ps.CurrentTile.tileValue);
            }
        }

        CurrentPassword = sb.ToString();
        livePasswordText.text = $"Your Password: {CurrentPassword}";
    }

    public void ClearPassword()
    {
        CurrentPassword = "";
        //passwordDisplay.text = ""; // assuming you’re using a TMP text to show input
    }

}
