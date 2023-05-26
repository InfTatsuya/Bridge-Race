using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] JoyStick moveStick;

    private Vector2 moveDirection;
    public Vector2 MoveDirection { get => moveDirection; }
    public bool CanMoveForward { get; set; }
    public bool CanMoveBackward { get; set; }
    public bool CanMoveLeft { get; set; }
    public bool CanMoveRight { get; set; }

    private bool isDance;
    public bool IsDance { get => isDance; set => isDance = value; }

    private float yPos;

    #region State

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerStunnedState StunnedState { get; private set; }
    public PlayerBuildState BuildState { get; private set; }
    public PlayerWinState WinState { get; private set; }

    #endregion

    protected override void Start()
    {
        base.Start();
        CanMoveForward = true;
        CanMoveBackward = true;
        CanMoveLeft = true;
        CanMoveRight = true;
    }

    protected override void Update()
    {
        base.Update();

        if (isDance) return;

        if (isStunned) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f && Mathf.Abs(moveDirection.y) > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 30f);
        }

        ConstrainPlayerMoveArea();
        float xPos = transform.position.x + moveDirection.x * moveSpeed * Time.deltaTime;
        float zPos = transform.position.z + moveDirection.y * moveSpeed * Time.deltaTime;

        Vector3 newPosition = new Vector3(xPos, yPos, zPos);
        transform.position = newPosition;
    }

    private void ConstrainPlayerMoveArea()
    {
        if (!CanMoveForward)
        {
            float yDir = Mathf.Clamp(moveDirection.y, -1, 0);
            moveDirection = new Vector2(moveDirection.x, yDir);
        }

        if (!CanMoveBackward)
        {
            float yDir = Mathf.Clamp(moveDirection.y, 0, 1);
            moveDirection = new Vector2(moveDirection.x, yDir);
        }

        if (!CanMoveRight)
        {
            float xDir = Mathf.Clamp(moveDirection.x, -1, 0);
            moveDirection = new Vector2(xDir, moveDirection.y);
        }

        if (!CanMoveLeft)
        {
            float xDir = Mathf.Clamp(moveDirection.x, 0, 1);
            moveDirection = new Vector2(xDir, moveDirection.y);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();

        moveStick.onStickInputValueUpdated += MoveStick_onStickInputValueUpdated;

        IdleState = new PlayerIdleState(this, anim, StringCollection.idleString, this);
        MoveState = new PlayerMoveState(this, anim, StringCollection.moveString, this);
        WinState = new PlayerWinState(this, anim, StringCollection.danceString, this);

        stateMachine.Initialize(IdleState);
    }

    private void MoveStick_onStickInputValueUpdated(object sender, JoyStick.OnStickInputValueUpdatedArg e)
    {
        moveDirection = e.inputVector;
    }

    protected override void AddBrick(BrickObject brick)
    {
        base.AddBrick(brick);
    }

    protected override void RemoveBrick(BrickObject brick)
    {
        base.RemoveBrick(brick);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("UndefinedBrick"))
        {
            BrickObject brick = other.GetComponent<BrickObject>();
            brick.SetupBrick(brickType);
            AddBrick(brick);
        }


    }

    protected override void NextLevel()
    {
        base.NextLevel();

        transform.position = startPosition;
        moveDirection = Vector2.zero;
        stateMachine.ChangeState(IdleState);
    }

    public void SetPlayerHeight(float y)
    {
        yPos = y;
    }

    public override void Dance()
    {
        
        stateMachine.ChangeState(WinState);
    }
}
