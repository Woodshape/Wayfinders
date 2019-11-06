using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float timeBetweenShots = 1f;
    private float shotCounter;

    public GameObject projectileGO;
    public Transform projectilePoint;

    void Update()
    {
        if (shotCounter > 0)
        {
            shotCounter -= Time.deltaTime;
        }
        else
        {

            //TODO: implement reload mechanic - based on shot rythm?
            // maybe we only "use up" ammunition of we miss a beat?
            if (Input.GetButtonDown("Fire1") || Input.GetButton("Fire1"))
            {
                Instantiate(projectileGO, projectilePoint.position, projectilePoint.rotation);
                shotCounter = timeBetweenShots;

                AudioManager.Instance.PlaySFX(12);
            }
        }
    }
}
