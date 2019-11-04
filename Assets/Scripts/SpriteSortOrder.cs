using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortOrder : MonoBehaviour
{
    private SpriteRenderer _mySpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }

    //  FIXME: it might be a bad idea to calculate the sorting order in Update / LateUpdate.
    // void LateUpdate()
    // {
    //     _mySpriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    // }
}
