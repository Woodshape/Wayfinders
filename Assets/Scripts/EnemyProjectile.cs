using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;

    // public GameObject impactParticles;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = PlayerController.Instance.transform.position - transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);

        if (other.tag == "Player")
        {
            PlayerHealthController.Instance.DamagePlayer(1);
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
