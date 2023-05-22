using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunnedState : State
{
    public PlayerStunnedState(Character character, Animator anim, int animString) : base(character, anim, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.SetIsStunned(true);
    }

    public override void Exit()
    {
        base.Exit();
        character.SetIsStunned(false);
    }

    public override void Tick()
    {
        base.Tick();
    }
}
