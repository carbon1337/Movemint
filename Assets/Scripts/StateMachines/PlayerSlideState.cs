using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("In the Slide state");

        player.animator.SetBool("isJumping", false);
        player.animator.SetBool("isRunning", false);
        player.animator.SetBool("isOnWall", false);
        player.animator.SetBool("isSliding", true);

        AudioManager.instance.PlaySFX("PlayerSlide", true, 1);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.rb.velocity.x < 0  && player.PC.canSlide)
        {
            player.SwitchState(player.LeftState);
        }

        if (player.rb.velocity.x > 0 && player.PC.canSlide)
        {
            player.SwitchState(player.RightState);
        }

        if (player.rb.velocity.y > 0 && !player.IsGrounded())
        {
            player.SwitchState(player.UpState);
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