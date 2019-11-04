using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject[] brokenPieces;

    public int breakThreshold = 3;

    private int maxPieces = 5;

    public bool canDropItems;

    public List<ItemsToDrop> itemsToDrop;

    public void LowerBreakingThreshold()
    {
        breakThreshold--;

        if (breakThreshold <= 0)
        {
            Break();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Player") return;

        if (other.gameObject.GetComponent<PlayerController>().IsDashing())
        {
            Break();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag != "Player") return;

        if (other.gameObject.GetComponent<PlayerController>().IsDashing())
        {
            Break();
        }
    }

    private void Break()
    {
        for (int i = 0; i < maxPieces; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            int randomRotaion = Random.Range(0, 4);
            Instantiate(brokenPieces[randomPiece], transform.position, Quaternion.Euler(0f, 0f, 90f * randomRotaion));

            DropItems();

            Destroy(gameObject);
        }

        AudioManager.Instance.PlaySFX(0);
    }

    private void DropItems()
    {
        if (canDropItems)
        {
            foreach (ItemsToDrop item in itemsToDrop)
            {
                float dropRoll = Random.Range(0f, 100f);

                if (dropRoll < item.dropChance)
                {
                    Instantiate(item.item, transform.position, transform.rotation);
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public struct ItemsToDrop
{
    [SerializeField]
    public GameObject item;
    [SerializeField]
    public float dropChance;
}
