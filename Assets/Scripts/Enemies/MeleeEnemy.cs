using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float attackDistance;

    public float attackSpeed;
    private float attackCounter;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (IsInAttackRange())
        {
            _myRigidbody.velocity = Vector3.zero;

            if (attackCounter > 0)
            {
                attackCounter -= Time.deltaTime;
            }
            else
            {
                attackCounter = attackSpeed;

                AudioManager.Instance.PlaySFX(13);

                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        PlayerHealthController.Instance.DamagePlayer(1);

        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = PlayerController.Instance.transform.position;

        float percent = 0f;
        while (percent <= 1)
        {

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector2.Lerp(originalPosition, targetPosition, interpolation);
            yield return null;

        }

    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= attackDistance;
    }
}
