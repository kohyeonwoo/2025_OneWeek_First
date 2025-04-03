using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Waypoints : MonoBehaviour, IDamageable
{
    public Animator anim;

    public SkinnedMeshRenderer[] meshes;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float health;

    [SerializeField]
    private int wavePointIndex = 0;

    int y = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
       target = EnemyWayPoints.points[0];
    }

    private void Update()
    {

        anim.SetBool("bMove", true);

        Vector3 direction = target.position - transform.position;

        transform.Translate(direction.normalized * movementSpeed * Time.deltaTime, Space.World);

        transform.rotation = Quaternion.Euler(0, y, 0);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWayPoint();
        }

    }

    private void GetNextWayPoint()
    {

        if (wavePointIndex >= EnemyWayPoints.points.Length - 1)
        {
            target = EnemyWayPoints.points[0];
            wavePointIndex = -1;
        }

        wavePointIndex++;

        y += 90;

        target = EnemyWayPoints.points[wavePointIndex];
    }

    IEnumerator ChangeColor()
    {

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.3f);

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material.color = Color.white;
        }

    }


    public void Damage(float Damage)
    {
        health -= Damage;

        if(health < 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Destroy(this.gameObject);
    }
}
