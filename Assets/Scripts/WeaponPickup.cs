using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon;

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
            if (waitToBeCollected <= 0 && PlayerController.Instance.PickupWeapon(weapon))
            {
                Destroy(gameObject);
            }
        }
    }
}
