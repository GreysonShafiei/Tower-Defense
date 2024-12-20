using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 15f; //Turret Range
    public float health = 100; //Turret's Health
    public float fireRate = 1f;
    public float cost = 100;
    private float fireCountDown = 0f;

    [Header("Setup")]
    public Transform rotate; //Rotating piece of turret
    public float rotateSpeed = 10f;
    public string leaderTag = "Leader";
    public string followerTag = "Follower";
    public GameObject bulletType;
    public Transform fireLocation;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("TargetUpdate", 0f, 0.5f);
    }

    void TargetUpdate()
    {
        List<GameObject> enemiesList = new List<GameObject>();

        enemiesList.AddRange(GameObject.FindGameObjectsWithTag(followerTag));
        enemiesList.AddRange(GameObject.FindGameObjectsWithTag(leaderTag));

        GameObject[] enemies = enemiesList.ToArray();
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
        Bullet bullet  = bulletFollow.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Follow(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void TakeDamage(float damage)
    {
        health -= damage; // Decrease health
        if (health <= 0f)
        {
            Die(); // Call the death method
        }
    }

    // Destroy Turret
    void Die()
    {
        Destroy(gameObject);
    }
}
