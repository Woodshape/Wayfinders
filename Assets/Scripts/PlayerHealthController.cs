using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;

    private int currentHealth;
    public int maxHealth;

    public GameObject hurtParticles;
    public SpriteRenderer playerBodyRenderer;

    private GameObject player;

    private float damageInvincibility = 1f;
    private float invincibilityCounter;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.Instance.gameObject;
        currentHealth = maxHealth;

        UIController.Instance.healthSlider.maxValue = maxHealth;
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
            UIController.Instance.deathScreen.SetActive(true);
            AudioManager.Instance.PlaySFX(8);
            AudioManager.Instance.PlayGameOver();
            player.SetActive(false);
        }

        AudioManager.Instance.PlaySFX(11);

        MakeInvincible(damageInvincibility);
    }

    public bool HealDamage(int amount)
    {
        int newHealth = currentHealth + amount;

        if (currentHealth == maxHealth) return false;

        AudioManager.Instance.PlaySFX(7);

        if (newHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }

        UIUpdate();

        return true;
    }

    public void MakeInvincible(float duration)
    {
        invincibilityCounter = duration;
        playerBodyRenderer.color = new Color(playerBodyRenderer.color.r, playerBodyRenderer.color.g, playerBodyRenderer.color.b, 0.5f);
    }

    private void UIUpdate()
    {
        UIController.Instance.healthSlider.value = currentHealth;
        UIController.Instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
