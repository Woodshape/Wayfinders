using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ShotType shotType;

    public int numberOfProjectiles;
    private int ammunition;
    public int maximumAmmunition;

    public float reloadTime;
    private float reloadCounter;

    public float timeBetweenShots = 1f;
    private float shotCounter;

    public float burstDelay;
    public float spreadAngle;

    public GameObject projectileGO;
    public Transform projectilePoint;

    private void Start()
    {
        ammunition = maximumAmmunition;

        UpdateAmmoCounter();
    }

    void Update()
    {
        if (shotCounter > 0)
        {
            shotCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shotCounter = timeBetweenShots;

                foreach (GameObject note in Conductor.Instance.spawnedNotes)
                {
                    if (note.GetComponent<Note>().IsHittable())
                    {
                        FireProjectile(false);
                        return;
                    }
                }

                FireProjectile(true);
            }
        }

        if (reloadCounter > 0)
        {
            reloadCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R) && ammunition < maximumAmmunition)
            {
                StartCoroutine(ReloadWeapon());
            }
        }
    }

    void FireProjectile(bool usesAmmo)
    {
        if (ammunition > 0 || !usesAmmo)
        {
            if (usesAmmo)
            {
                ammunition -= numberOfProjectiles;
                UpdateAmmoCounter();
            }

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
    }

    public IEnumerator ReloadWeapon()
    {
        //  Animation stuff
        yield return new WaitForSeconds(reloadTime);

        ammunition = maximumAmmunition;
        reloadCounter = reloadTime;

        UpdateAmmoCounter();
    }

    void SingleShot()
    {
        Instantiate(projectileGO, projectilePoint.position, projectilePoint.rotation);
        // AudioManager.Instance.PlaySFX(12);
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

        // AudioManager.Instance.PlaySFX(12);
    }

    void UpdateAmmoCounter()
    {
        UIController.Instance.ammoText.text = ammunition.ToString();
    }
}

public enum ShotType
{
    SingleFire,
    Burst,
    Spread
}
