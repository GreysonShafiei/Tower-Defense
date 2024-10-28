using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombieLeaderPrefab;  // Reference to the zombie leader prefab
    public GameObject zombieFollowerPrefab;  // Reference to the zombie follower prefab
    public Transform[] spawnPoints;  // Array of spawn points in the scene
    public float spawnDelay = 2f;    // Delay between spawns

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

            // Choose a random spawn point
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

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
        }
    }

    void SpawnLeader(Vector3 spawnPosition)
    {
        // Instantiate a new zombie leader at the specified spawn position
        GameObject newZombieLeader = Instantiate(zombieLeaderPrefab, spawnPosition, Quaternion.identity, transform);

        // Get the ZombieLeader component
        currentLeader = newZombieLeader.GetComponent<ZombieLeader>();

        // Clear the list of followers since this is a new group
        currentFollowers.Clear();
    }

    void SpawnFollower(Vector3 spawnPosition)
    {
        // Instantiate a new zombie follower at the specified spawn position
        GameObject newZombieFollower = Instantiate(zombieFollowerPrefab, spawnPosition, Quaternion.identity, transform);

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