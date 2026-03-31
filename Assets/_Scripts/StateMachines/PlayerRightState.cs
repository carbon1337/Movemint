using UnityEngine;

public class PlayerRightState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("In the right state");

        player.animator.SetBool("isJumping", false);
        player.animator.SetBool("isSliding", false);
        player.animator.SetBool("isOnWall", false);
        player.animator.SetBool("isRunning", true);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.rb.velocity.y == 0 && player.rb.velocity.x == 0 && player.IsGrounded())
        {
            player.SwitchState(player.IdleState);
        }

        if (Input.GetKey(KeyCode.A) && player.rb.velocity.y != 0)
        {
            player.SwitchState(player.LeftState);
        }

        else if (player.rb.velocity.y != 0)
        {
            player.SwitchState(player.UpState);
        }
        
        if (!player.PC.canSlide) 
        {
            player.SwitchState(player.SlideState);
        }

        if (player.hasAttacked)
        {
            player.animator.SetTrigger("Punch1");
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
