using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Sting : Player
{

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float range;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float speed;

    private float distance;

    public TextMeshProUGUI stingHealth;

    private void Start()
    {

        Init();

        maxHealth = 20.0f;
        currentHealth = maxHealth;

        range = 6.5f;
        speed = 3.0f;

    }

    private void Update()
    {

        UpdateTarget();

        if (target == null)
        {
            anim.SetBool("bAttack", false);
            return;
        }

        stingHealth.text = currentHealth.ToString();

        Vector3 direction = target.position - transform.position;

        distance = Vector3.Distance(this.transform.position, target.position);

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, Time.deltaTime * 3).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);

        float distanceThisFrame = speed * Time.deltaTime;

        if (distance < 2.0f)
        {
            anim.SetBool("bAttack", true);
        }


    }

    public void FindTarget(Transform Target)
    {
        target = Target;
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;

                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

}
