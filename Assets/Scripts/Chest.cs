using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public WeaponPickup[] weaponPickups;

    public SpriteRenderer _mySpriteRenderer;
    public Sprite chestOpen;

    public Transform spawnPoint;

    public GameObject notification;

    private bool canBeInteracted = false;
    private bool hasSpawnedLoot = false;


    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (canBeInteracted && Input.GetKeyDown(KeyCode.E))
        {
            _mySpriteRenderer.sprite = chestOpen;

            if (hasSpawnedLoot) return;

            int randomWeapon = Random.Range(0, weaponPickups.Length);

            Instantiate(weaponPickups[randomWeapon], spawnPoint.transform.position, spawnPoint.transform.rotation);

            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            foreach (BoxCollider2D col in GetComponents<BoxCollider2D>())
            {
                col.enabled = false;
            }

            hasSpawnedLoot = true;
        }

        if (hasSpawnedLoot)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canBeInteracted = true;
            notification.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        {
            if (other.tag == "Player")
            {
                canBeInteracted = false;
                notification.SetActive(false);
            }
        }
    }
}
