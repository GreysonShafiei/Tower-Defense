using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 15f; //Turret Range
    public int health = 100; //Turret's Health
    public float fireRate = 1f;
    private float fireCountDown = 0f;

    [Header("Setup")]
    public Transform rotate; //Rotating piece of turret
    public float rotateSpeed = 10f;
    public string enemyTag = "Enemy";
    public GameObject bulletType;
    public Transform fireLocation;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TargetUpdate", 0f, 0.5f);
    }

    void TargetUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (enemyDistance < closestDistance)
            {
                closestDistance = enemyDistance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && closestDistance <= range)
        {
            target = closestEnemy.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || Vector3.Distance(transform.position, target.position) > range)
        {
            target = null;
            return;
        }

        Vector3 dir = target.position - transform.position;
        Quaternion rotationLook = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(rotate.rotation, rotationLook, Time.deltaTime * rotateSpeed).eulerAngles;
        rotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); //Rotate towards enemy

        if (fireCountDown <=0)
        {
            Fire();
            fireCountDown = 1f/ fireRate;
        }

        fireCountDown -= Time.deltaTime;
    }

    // Firing Mechanism
    void Fire()
    {
        GameObject bulletFollow = (GameObject) Instantiate(bulletType, fireLocation.position, fireLocation.rotation);
        Debug.Log("Bullet instantiated: " + bulletFollow.name);
        Bullet bullet  = bulletFollow.GetComponent<Bullet>();

        if (bullet != null)
        {
            Debug.Log("Assigning target to bullet: " + target.name);
            bullet.Follow(target);
        }
        else
        {
            Debug.LogError("Bullet component not found on instantiated object.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
