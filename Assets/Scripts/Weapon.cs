using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public ShotType shotType;

    public float damageModifier = 1f;

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
    public Sprite weaponHUDImage;

    private void Start()
    {
        ammunition = maximumAmmunition;

        UpdateWeaponHUD();

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
            if (Input.GetButtonDown("Fire1") && !PlayerController.Instance.IsDashing())
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
            if (Input.GetKeyDown(KeyCode.R) && CanReload() && !PlayerController.Instance.IsDashing())
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
        PlayerController.Instance.GetMyAnimator().SetBool("isReloading", true);

        yield return new WaitForSeconds(reloadTime);

        PlayerController.Instance.GetMyAnimator().SetBool("isReloading", false);

        ammunition = maximumAmmunition;
        reloadCounter = reloadTime;

        UpdateAmmoCounter();
    }

    void SingleShot()
    {
        SpawnProjectile();
        // AudioManager.Instance.PlaySFX(12);
    }

    IEnumerator BurstShot()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            SpawnProjectile();
            AudioManager.Instance.PlaySFX(12);
            yield return new WaitForSeconds(burstDelay);
        }
    }

    void SpreadShot()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            GameObject bullet = SpawnProjectile();

            // Apply stray
            bullet.transform.Rotate(0, 0, Random.Range(-spreadAngle, spreadAngle));
        }

        // AudioManager.Instance.PlaySFX(12);
    }

    GameObject SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectileGO, projectilePoint.position, projectilePoint.rotation);

        int projectileDamage = projectile.GetComponent<PlayerProjectile>().damage;
        projectile.GetComponent<PlayerProjectile>().damage = Mathf.RoundToInt(projectileDamage * damageModifier);

        return projectile;
    }

    void UpdateAmmoCounter()
    {
        UIController.Instance.ammoText.text = ammunition.ToString();
    }

    void UpdateWeaponHUD()
    {
        UIController.Instance.weaponHUD.sprite = weaponHUDImage;
    }

    public void ShowWeapon()
    {
        this.gameObject.SetActive(true);

        UpdateWeaponHUD();
        UpdateAmmoCounter();
    }

    public bool CanReload()
    {
        return ammunition < maximumAmmunition;
    }
}

public enum ShotType
{
    SingleFire,
    Burst,
    Spread
}
