using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickObject : MonoBehaviour 
{
    [SerializeField] BrickType brickType = BrickType.RedBrick;
    private BrickType defaultType;

    [SerializeField] Material[] materialArray;

    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;

    private BrickState state;
    public BrickState State
    {
        get => state;
        set
        {
            state = value;

            switch(state)
            {
                case BrickState.OnGround:
                    break;

                case BrickState.IsCollected:
                    boxCollider.enabled = false;
                    break;

                case BrickState.Dropped:
                    StartCoroutine(SetupDroppedBrick());
                    break;

                default:
                    break;
            }
        }
    }

    private IEnumerator SetupDroppedBrick()
    {
        meshRenderer.material = materialArray[4];
        gameObject.tag = "UndefinedBrick";

        yield return new WaitForSeconds(0.1f);

        boxCollider.enabled = true;
    }

    private void OnValidate()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        SetupBrickColor();
    }

    private void SetupBrickColor()
    {
        gameObject.tag = brickType.ToString();

        switch (brickType)
        {
            case BrickType.RedBrick:
                meshRenderer.material = materialArray[0];
                break;

            case BrickType.GreenBrick:
                meshRenderer.material = materialArray[1];
                break;

            case BrickType.BlueBrick:
                meshRenderer.material = materialArray[2];
                break;

            case BrickType.YellowBrick:
                meshRenderer.material = materialArray[3];
                break;

            default:
                break;

        }
    }

    private ObjectPool<BrickObject> pool;
    public ObjectPool<BrickObject> Pool { get => pool; set => pool = value; }
    public void Release()
    {
        SetupBrick(defaultType);
        pool.ReturnToPool(this);
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        State = BrickState.OnGround;
        defaultType = brickType;
    }

    public void SetupBrick(BrickType brickType)
    {
        this.brickType = brickType;

        SetupBrickColor();
    }
}

public enum BrickType
{
    RedBrick,
    GreenBrick,
    BlueBrick,
    YellowBrick,
    UndefinedBrick,
    NoColor
}

public enum BrickState
{
    OnGround,
    IsCollected,
    Dropped
}
