using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFollower : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 5f;

    private Transform endNode; // Reference to the EndNode
    private List<Transform> pathToFollow = new List<Transform>(); // Path provided by the leader
    private int currentWaypointIndex = 0; // Track which waypoint the follower is heading towards

    void Start()
    {
        // Find the EndNode by its tag
        GameObject endNodeObject = GameObject.FindGameObjectWithTag("End");
        if (endNodeObject != null)
        {
            endNode = endNodeObject.transform; // Assign the EndNode's transform
        }
    }

    public void SetLeader(ZombieLeader leader)
    {
        // Set the path to follow to match the leader's path
        pathToFollow = new List<Transform>(leader.pathTaken);
        currentWaypointIndex = 0; // Reset the waypoint index to start following from the beginning
    }

    public void UpdatePath(List<Transform> updatedPath)
    {
        // Only update the path if there are new waypoints to follow
        if (updatedPath.Count > pathToFollow.Count)
        {
            pathToFollow = new List<Transform>(updatedPath);
        }
    }

    private void Update()
    {
        if (endNode == null) return; // Ensure the EndNode has been found

        // Check if the follower has reached the EndNode
        if (Vector3.Distance(transform.position, endNode.position) <= 0.4f)
        {
            // Destroy the zombie follower upon reaching the end node
            Destroy(gameObject);
            return; // Exit Update to avoid further execution
        }

        if (pathToFollow.Count > currentWaypointIndex)
        {
            // Follow the path by moving towards each waypoint in sequence
            Transform currentWaypoint = pathToFollow[currentWaypointIndex];

            Vector3 dir = currentWaypoint.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            // Rotate to face the direction of movement
            RotateTowardsMovementDirection(dir);

            // If the follower reaches the current waypoint, move to the next waypoint
            if (Vector3.Distance(transform.position, currentWaypoint.position) <= 0.4f)
            {
                currentWaypointIndex++;
            }
        }
    }

    void RotateTowardsMovementDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
