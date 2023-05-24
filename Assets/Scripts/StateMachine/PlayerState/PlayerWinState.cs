using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinState : State
{
    private Player player;

    public PlayerWinState(Character character, Animator anim, int animString, Player player) : base(character, anim, animString)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();

        player.IsDance = true;
    }

    public override void Exit()
    {
        base.Exit();

        player.IsDance = false;
    }

    public override void Tick()
    {
        base.Tick();
    }
}
