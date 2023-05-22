using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBridge : MonoBehaviour
{
    public static event EventHandler<OnAnyCharacterPassArgs> onAnyCharacterPass;

    public class OnAnyCharacterPassArgs : EventArgs
    {
        public BrickType characterBrickType;
        public int stageIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Character>(out var character))
        {
            character.OnNewStage();


            onAnyCharacterPass?.Invoke(this, new OnAnyCharacterPassArgs()
            {
                characterBrickType = character.CharacterBrickType,
                stageIndex = character.StageIndex
            });
        }
    }
}
