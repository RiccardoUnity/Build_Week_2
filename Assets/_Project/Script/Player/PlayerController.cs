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

    [Header("Jump Settings")]
    public float jumpForce = 7f;

    [Header("Slide Settings")]
    public Animator animator;
    public float slideDuration = 1f;
    private bool _isSliding = false;
    private float _originalColliderHeight;
    private Vector3 _originalColliderCenter;
    private CapsuleCollider _capsule;

    [Header("Ground Check Settings")]
    public LayerMask groundLayerMask = 5;
    public float groundCheckOffset = 1.01f;
    public float groundCheckRadius = .2f;
    private bool _isGrounded;

    [Header("Debug Features")]
    public bool showGroundCheckGizmos = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _capsule = GetComponent<CapsuleCollider>();
        _originalColliderHeight = _capsule.height;
        _originalColliderCenter = _capsule.center;
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

    #region Move and Lane changing
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

    #region Slide and Jump
    private IEnumerator Slide() // <- gestisce la scivolata
    {
        _isSliding = true;

        animator.SetBool("isSliding", true);

        _capsule.height = 1f;
        _capsule.center = new Vector3(0f, -.5f, 0f);

        yield return new WaitForSeconds(slideDuration);

        _capsule.height = _originalColliderHeight;
        _capsule.center = _originalColliderCenter;

        animator.SetBool("isSliding", false);

        _isSliding = false;
    }


    private void Jump() // <- gestisce il salto
    {
        Vector3 vel = _rb.velocity;
        vel.y = 0f;
        _rb.velocity = vel;

        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    #endregion

    #region Ground Checker
    private void GroundChecker() // <- check sphere sotto i piedi per verificare il contatto col terreno
    {
        _isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckOffset, 0.2f, groundLayerMask);
    }

    private void OnDrawGizmosSelected() // <- disegna un gizmo relativo alla check sphere
    {
        if (!showGroundCheckGizmos) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckOffset, groundCheckRadius);
    }
    #endregion
}