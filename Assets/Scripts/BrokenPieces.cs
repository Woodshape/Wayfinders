using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float deceleration = 5f;
    public float lifetime = 10f;
    public float fadeSpeed = 5f;

    private Vector3 moveDirection;

    private SpriteRenderer _mySpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;

        // Make the broken pieces slow down over time (to make it look like they are being scattered away).
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifetime -= Time.deltaTime;

        if (lifetime < 0)
        {
            _mySpriteRenderer.color = new Color(_mySpriteRenderer.color.r, _mySpriteRenderer.color.g, _mySpriteRenderer.color.b,
                Mathf.MoveTowards(_mySpriteRenderer.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (_mySpriteRenderer.color.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
