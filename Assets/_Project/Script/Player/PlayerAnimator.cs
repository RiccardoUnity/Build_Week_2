using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animation Components")]
    public Animator animator;

    [Header("Animation Triggers")]
    public string jumpTrigger = "jump";
    public string slideTrigger = "slide";
    public string hitTrigger = "isFalling";

    private PlayerController _playerController;

    private bool _wasJumping = false;
    private bool _wasSliding = false;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();

        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        CheckJumpAnimation();
        CheckSlideAnimation();
    }

    private void CheckJumpAnimation()
    {
        if (_playerController.IsJumping && !_wasJumping) animator.SetTrigger(jumpTrigger);

        _wasJumping = _playerController.IsJumping;
    }

    private void CheckSlideAnimation()
    {
        if (_playerController.IsSliding && !_wasSliding) animator.SetTrigger(slideTrigger);

        _wasSliding = _playerController.IsSliding;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles")) animator.SetTrigger(hitTrigger);
    }
}
