using System;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public static event EventHandler<OnCharacterWinArgs> onCharacterWin;
    public class OnCharacterWinArgs : EventArgs
    {
        public Character character;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Character>(out var character))
        {
            onCharacterWin?.Invoke(this, new OnCharacterWinArgs { character = character });
            character.Dance();
        }
    }
}
