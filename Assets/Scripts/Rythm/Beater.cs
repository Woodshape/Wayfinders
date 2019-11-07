using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beater : MonoBehaviour
{
    private bool hitNote;

    private Image _myImage;

    private Color onBeatColor = new Color(0f, 1f, 0f, 1f);
    private Color offBeatColor = new Color(1f, 0f, 0f, 1f);

    void Start()
    {
        _myImage = GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Beat")
        {
            hitNote = true;
            _myImage.color = onBeatColor;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Beat")
        {
            hitNote = false;
            _myImage.color = offBeatColor;
        }
    }

    public bool IsOnBeat()
    {
        return hitNote;
    }
}
