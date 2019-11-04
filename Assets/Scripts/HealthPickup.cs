using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    private float waitToBeCollected = 0.5f;

    void Update()
    {
        if (waitToBeCollected > 0)
            waitToBeCollected -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (waitToBeCollected <= 0 && PlayerHealthController.Instance.HealDamage(healAmount))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (waitToBeCollected <= 0 && PlayerHealthController.Instance.HealDamage(healAmount))
            {
                Destroy(gameObject);
            }
        }
    }
}
