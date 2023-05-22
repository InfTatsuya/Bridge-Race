using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunnedState : State
{
    private Enemy enemy;

    private float stunnedTimer;

    public EnemyStunnedState(Character character, Animator anim, int animString, Enemy enemy) : base(character, anim, animString)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        foreach(BrickObject brick in enemy.collectedBricks)
        {
            Vector3 randomPos = Random.insideUnitSphere * 3f;
            randomPos.y = 0;

            brick.transform.position = enemy.transform.position + randomPos;
            brick.transform.SetParent(null);
            brick.State = BrickState.Dropped;
        }
        
        enemy.collectedBricks.Clear();  
        stunnedTimer = enemy.StunnedTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Tick()
    {
        base.Tick();

        stunnedTimer -= Time.deltaTime;

        if( stunnedTimer < 0f)
        {
            enemy.CharacterStateMachine.ChangeState(enemy.CollectState);
        }
    }
}
