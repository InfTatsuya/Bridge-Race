using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCollectState : State
{
    private Enemy enemy;
    private NavMeshAgent agent;

    public EnemyCollectState(Character character, Animator anim, int animString, Enemy enemy) : base(character, anim, animString)
    {
        this.enemy = enemy;
        agent = enemy.Agent;
    }

    public override void Enter()
    {
        base.Enter();

        //if (agent == null)
        //{
        //    Debug.Log("Change State vi NavMesh");
        //    enemy.CharacterStateMachine.ChangeState(enemy.IdleState);
        //}

        Vector3 target = enemy.GetClosestBrick();

        if(target == Vector3.zero)
        {
            Debug.Log("ChangeTo BuildState");
            enemy.CharacterStateMachine.ChangeState(enemy.BuildState);
            return;
        }

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
    }
}
