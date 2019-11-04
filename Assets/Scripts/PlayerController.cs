using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Transform gunHand;
    public float moveSpeed;
    public float dashSpeed;
    public float dashLength;
    public float dashCooldown;

    private float dashCounter;
    private float dashCoolCounter;

    public GameObject projectileGO;
    public Transform projectilePoint;

    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;

    private Vector2 moveInput;

    private float activeMoveSpeed;

    public float timeBetweenShots = 1f;
    private float shotCounter;

    private bool canMove = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();

        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Normalize movement vector to handle diagonal movement.
        moveInput.Normalize();

        _myRigidbody.velocity = moveInput * activeMoveSpeed;

        //TODO: implement reload mechanic - based on shot rythm?
        // maybe we only "use up" ammunition of we miss a beat?
        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
        {
            if (shotCounter <= 0)
            {
                Instantiate(projectileGO, projectilePoint.position, projectilePoint.rotation);
                shotCounter = timeBetweenShots;

                AudioManager.Instance.PlaySFX(12);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (CanDash())
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                dashCoolCounter = dashCooldown;

                _myAnimator.SetTrigger("toDash");

                AudioManager.Instance.PlaySFX(8);

                PlayerHealthController.Instance.MakeInvincible(dashLength);
            }
        }

        HandleAnimation();

        Counter();
    }

    private void Counter()
    {
        if (shotCounter > 0)
            shotCounter -= Time.deltaTime;

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }

        if (dashCoolCounter > 0)
            dashCoolCounter -= Time.deltaTime;
    }

    private void HandleAnimation()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = CameraController.Instance._mainCamera.WorldToScreenPoint(transform.localPosition);

        // Rotate the gun hand.
        Vector2 offset = new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        gunHand.rotation = Quaternion.Euler(0f, 0f, angle);

        //  Flip the player to look the way the mouse if facing.
        if (mousePos.x < playerPos.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunHand.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunHand.localScale = Vector3.one;
        }

        if (moveInput != Vector2.zero)
        {
            _myAnimator.SetBool("isWalking", true);
        }
        else
        {
            _myAnimator.SetBool("isWalking", false);
        }
    }

    public void FreezePlayer()
    {
        canMove = false;
        _myRigidbody.isKinematic = true;
        _myRigidbody.velocity = Vector3.zero;
        _myAnimator.SetBool("isWalking", false);
    }

    public bool CanDash()
    {
        return dashCounter <= 0 && dashCoolCounter <= 0;
    }

    public bool IsDashing()
    {
        return dashCounter > 0;
    }
}
