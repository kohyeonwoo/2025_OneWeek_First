using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit_LongDistance : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float range = 7.5f;

    [SerializeField]
    private float turnSpeed = 7.5f;

    [SerializeField]
    private string targetTag = "Bug";

    public Transform partToRotate;

    [SerializeField]
    private float fireRate = 1.0f;
    [SerializeField]
    private float fireCoolDown = 0.0f;

    [SerializeField]
    private float health;

    //�Ѿ� ���� �κ� 
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    private Transform muzzleLocation;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0.0f, 0.5f); //--> ���߿� Courutine ���� �ٲٱ�
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        //Ÿ�� ����
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
            lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);

        if (fireCoolDown <= 0.0f)
        {
            Shoot();
            fireCoolDown = 1.0f / fireRate;
        }

        fireCoolDown -= Time.deltaTime;

    }

    private void UpdateTarget()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

        float shortestDistance = Mathf.Infinity; //�� ���� �����Ǿ� ���� ���� ���

        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position,
                enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;

                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }

    }

    private void Shoot()
    {
        Debug.Log("Shoot");

        GameObject bulletGameObject = (GameObject)Instantiate(bulletPrefab, muzzleLocation.position, muzzleLocation.rotation);
        Projectile bullet = bulletGameObject.GetComponent<Projectile>();

        if (bullet != null)
        {
            bullet.FindTarget(target);
        }

    }

    public void SetHealth(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
