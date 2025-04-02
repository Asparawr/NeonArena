using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardNotification : MonoBehaviour
{
    public TextMeshProUGUI notificationText;
    public GameSetup gameSetup;
    public void SetReward(int rewardChests, int rewardMoney)
    {
        var text = "You received";
        if (rewardChests > 0)
        {
            text += "\n+" + rewardChests + " chests";
        }
        if (rewardMoney > 0)
        {
            text += "\n+" + rewardMoney + " coins";
        }
        notificationText.text = text;
    }
    public void Close()
    {
        gameSetup.LeaveGame();
        gameObject.SetActive(false);
    }
}
