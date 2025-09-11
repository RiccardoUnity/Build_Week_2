using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_animator != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _animator.SetTrigger("jump");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _animator.SetTrigger("slide");
            }
        }
    }
}
