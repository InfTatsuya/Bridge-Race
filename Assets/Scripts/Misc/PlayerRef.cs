using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PlayerRef 
{
    public static event EventHandler onPlayerNameChanged;

    private static string playerName;
    private static int money;

    public static string PlayerName
    {
        get
        {
            if (string.IsNullOrEmpty(playerName))
            {
                playerName = PlayerPrefs.GetString(StringCollection.PLAYER_NAME_KEY, "Player");
            }
            return playerName;
        }
    }

    public static int Money
    {
        get => money = PlayerPrefs.GetInt(StringCollection.PLAYER_MONEY_KEY, 0);
    }

    public static void SetPlayerName(string _playerName)
    {
        playerName = _playerName;
        PlayerPrefs.SetString(StringCollection.PLAYER_NAME_KEY, playerName);

        onPlayerNameChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void SetPlayerMoney(int value)
    {
        money = value;
        PlayerPrefs.SetInt(StringCollection.PLAYER_NAME_KEY, money);
    }
}
