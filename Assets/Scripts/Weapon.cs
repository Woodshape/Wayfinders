using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ShotType shotType;

    public int numberOfProjectiles;
    public int ammunition;

    public float timeBetweenShots = 1f;
    private float shotCounter;

    public float burstDelay;
    public float spreadAngle;

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
            if (Input.GetButtonDown("Fire1"))
            {
                if (Conductor.Instance.IsOnBeat())
                {
                    FireProjectile();
                }
                // else
                // {
                //     if (ammunition > 0)
                //     {
                //         FireProjectile();
                //         ammunition -= numberOfProjectiles;
                //     }
                // }

                shotCounter = timeBetweenShots;
            }
        }
    }

    void FireProjectile()
    {
        switch (shotType)
        {
            case ShotType.SingleFire:
                SingleShot();
                break;
            case ShotType.Burst:
                StartCoroutine(BurstShot());
                break;
            case ShotType.Spread:
                SpreadShot();
                break;
            default:
                SingleShot();
                break;
        }
    }

    void SingleShot()
    {
        Instantiate(projectileGO, projectilePoint.position, projectilePoint.rotation);
        AudioManager.Instance.PlaySFX(12);
    }

    IEnumerator BurstShot()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Instantiate(projectileGO, projectilePoint.position, projectilePoint.rotation);
            AudioManager.Instance.PlaySFX(12);
            yield return new WaitForSeconds(burstDelay);
        }
    }

    void SpreadShot()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject bullet = Instantiate(projectileGO, projectilePoint.position, projectilePoint.rotation);

            // Apply stray
            bullet.transform.Rotate(0, 0, Random.Range(-spreadAngle, spreadAngle));
        }

        AudioManager.Instance.PlaySFX(12);
    }
}

public enum ShotType
{
    SingleFire,
    Burst,
    Spread
}
