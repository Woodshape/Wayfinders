using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 150;

    public float moveSpeed;
    public float chaseRadius;

    public bool rangedEnemy;
    public float fireDistance;

    public GameObject projectileGO;
    public Transform projectilePoint;

    private Vector3 moveDirection;

    private Rigidbody2D _myRigidbody;
    private SpriteRenderer _mySpriteRenderer;
    private Animator _myAnimator;

    public float fireRate = 1f;
    private float fireCounter;

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
    void Update()
    {
        if (!_mySpriteRenderer.isVisible && !PlayerController.Instance.gameObject.activeInHierarchy) return;

        if (IsPlayerInRange())
        {
            moveDirection = PlayerController.Instance.transform.position - transform.position;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        HandleAnimation();

        moveDirection.Normalize();
        _myRigidbody.velocity = moveDirection * moveSpeed;

        Attack();
    }

    private void Attack()
    {
        if (rangedEnemy && IsInFireRange())
        {
            fireCounter -= Time.deltaTime;
            if (fireCounter <= 0)
            {
                Instantiate(projectileGO, projectilePoint.position, projectilePoint.rotation);
                fireCounter = fireRate;

                AudioManager.Instance.PlaySFX(13);
            }
        }
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= chaseRadius;
    }

    private bool IsInFireRange()
    {
        return Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= fireDistance;
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
