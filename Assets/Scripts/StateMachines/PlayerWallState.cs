using UnityEngine;

public class PlayerWallState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("In the wall state");

        player.animator.SetBool("isOnWall", true);
        AudioManager.instance.PlaySFX("PlayerSlide", false, 1.5f);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        //conditions for right state
        if (player.rb.velocity.x >= 0.01 && player.IsGrounded())
        {
            player.SwitchState(player.RightState);
        }

        //conditions for left state
        if (player.rb.velocity.x <= 0 && player.IsGrounded())
        {
            player.SwitchState(player.LeftState);
        }

        //conditions for idle state
        if (player.rb.velocity.y == 0 && player.rb.velocity.x == 0 && player.IsGrounded())
        {
            player.SwitchState(player.IdleState);
        }

        if (player.rb.velocity.y != 0 && !player.PC.IsOnWall())
        {
            player.SwitchState(player.UpState);
        }

        if( player.GM.levelFailed) 
        {
            player.SwitchState(player.DeadState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}
