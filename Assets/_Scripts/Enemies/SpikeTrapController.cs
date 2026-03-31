using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    private Animator animator;

    public float trapCooldownTimer;
    private bool canTriggerTrap = true;

    void Awake() {
        animator = gameObject.GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            HandleTrapTrigger();
        }
    }

    private void HandleTrapTrigger() {
        Debug.Log("rahh");
        if(canTriggerTrap) {
            animator.SetTrigger("TriggeredTrap");
            canTriggerTrap = false;
            StartCoroutine("TrapCooldown");
        }

    }

    private IEnumerator TrapCooldown() {
        yield return new WaitForSeconds(trapCooldownTimer);
        canTriggerTrap = true;
    }
}
