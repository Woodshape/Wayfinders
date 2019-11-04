using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private int currentHealth;
    public int maxHealth;

    public GameObject hurtParticles;
    public SpriteRenderer playerBodyRenderer;

    private GameObject player;

    private float damageInvincibility = 1f;
    private float invincibilityCounter;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance.gameObject;
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            //  Invincibility timer is over
            if (invincibilityCounter <= 0)
            {
                playerBodyRenderer.color = new Color(playerBodyRenderer.color.r, playerBodyRenderer.color.g, playerBodyRenderer.color.b, 1f);
            }
        }
    }

    public void DamagePlayer(int amount)
    {
        if (invincibilityCounter > 0) return;

        currentHealth -= amount;

        UIUpdate();

        Instantiate(hurtParticles, player.transform.position, player.transform.rotation);

        if (currentHealth <= 0)
        {
            UIController.instance.deathScreen.SetActive(true);
            player.SetActive(false);
        }

        MakeInvincible(damageInvincibility);
    }

    public void MakeInvincible(float duration)
    {
        invincibilityCounter = duration;
        playerBodyRenderer.color = new Color(playerBodyRenderer.color.r, playerBodyRenderer.color.g, playerBodyRenderer.color.b, 0.5f);
    }

    private void UIUpdate()
    {
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
