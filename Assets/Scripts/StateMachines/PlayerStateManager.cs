using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState currentState;
    public PlayerController PC;
    public GameManager GM;

    public PlayerLeftState LeftState = new PlayerLeftState();
    public PlayerRightState RightState = new PlayerRightState();
    public PlayerUpState UpState = new PlayerUpState();
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerSlideState SlideState = new PlayerSlideState();
    public PlayerWallState WallState = new PlayerWallState();
    public PlayerDeadState DeadState = new PlayerDeadState();

    public Rigidbody2D rb;
    public Animator animator;

    public SpriteRenderer sprite;
    public bool hasAttacked;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        //starting state for machine
        currentState = IdleState;
        //"this" is a reference to the context (this EXACT Monobehavior script)
        currentState.EnterState(this);

        rb = GetComponent<Rigidbody2D>();
        PC = gameObject.GetComponent<PlayerController>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //will call any logic in Update State from the current state every frame
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
}
