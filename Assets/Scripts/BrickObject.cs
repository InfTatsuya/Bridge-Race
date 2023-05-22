using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickObject : MonoBehaviour 
{
    [SerializeField] BrickType brickType = BrickType.RedBrick;
    private BrickType defaultType;

    [SerializeField] ColorData colorData;
    //[SerializeField] Material[] materialArray;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float animationSpeed = 10f;
    [SerializeField] TrailRenderer trailRenderer;

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
        meshRenderer.material = colorData.GetMaterial(BrickType.UndefinedBrick);
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

        meshRenderer.material = colorData.GetMaterial(brickType);
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

        trailRenderer.enabled = false;
    }

    public void SetupBrick(BrickType brickType)
    {
        this.brickType = brickType;

        SetupBrickColor();
    }

    public void SetBrickLocalPosition(Vector3 localPos)
    {
        StartCoroutine(MovingRoutine(localPos));
    }

    private IEnumerator MovingRoutine(Vector3 target)
    {
        trailRenderer.enabled = true;

        float deltaX = 2f;
        float deltaY = target.y;
        float t = 0;
        while(t < 1f)
        {
            float value = animationCurve.Evaluate(t);
            Vector3 pos = new Vector3(deltaX * value, deltaY * t, 0f);
            transform.localPosition = pos;

            yield return new WaitForSeconds(0.1f);

            t += Time.deltaTime * animationSpeed;
        }

        transform.localPosition = target;
        trailRenderer.enabled = false;
    }
}

public enum BrickState
{
    OnGround,
    IsCollected,
    Dropped
}
