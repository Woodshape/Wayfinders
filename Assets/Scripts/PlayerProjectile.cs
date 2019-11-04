using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damage = 50;
    public float speed = 10f;

    public GameObject impactParticles;

    private Rigidbody2D _myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //  Move the bullet along it's z-axis (so we don't have to care about rotation etc.).
        _myRigidbody.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impactParticles, transform.position, transform.rotation);

        AudioManager.Instance.PlaySFX(4);

        Destroy(gameObject);

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }

        if (other.GetComponent<Breakable>())
        {
            other.GetComponent<Breakable>().LowerBreakingThreshold();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
