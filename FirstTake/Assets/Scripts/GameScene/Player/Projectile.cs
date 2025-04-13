using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speed = 70.0f;
    [SerializeField]
    private int attackPoint = 5;

    public GameObject impactEffect;

    public void FindTarget(Transform Target)
    {
        target = Target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;

        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

    }

    private void Damage(Transform Target)
    {
        IDamageable damageable = Target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(attackPoint);
        }
    }

    private void HitTarget()
    {
        Debug.Log("���𰡸� �����");

        Damage(target);

        GameObject effectObject = Instantiate(impactEffect, transform.position, transform.rotation);

        Destroy(effectObject, 0.2f);

        Destroy(gameObject);
    }
}
