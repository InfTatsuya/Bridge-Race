using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBuildState : State
{
    private Enemy enemy;
    private NavMeshAgent agent;

    public EnemyBuildState(Character character, Animator anim, int animString, Enemy enemy) : base(character, anim, animString)
    {
        this.enemy = enemy;
        this.agent = enemy.Agent;
    }

    public override void Enter()
    {
        base.Enter();

        Vector3 target = enemy.GetBridgeStartPosition();

        agent.SetDestination(target);
    }

    public override void Exit()
    {
        base.Exit();
        agent.ResetPath();
    }

    public override void Tick()
    {
        base.Tick();

        if(enemy.collectedBricks.Count <= 0)
        {
            enemy.CharacterStateMachine.ChangeState(enemy.IdleState);
        }
    }
}
