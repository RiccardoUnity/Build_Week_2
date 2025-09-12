using SGM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float forwardSpeed = 5f;
    private Rigidbody _rb;

    public float horizontalMultiplier = 2f;
    private float _horizontalInput;

    public float onLaneDistance = 3f;
    public float laneMultiplier = 80f;
    private int _laneInput = 1; // <- 0: sinistra   1: centro   2: destra
    private Animator _animator;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    private bool _isJumping = false;

    [Header("Slide Settings")]
    public float slideDuration = 1f;
    private bool _isSliding = false;
    private float _originalColliderHeight;
    private float _colliderResize = .7f;
    private CapsuleCollider _capsule;

    [Header("Ground Check Settings")]
    public LayerMask groundLayerMask = 5;
    public float groundCheckOffset = 1.01f;
    public float groundCheckRadius = .2f;
    private bool _isGrounded;

    [Header("Debug Features")]
    public bool showGroundCheckGizmos = true;
    public bool showSlideColliderGizmos = false;

    public bool IsJumping => _isJumping;
    public bool IsSliding => _isSliding;

    public string hitTrigger = "fall";
    public string crashedTrigger = "crashed";

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _capsule = GetComponent<CapsuleCollider>();
        _originalColliderHeight = _capsule.height;

        _animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        ChangeLane();

        GroundChecker();

        if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(Slide());

        if (Input.GetKeyDown(KeyCode.W) && _isGrounded) Jump();
    }

    #region Move & Lane changing
    private void Move() // <- gestisce il movimento continuo del Player
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        Vector3 fwdMove = transform.forward * (forwardSpeed * Time.fixedDeltaTime);
        Vector3 horMove = transform.right * (_horizontalInput * horizontalMultiplier) * (forwardSpeed * Time.fixedDeltaTime);

        _rb.MovePosition(_rb.position + fwdMove + horMove);
    }

    private void ChangeLane() // <- gestisce il cambio di corsia
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _laneInput--;
            if (_laneInput == -1) _laneInput = 0;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _laneInput++;
            if (_laneInput == 3) _laneInput = 2;
        }

        Vector3 newPos = transform.position;

        if (_laneInput == 0) newPos.x = -onLaneDistance;
        else if (_laneInput == 1) newPos.x = 0f;
        else if (_laneInput == 2) newPos.x = onLaneDistance;

        transform.position = Vector3.Lerp(transform.position, newPos, laneMultiplier * Time.deltaTime);
    }
    #endregion

    #region Slide & Jump
    private IEnumerator Slide() // <- gestisce la scivolata ruotando il player
    {
        if (_isSliding) yield break; // <- per prevenire slide multipli
        if (_isJumping) yield break; // <- per prevenire slide durante il salto

        _isSliding = true;

        if (AudioManager.Instance != null) AudioManager.Instance.PlaySlideSound();

        float colliderResize = _capsule.height - _colliderResize;
        _capsule.height = colliderResize;

        yield return new WaitForSeconds(slideDuration);

        _capsule.height = _originalColliderHeight;

        _isSliding = false;
    }

    private void Jump() // <- gestisce il salto
    {
        if (_isSliding) return; // <- per non saltare durante lo slide

        _isJumping = true;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayJumpSound();

        Vector3 vel = _rb.velocity;
        vel.y = 0f;
        _rb.velocity = vel;

        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    #endregion

    #region Ground Checker
    private void GroundChecker() // <- check sphere sotto i piedi per verificare il contatto col terreno
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckOffset, 0.2f, groundLayerMask);
        if (!wasGrounded && _isGrounded && _isJumping) _isJumping = false; // <- se il player è appena atterrato, smette di considerarlo in salto
    }

    private void OnDrawGizmosSelected() // <- disegna un gizmo relativo alla check sphere e al collider del player (utile per capire come si comporta durante lo slide)
    {
        if (showGroundCheckGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckOffset, groundCheckRadius);
        }

        if (showSlideColliderGizmos && _isSliding && _capsule != null)
        {
            Gizmos.color = Color.red;

            // disegna una semplice rappresentazione del collider ruotato
            Vector3 colliderSize = new Vector3(_capsule.radius * 2, _capsule.height, _capsule.radius * 2);
            Vector3 colliderCenter = transform.position + _capsule.center;

            Gizmos.DrawWireCube(colliderCenter, colliderSize); // <- disegna un cubo che rappresenta approssimativamente il collider ruotato

            Gizmos.color = Color.yellow;
            Vector3 forward = transform.forward * (_capsule.height * 0.5f);
            Gizmos.DrawLine(colliderCenter - forward, colliderCenter + forward); // <- disegna anche una linea per indicare la direzione della rotazione
        }
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles")) _animator.SetTrigger(hitTrigger);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Default")) _animator.SetTrigger(crashedTrigger);
    }
}