using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringCollection 
{
    public static int idleString = Animator.StringToHash("Idle");
    public static int moveString = Animator.StringToHash("Move");
    public static int stunnedString = Animator.StringToHash("Stunned");
    public static int danceString = Animator.StringToHash("Dance");

    public const string PLAYER_NAME_KEY = "PlayerName";
    public const string PLAYER_MONEY_KEY = "PlayerMoney";

    public const string masterVolumeString = "masterVolume";
}
