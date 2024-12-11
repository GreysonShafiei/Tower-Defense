using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 50f;
    public float Damage = 5f;
    public GameObject impactEffect;

    public void Follow(Transform turretTarget)
    {
        target = turretTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            Hit();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void Hit()
    {
        GameObject eff = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(eff, 2f);

        // Check if the target has a Zombie or ZombieLeader component
        ZombieFollower zombieFollower = target.GetComponent<ZombieFollower>();
        ZombieLeader zombieLeader = target.GetComponent<ZombieLeader>();
        Turret turret = target.GetComponent<Turret>();

        if (zombieFollower != null)
        {
            zombieFollower.TakeDamage(Damage); // Apply damage to the zombie
        }
        else if (zombieLeader != null)
        {
            zombieLeader.TakeDamage(Damage); // Apply damage to the zombie leader

        }
        else if (turret != null)
        {
            turret.TakeDamage(Damage);
        }

        Destroy(gameObject); // Destroy the bullet
    }
}
