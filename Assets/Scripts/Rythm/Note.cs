using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float beat;

    private bool isHittable = false;

    private float hitDelay = .25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Beat")
        {
            isHittable = true;
            StartCoroutine(DelayHitReaction());
        }
    }

    IEnumerator DelayHitReaction()
    {
        yield return new WaitForSeconds(hitDelay);
        isHittable = false;
    }

    public void Hide()
    {
        GetComponent<Image>().enabled = false;
    }

    public bool IsHittable()
    {
        return isHittable;
    }
}
