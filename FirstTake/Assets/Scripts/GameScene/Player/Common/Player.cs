using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
   // public Transform target;  
    protected NavMeshAgent agent;
    public GameObject attackCollision;
    public GameObject particle;
    public GameObject deadParticle;
    public Transform particleLocation;

    protected float maxHealth;
    protected float currentHealth;

    protected Animator anim;
    protected Rigidbody rigid;

    protected void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    public void ActiveAttackCollision()
    {
        attackCollision.SetActive(true);
    }

    public void DeActiveAttackCollision()
    {
        attackCollision.SetActive(false);
    }

    public void Damage(float Damage)
    {
        currentHealth -= Damage;

        AudioManager.Instance.PlaySFX("PlayerHitSound");
        GameObject obj = Instantiate(particle, particleLocation.position, Quaternion.identity);
        Destroy(obj, 2.0f);

        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        GameObject obj = Instantiate(deadParticle, particleLocation.position, Quaternion.identity);
        Destroy(obj, 3.0f);
        // this.gameObject.SetActive(false);
        Destroy(this.gameObject);

        GameManager.Instance.SetActiveEndPanel();
    }

}
