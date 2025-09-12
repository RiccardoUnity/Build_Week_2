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
            if (Input.GetKeyDown(KeyCode.W)) _animator.SetTrigger("jump");
            }
            if (Input.GetKeyDown(KeyCode.S)) _animator.SetTrigger("slide");
        }
    }

    //private void OnCollision(Collider other)
    //{
    //    Debug.Log(other);
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
    //    {
    //        _animator.SetTrigger("falling");
    //    }
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision);
    //}
