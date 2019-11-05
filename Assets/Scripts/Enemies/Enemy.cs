using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 150;

    public float moveSpeed;

    public bool chasesPlayer;
    public bool shouldRunAway;
    public float aggroRadius;

    private Vector3 moveDirection;

    [HideInInspector]
    public Rigidbody2D _myRigidbody;
    [HideInInspector]
    public SpriteRenderer _mySpriteRenderer;
    [HideInInspector]
    public Animator _myAnimator;

    public GameObject hurtParticles;
    public GameObject[] deathSplatters;

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //  Always make sure to deactivate enemy update loop if the enemy is not on screen
        //  or if the player is inactive for any reason (maybe he is loading or switching scenes etc.)
        if (!_mySpriteRenderer.isVisible || !PlayerController.Instance.gameObject.activeInHierarchy) return;

        moveDirection = Vector3.zero;

        if (chasesPlayer && IsPlayerInRange())
        {
            moveDirection = PlayerController.Instance.transform.position - transform.position;
        }
        else if (shouldRunAway && IsPlayerInRange())
        {
            moveDirection = transform.position - PlayerController.Instance.transform.position;
        }

        HandleAnimation();

        moveDirection.Normalize();
        _myRigidbody.velocity = moveDirection * moveSpeed;
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= aggroRadius;
    }

    private void HandleAnimation()
    {
        if (transform.position.x < PlayerController.Instance.transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }

        if (moveDirection != Vector3.zero)
        {
            _myAnimator.SetBool("isWalking", true);
        }
        else
        {
            _myAnimator.SetBool("isWalking", false);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Instantiate(hurtParticles, transform.position, transform.rotation);

        AudioManager.Instance.PlaySFX(2);

        if (health <= 0)
        {
            int randomSplatter = Random.Range(0, deathSplatters.Length);
            int randomRotaion = Random.Range(0, 4);
            Instantiate(deathSplatters[randomSplatter], transform.position, Quaternion.Euler(0f, 0f, 90f * randomRotaion));
            Destroy(gameObject);

            AudioManager.Instance.PlaySFX(1);
        }
    }
}