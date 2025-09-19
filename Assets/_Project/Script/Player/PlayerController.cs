using SGM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float forwardSpeed = 5f;
    private Rigidbody _rb;
    private bool _isAlive = true;

    public float horizontalMultiplier = 2f;
    private float _horizontalInput;

    public float onLaneDistance = 3f;
    public float laneMultiplier = 120f;
    private int _laneInput = 1; // <- 0: sinistra   1: centro   2: destra

    [Header("Running Settings")]
    public float runningSoundInterval = .5f;
    private float _lastRunningSoundTime;

    [Header("Jump Settings")]
    public float jumpForce = 4f;
    private bool _isJumping = false;

    [Header("Slide Settings")]
    public float slideDuration = 1f;
    private bool _isSliding = false;
    private float _originalColliderHeight;
    private float _colliderResize = .7f;
    private CapsuleCollider _capsule;

    [Header("Wings PowerUp Settings")]
    public float wingsHeight = 5f;
    public float wingsTransitionSpeed = 3f;
    private bool _isUsingWings = false;
    private Vector3 _originalPosition;
    private float _originalGroundCheckOffset;
    private bool _wingsTransitionInProgress = false;


    [Header("Ground Check Settings")]
    public LayerMask groundLayerMask = 5;
    public float groundCheckOffset = 1.01f;
    public float groundCheckRadius = .2f;
    private bool _isGrounded;

    [Header("Debug Features")]
    public bool showGroundCheckGizmos = true;
    public bool showSlideColliderGizmos = false;

    [Header("Unity Events")]
    public UnityEvent playerDeadEvent;

    public bool IsJumping => _isJumping;
    public bool IsSliding => _isSliding;
    public bool IsAlive => _isAlive;
    public bool IsUsingWings => _isUsingWings;
    public bool WingsTransitionInProgress => _wingsTransitionInProgress;

    private PlayerAnimator _playerAnimator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _capsule = GetComponent<CapsuleCollider>();
        _originalColliderHeight = _capsule.height;
        _playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void FixedUpdate()
    {
        if (_isAlive) Move();
    }

    private void Update()
    {
        if(_isAlive)
        {
            ChangeLane();

            GroundChecker();

            if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(Slide());

            if (Input.GetKeyDown(KeyCode.W) && _isGrounded) Jump();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            _playerAnimator.TriggerFallAnimation();
            PlayerDead();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("High Obstacles"))
        {
            _playerAnimator.TriggerCrashAnimation();
            PlayerDead();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            _playerAnimator.TriggerDeathAnimation();
            PlayerDead();
        }
    }

    #region Move & Lane changing
    private void Move() // <- gestisce il movimento continuo del Player
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        float forcedHorizontalInput = _horizontalInput;

        if (_laneInput == 0 && _horizontalInput < 0) forcedHorizontalInput = 0; // <- corsia sinistra, input sinistra
        if (_laneInput == 2 && _horizontalInput > 0) forcedHorizontalInput = 0; // <- corsia destra, input destra

        Vector3 fwdMove = transform.forward * (forwardSpeed * S_GameManager.Difficulty() * Time.fixedDeltaTime);
        Vector3 horMove = transform.right * (forcedHorizontalInput * horizontalMultiplier) * (forwardSpeed * Time.fixedDeltaTime);

        _rb.MovePosition(_rb.position + fwdMove + horMove);

        HandleRunningSounds();
    }

    private void ChangeLane() // <- gestisce il cambio di corsia
    {
        if (!_isGrounded) return;

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

    #region Running, Slide & Jump
    private void HandleRunningSounds() // <- gestisce il suono dei passi
    {
        if (_isGrounded && !_isSliding && Time.time - _lastRunningSoundTime >= runningSoundInterval)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayRunningSound();
            _lastRunningSoundTime = Time.time;
        }
    }

    private IEnumerator Slide() // <- gestisce la scivolata
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
    private void GroundChecker() // <- check sphere per verificare il contatto con il terreno
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckOffset, 0.2f, groundLayerMask);
        if (!wasGrounded && _isGrounded && _isJumping) _isJumping = false; // <- se il player � appena atterrato, smette di considerarlo in salto
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

    #region Death
    private void PlayerDead()
    {
        _isAlive = false;
        playerDeadEvent.Invoke();
    }
    #endregion

    #region Wings PowerUp
    public void EnterWings()
    {
        if (_isUsingWings || _wingsTransitionInProgress) return; // <- per prevenire attivazioni multiple

        _isUsingWings = true;
        _wingsTransitionInProgress = true;

        _originalPosition = transform.position;
        _originalGroundCheckOffset = groundCheckOffset;
        
        _rb.isKinematic = true;

        groundCheckOffset = _originalGroundCheckOffset + wingsHeight; // <- aggiusta il ground check per la nuova altezza

        StopAllCoroutines(); // <- ferma eventuali slide o jump in corso
        _isSliding = false;
        _isJumping = false;
        
        if (_capsule.height != _originalColliderHeight) _capsule.height = _originalColliderHeight; // <- ripristina il collider se era ridotto per lo slide

        StartCoroutine(TakeOff()); // <- inizia la coroutine per il movimento verso l'alto
    }

    public void ExitWings()
    {
        if (!_isUsingWings || _wingsTransitionInProgress) return; // <- per prevenire disattivazioni multiple

        _wingsTransitionInProgress = true;

        StartCoroutine(Landing()); // <- inizia la coroutine per il movimento verso il basso

    }

    private IEnumerator TakeOff()
    {
        float startPos = transform.position.y;
        float targetPos = wingsHeight;

        float elapsedTime = 0f;
        float transitionDuration = wingsHeight / wingsTransitionSpeed;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            t = Mathf.Sin(t * Mathf.PI * 0.5f); // <- usa una curva smooth con Sin per un movimento molto fluido

            float currentPos = Mathf.Lerp(startPos, targetPos, t);
            transform.position = new Vector3(transform.position.x, currentPos, transform.position.z);

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetPos, transform.position.z);
        _wingsTransitionInProgress = false;
    }

    private IEnumerator Landing()
    {
        float startPos = transform.position.y;
        float targetPos = 0f;

        float elapsedTime = 0f;
        float transitionDuration = wingsHeight / wingsTransitionSpeed;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            t = Mathf.Sin(t * Mathf.PI * 0.5f); // <- usa una curva smooth con Sin per un movimento molto fluido

            float currentPos = Mathf.Lerp(startPos, targetPos, t);
            transform.position = new Vector3(transform.position.x, currentPos, transform.position.z);

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetPos, transform.position.z);

        _rb.isKinematic = false;

        groundCheckOffset = _originalGroundCheckOffset; // <- ripristina il ground check offset originale

        _isUsingWings = false;
        _wingsTransitionInProgress = false;
    }
    #endregion
}