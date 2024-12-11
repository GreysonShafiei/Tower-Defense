using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndNodeScript : MonoBehaviour
{
    public float health = 100f; // Starting health of the end node
    public GameObject SpawnedZombies; // Holds all of the zombies

    // Start is called before the first frame update
    void Start()
    {
        // Initialize health (if not already set in the Inspector)
        if (health <= 0f)
        {
            health = 100f;
        }
    }

    // Detect when a zombie enters the end node's trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding is a zombie leader
        ZombieLeader zombieLeader = other.GetComponent<ZombieLeader>();

        if (zombieLeader != null)
        {
            // If a zombie leader reaches the end, reduce the end node's health
            TakeDamage(10f); // You can adjust the damage amount as needed
        }
    }

    // Function to reduce health of the end node
    void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log("End Node took damage. Current health: " + health);

        // Check if health falls to zero or below
        if (health <= 0f)
        {
            OnEndNodeDestroyed();
        }
    }

    // What happens when the end node's health reaches 0
    void OnEndNodeDestroyed()
    {
        Destroy(SpawnedZombies);
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
        Debug.Log("End Node has been destroyed!");
    }
}
