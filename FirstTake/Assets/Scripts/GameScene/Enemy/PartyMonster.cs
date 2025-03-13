using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMonster : Enemy
{

    public GameObject attackCollision;

    private void Start()
    {
        maxHealth = 3.0f;
        currentHealth = maxHealth;
    }

    public override void EnemyActiveAttackCollision1()
    {
        attackCollision.SetActive(true);
    }

    public override void EnemyDeActiveAttackCollision1()
    {
        attackCollision.SetActive(false);
    }


}
