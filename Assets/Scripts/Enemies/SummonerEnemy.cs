using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerEnemy : Enemy
{
    public float summonTime;
    private float summonCounter;

    public GameObject enemyToSummon;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (summonCounter > 0)
            summonCounter -= Time.deltaTime;

        if (summonCounter <= 0)
        {
            _myAnimator.SetTrigger("tSummon");

            Instantiate(enemyToSummon, transform.position, transform.rotation);

            summonCounter = summonTime;

            AudioManager.Instance.PlaySFX(14);
        }
    }
}
