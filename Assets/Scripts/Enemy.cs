using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    [SerializeField] private float idleTime = 3f;
    public float IdleTime { get => idleTime; }

    [SerializeField] private float stunnedTime = 3f;
    public float StunnedTime { get => stunnedTime; }

    [SerializeField] int bridgeIndex = -1;
    public int BridgeIndex { get => bridgeIndex; set => bridgeIndex = value; }

    [SerializeField] private List<BrickObject> bricksToCollect = new List<BrickObject>();

    #region State

    public EnemyIdleState IdleState { get; private set; }
    public EnemyCollectState CollectState { get; private set; }
    public EnemyStunnedState StunnedState { get; private set; }
    public EnemyBuildState BuildState { get; private set; }
    #endregion

    protected override void OnInit()
    {
        base.OnInit();

        agent = GetComponent<NavMeshAgent>();

        IdleState = new EnemyIdleState(this, anim, StringCollection.idleString, this);
        CollectState = new EnemyCollectState(this, anim, StringCollection.moveString, this);
        StunnedState = new EnemyStunnedState(this, anim, StringCollection.stunnedString, this);
        BuildState = new EnemyBuildState(this, anim, StringCollection.moveString, this);

        StartCoroutine(GetBrickList(brickType));

        stateMachine.Initialize(IdleState);
    }

    protected override void AddBrick(BrickObject brick)
    {
        base.AddBrick(brick);

        bricksToCollect.Remove(brick);

        //after picking up a brick, the enemy has a 40% chance to pick up another brick, otherwise he will idle
        int random = Random.Range(0, 100);
        if(random <= 70)
        {
            stateMachine.ChangeState(CollectState);
        }
        else
        {
            stateMachine.ChangeState(IdleState);
        }
    }

    protected override void RemoveBrick(BrickObject brick)
    {
        base.RemoveBrick(brick);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public Vector3 GetClosestBrick()
    {
        float minDist = Mathf.Infinity;
        GameObject nearestBrick = null;

        if(bricksToCollect.Count <= 0)
        {
            return Vector3.zero;
        }

        foreach(var brick in bricksToCollect)
        {
            float distance = Vector3.Distance(brick.transform.position, this.transform.position);
            if (distance < minDist)
            {
                minDist = distance;
                nearestBrick = brick.gameObject;
            }
        }

        return nearestBrick.transform.position;
    }

    private IEnumerator GetBrickList(BrickType brickType)
    {
        while(bricksToCollect == null || bricksToCollect.Count <= 0)
        {
            yield return new WaitForSeconds(0.5f);

            bricksToCollect = PoolManager.Instance.GetBrickList(brickType);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            if (stateMachine.CurrentState == StunnedState) return;

            stateMachine.ChangeState(StunnedState);
        }
    }

    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public override void OnNewStage()
    {
        base.OnNewStage();

        if (StageIndex >= 3)
        {
            MoveTo(LevelManager.Instance.winPos);
            return;
        }

        bricksToCollect.Clear();

        StartCoroutine(GetBrickList(brickType));

        CharacterStateMachine.ChangeState(IdleState);

        BridgeIndex = -1;

        
    }

    public Vector3 GetBridgeStartPosition()
    {
        List<Vector3> bridgeStartPos = LevelManager.Instance.GetBridgeStartPositionList(StageIndex);

        if(bridgeIndex <= -1)
        {
            bridgeIndex = Random.Range(0, bridgeStartPos.Count);
        }

        return bridgeStartPos[bridgeIndex];
    }


}
