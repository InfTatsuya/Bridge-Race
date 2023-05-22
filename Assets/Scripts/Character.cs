using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;

    [SerializeField] protected Animator anim;
    protected Rigidbody rb;

    protected StateMachine stateMachine;
    public StateMachine CharacterStateMachine { get => stateMachine; }

    protected bool isStunned;

    [SerializeField] protected Transform brickParent;
    [SerializeField] protected float offset = 0.3f;
    [SerializeField] protected BrickType brickType;
    [SerializeField] protected ColorData colorData;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    public BrickType CharacterBrickType { get => brickType; }

    [SerializeField] protected string brickTag;

    [SerializeField] int stageIndex;
    public int StageIndex { get => stageIndex; set => stageIndex = value; }

    public List<BrickObject> collectedBricks = new List<BrickObject>();
    protected virtual void Start()
    {
        OnInit();
    }

    protected virtual void Update()
    {
        if(stateMachine.CurrentState != null) 
        {
            stateMachine.CurrentState.Tick();
        }

        if (isStunned) return;
    }

    protected virtual void OnInit()
    {
        rb = GetComponent<Rigidbody>();
        ClearBricks();
        stateMachine = new StateMachine();
        brickTag = GetBrickTagByColor();
        skinnedMeshRenderer.material = colorData.GetMaterial(brickType);

        stageIndex = 1;
    }

    protected virtual void AddBrick(BrickObject brick)
    {
        collectedBricks.Add(brick);
        brick.transform.SetParent(brickParent, false);
        brick.SetBrickLocalPosition(Vector3.up * offset * collectedBricks.Count);
        brick.transform.localRotation = Quaternion.identity;
    }

    protected virtual void RemoveBrick(BrickObject brick)
    {
        brick.Release();
        collectedBricks.Remove(brick);
    }

    protected void ClearBricks()
    {
        collectedBricks.Clear();    
    }

    public void SetIsStunned(bool isStunned)
    {
        this.isStunned = isStunned;
    }

    public bool CanBuildBridge() => collectedBricks.Count > 0;

    public void BuildBridge()
    {
        BrickObject brick = collectedBricks[collectedBricks.Count - 1];
        RemoveBrick(brick);
    }

    private string GetBrickTagByColor()
    {
        return brickType.ToString();
    }

    public virtual void OnNewStage()
    {
        stageIndex++;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(brickTag))
        {
            BrickObject brick = other.GetComponent<BrickObject>();
            AddBrick(brick);
        }
    }


}
