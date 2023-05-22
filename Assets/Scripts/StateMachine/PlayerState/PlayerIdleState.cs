using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    private Player player;

    public PlayerIdleState(Character character, Animator anim, int animString, Player player) : base(character, anim, animString)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        if(player.MoveDirection != Vector2.zero)
        {
            player.CharacterStateMachine.ChangeState(player.MoveState);
        }
    }
}
