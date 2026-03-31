using UnityEngine;

public class PlayerUpState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("In the up state");

        player.animator.SetBool("isJumping", true);
        player.animator.SetBool("isOnWall", false);
        player.animator.SetFloat("yVelocity", 1f);
        AudioManager.instance.PlaySFX("PlayerJump", false, 1);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        //conditions for right state
        if (player.rb.velocity.x >= 0.01 && player.IsGrounded())
        {
            AudioManager.instance.PlaySFX("PlayerLand", false, 1);
            player.SwitchState(player.RightState);
        }

        //conditions for left state
        if (player.rb.velocity.x <= 0 && player.IsGrounded())
        {
            AudioManager.instance.PlaySFX("PlayerLand", false, 1);
            player.SwitchState(player.LeftState);
        }

        //conditions for idle state
        if (player.rb.velocity.y == 0 && player.rb.velocity.x == 0 && player.IsGrounded())
        {
            AudioManager.instance.PlaySFX("PlayerLand", false, 1);
            player.SwitchState(player.IdleState);
        }

        if(player.PC.IsOnWall()) 
        {
            AudioManager.instance.PlaySFX("PlayerLand", false, 1);
            player.SwitchState(player.WallState);
        }

        if (player.rb.velocity.y <= 0)
        {
            player.animator.SetFloat("yVelocity", -1f);
        }

        if (player.rb.velocity.y >= 0.00000001)
        {
            player.animator.SetFloat("yVelocity", 1f);
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
