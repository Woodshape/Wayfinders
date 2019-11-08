using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Slider healthSlider;
    public Text healthText;

    public Text ammoText;

    public GameObject deathScreen;

    public Image weaponHUD;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
