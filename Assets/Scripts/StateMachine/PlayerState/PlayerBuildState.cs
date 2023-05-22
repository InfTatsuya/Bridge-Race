using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildState : State
{
    private Player player;

    public PlayerBuildState(Character character, Animator anim, int animString, Player player) : base(character, anim, animString)
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
    }
}
