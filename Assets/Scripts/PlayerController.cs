using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public List<Weapon> weapons = new List<Weapon>();
    private Weapon currentWeapon;

    public Transform gunHand;
    public float moveSpeed;
    public float dashSpeed;
    public float dashLength;
    public float dashCooldown;

    private float dashCounter;
    private float dashCoolCounter;

    private Rigidbody2D _myRigidbody;
    private Animator _myAnimator;

    private Vector2 moveInput;

    private float activeMoveSpeed;

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

        currentWeapon = weapons[0];
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int index = 0;

            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i] == currentWeapon)
                {
                    index = i + 1;
                    if (index >= weapons.Count) index = 0;
                }
            }

            SwitchWeapon(weapons[index]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weapons.Count >= 0)
                SwitchWeapon(weapons[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weapons.Count >= 1)
                SwitchWeapon(weapons[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weapons.Count >= 2)
                SwitchWeapon(weapons[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (weapons.Count >= 3)
                SwitchWeapon(weapons[3]);
        }

        HandleAnimation();

        Counter();
    }

    private void Counter()
    {
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

    public void SwitchWeapon(Weapon weaponToSwitchTo)
    {
        if (weapons.Count > 0)
        {
            foreach (Weapon weapon in weapons)
            {
                weapon.gameObject.SetActive(false);
            }

            currentWeapon = weaponToSwitchTo;
            currentWeapon.ShowWeapon();

            _myAnimator.SetBool("isReloading", false);
        }
        else
        {
            Debug.LogError("NO WEAPONS AVAILIABLE ON PLAYER");
        }
    }

    public bool PickupWeapon(Weapon weaponToPickup)
    {
        bool hasSameWeapon = false;

        foreach (Weapon availiableWeapon in weapons)
        {
            if (availiableWeapon.weaponName == weaponToPickup.weaponName)
            {
                hasSameWeapon = true;
            }
        }

        if (!hasSameWeapon)
        {
            Weapon weapon = Instantiate(weaponToPickup);
            weapon.transform.parent = gunHand;
            weapon.transform.position = gunHand.transform.position;
            weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
            weapon.transform.localScale = Vector3.one;

            weapons.Add(weapon);

            SwitchWeapon(weapon);

            return true;
        }

        return false;
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

    public Animator GetMyAnimator()
    {
        return _myAnimator;
    }
}
