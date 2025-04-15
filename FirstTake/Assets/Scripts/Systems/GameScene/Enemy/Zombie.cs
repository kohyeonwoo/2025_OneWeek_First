using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{

    public List<GameObject> attackCollisions;

    private void Start()
    {
        maxHealth = 5.0f;
        currentHealth = maxHealth;
    }

    public override void EnemyActiveAttackCollision1()
    {
        attackCollisions[0].SetActive(true);
    }

    public override void EnemyDeActiveAttackCollision1()
    {
        attackCollisions[0].SetActive(false);
    }

    public override void EnemyActiveAttackCollision2()
    {
        attackCollisions[1].SetActive(true);
    }

    public override void EnemyDeActiveAttackCollision2()
    {
        attackCollisions[1].SetActive(false);
    }

}
