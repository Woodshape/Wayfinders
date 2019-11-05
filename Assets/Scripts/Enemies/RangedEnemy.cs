using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{

    public float fireDistance;

    public GameObject projectileGO;
    public Transform projectilePoint;

    public float fireRate = 1f;
    private float fireCounter;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (IsInFireRange())
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

    private bool IsInFireRange()
    {
        return Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= fireDistance;
    }
}
