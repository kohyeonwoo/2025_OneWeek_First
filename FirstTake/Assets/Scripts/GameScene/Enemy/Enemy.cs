using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{

    protected float maxHealth;
    protected float currentHealth;

    public Transform particleLocation;
    public GameObject particle;

    public virtual void EnemyActiveAttackCollision1() {}

    public virtual void EnemyDeActiveAttackCollision1() {}

    public virtual void EnemyActiveAttackCollision2() {}

    public virtual void EnemyDeActiveAttackCollision2() {}

    public void Damage(float Damage)
    {
        currentHealth -= Damage;

        AudioManager.Instance.PlaySFX("EnemyHitSound");
        GameObject obj = Instantiate(particle, particleLocation.position, Quaternion.identity);
        Destroy(obj, 2.0f);

        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        this.gameObject.SetActive(false);
    }

    

}
