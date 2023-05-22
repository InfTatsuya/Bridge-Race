using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{
    private Enemy enemy;

    private float idleTimer;

    public EnemyIdleState(Character character, Animator anim, int animString, Enemy enemy) : base(character, anim, animString)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        idleTimer = enemy.IdleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        idleTimer -= Time.deltaTime;

        if(idleTimer < 0f)
        {
            if(enemy.collectedBricks.Count >= 15)
            {
                enemy.CharacterStateMachine.ChangeState(enemy.BuildState);
            }
            else
            {
                enemy.CharacterStateMachine.ChangeState(enemy.CollectState);
            }

        }

    }
}
