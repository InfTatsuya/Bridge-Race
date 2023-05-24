using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;
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
    public EnemyWinState WinState { get; private set; }
    #endregion

    protected override void OnInit()
    {
        base.OnInit();

        agent = GetComponent<NavMeshAgent>();

        IdleState = new EnemyIdleState(this, anim, StringCollection.idleString, this);
        CollectState = new EnemyCollectState(this, anim, StringCollection.moveString, this);
        StunnedState = new EnemyStunnedState(this, anim, StringCollection.stunnedString, this);
        BuildState = new EnemyBuildState(this, anim, StringCollection.moveString, this);
        WinState = new EnemyWinState(this, anim, StringCollection.danceString);

        StartCoroutine(GetBrickList(brickType));

        UIManager.onPlayGame += UIManager_onPlayGame;
        WinTrigger.onCharacterWin += WinTrigger_onCharacterWin;
    }

    private void WinTrigger_onCharacterWin(object sender, WinTrigger.OnCharacterWinArgs e)
    {
        agent.ResetPath();
        stateMachine.ChangeState(IdleState);
        agent.enabled = false;

        IsPaused = true;
    }

    private void UIManager_onPlayGame(object sender, System.EventArgs e)
    {
        StartPlaying();
    }

    private void StartPlaying()
    {
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

        //Debug.Log(stateMachine.CurrentState);
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
            Debug.Log("Try get new brickList");
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

    protected override void NextLevel()
    {
        base.NextLevel();

        transform.position = startPosition;
        agent.enabled = true;
        bridgeIndex = -1;
        stateMachine.ChangeState(IdleState);

        GetNewBrickList();
        Debug.Log("Next Level");
        IsPaused = false;
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

        GetNewBrickList();

        CharacterStateMachine.ChangeState(IdleState);

        BridgeIndex = -1;


    }

    private void GetNewBrickList()
    {
        bricksToCollect.Clear();

        StartCoroutine(GetBrickList(brickType));
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

    public override void Dance()
    {
        stateMachine.ChangeState(WinState);
    }
}
