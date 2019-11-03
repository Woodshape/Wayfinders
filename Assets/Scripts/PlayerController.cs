using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform gunHand;
    public float moveSpeed;

    public GameObject bulletGO;
    public Transform bulletPoint;

    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;
    private Camera _mainCamera;

    private Vector2 moveInput;

    public float timeBetweenShots = 1f;
    private float shotCounter;

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Normalize movement vector to handle diagonal movement.
        moveInput.Normalize();

        HandleAnimation();

        _myRigidbody.velocity = moveInput * moveSpeed;

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = _mainCamera.WorldToScreenPoint(transform.localPosition);

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

        // Rotate the gun hand.
        Vector2 offset = new Vector2(mousePos.x - playerPos.x, mousePos.y - playerPos.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        gunHand.rotation = Quaternion.Euler(0f, 0f, angle);

        //TODO: implement reload mechanic - based on shot rythm?
        // maybe we only "use up" ammunition of we miss a beat?
        shotCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
        {
            if (shotCounter <= 0)
            {
                Instantiate(bulletGO, bulletPoint.position, bulletPoint.rotation);
                shotCounter = timeBetweenShots;
            }
        }
    }

    private void HandleAnimation()
    {
        if (moveInput != Vector2.zero)
        {
            _myAnimator.SetBool("isWalking", true);
        }
        else
        {
            _myAnimator.SetBool("isWalking", false);
        }
    }
}
