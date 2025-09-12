using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animation Components")]
    private Animator _animator;

    [Header("Animation Triggers")]
    private const string jumpTrigger = "jump";
    private const string slideTrigger = "slide";

    private PlayerController _playerController;

    private bool _wasJumping = false;
    private bool _wasSliding = false;

    private void Awake()
    {
        if (_animator == null) _animator = GetComponent<Animator>();

        _playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        CheckJumpAnimation();
        CheckSlideAnimation();
    }

    private void CheckJumpAnimation()
    {
        if (_playerController.IsJumping && !_wasJumping) _animator.SetTrigger(jumpTrigger);

        _wasJumping = _playerController.IsJumping;
    }

    private void CheckSlideAnimation()
    {
        if (_playerController.IsSliding && !_wasSliding) _animator.SetTrigger(slideTrigger);

        _wasSliding = _playerController.IsSliding;
    }
}
