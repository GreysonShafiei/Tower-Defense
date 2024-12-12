using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombieLeaderPrefab;  // Reference to the zombie leader prefab
    public GameObject zombieFollowerPrefab;  // Reference to the zombie follower prefab
    public Transform spawnPoint;  // Spawnpoint in the scene
    public Transform SpawnedZombies; // What object to spawn the zombies as a child of
    public float spawnDelay = 2f;    // Delay between spawns

    public TextMeshProUGUI CountDownText;

    private int waveCounter = 0;
    private int zombieCount = 0; // Counter to track how many zombies have been spawned
    private ZombieLeader currentLeader; // Reference to the current leader zombie
    private List<ZombieFollower> currentFollowers = new List<ZombieFollower>(); // List of followers for the current leader

    private void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            // Delay between spawns
            yield return new WaitForSeconds(spawnDelay);

            // Every 5th zombie will be a leader
            if (zombieCount % 5 == 0)
            {
                // Spawn a new zombie leader
                SpawnLeader(spawnPoint.position);
            }
            else
            {
                // Spawn a new zombie follower
                SpawnFollower(spawnPoint.position);
            }

            // Increment zombie count after spawning
            zombieCount++;
            waveCounter++;
            // Increase health every 5 waves
            if (waveCounter % 5 == 0)
            {
                IncreaseZombieHealth();
            }
        }
    }

    void IncreaseZombieHealth()
    {
        // Adjust the health values for the leader and follower prefabs
        ZombieLeader leader = zombieLeaderPrefab.GetComponent<ZombieLeader>();
        if (leader != null)
        {
            leader.health += 25f; // Increase leader's health by 25 (example value)
        }

        ZombieFollower follower = zombieFollowerPrefab.GetComponent<ZombieFollower>();
        if (follower != null)
        {
            follower.health += 15f; // Increase follower's health by 15 (example value)
        }
    }

    void SpawnLeader(Vector3 spawnPosition)
    {
        // Instantiate a new zombie leader at the specified spawn position
        GameObject newZombieLeader = Instantiate(zombieLeaderPrefab, spawnPosition, Quaternion.identity, SpawnedZombies);

        // Get the ZombieLeader component
        currentLeader = newZombieLeader.GetComponent<ZombieLeader>();

        // Clear the list of followers since this is a new group
        currentFollowers.Clear();
    }

    void SpawnFollower(Vector3 spawnPosition)
    {
        // Instantiate a new zombie follower at the specified spawn position
        GameObject newZombieFollower = Instantiate(zombieFollowerPrefab, spawnPosition, Quaternion.identity, SpawnedZombies);

        // Get the ZombieFollower component and set the leader as its target
        ZombieFollower follower = newZombieFollower.GetComponent<ZombieFollower>();
        if (currentLeader != null)
        {
            follower.SetLeader(currentLeader); // Use SetLeader to assign the current leader
            currentFollowers.Add(follower); // Add follower to the leader's follower list
            currentLeader.followers.Add(follower); // Update the leader's follower list
        }
    }
}