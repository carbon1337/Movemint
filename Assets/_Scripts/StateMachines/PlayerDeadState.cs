using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("In the Dead state");

        player.animator.SetBool("isJumping", false);
        player.animator.SetBool("isRunning", false);
        player.animator.SetBool("isOnWall", false);
        player.animator.SetBool("isSliding", false);
        player.animator.SetBool("isDead", true);

        AudioManager.instance.PlaySFX("PlayerDeath", true, 1);
    }

    public override void UpdateState(PlayerStateManager player)
    {

    }

    public override void OnCollisionEnter(PlayerStateManager player, Collision collision)
    {

    }
}