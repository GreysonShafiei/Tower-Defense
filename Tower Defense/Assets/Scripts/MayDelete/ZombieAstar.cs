using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAstar : MonoBehaviour
{
    public Transform target; // The target to reach
    private AStarPathfinding pathfinding; // Reference to the A* pathfinding component
    private List<Waypoint> path; // The path to follow
    private int currentWaypointIndex; // Current waypoint index

    private void Start()
    {
        pathfinding = GetComponent<AStarPathfinding>();
        StartCoroutine(pathfinding.FindPath(transform.position, target.position, this)); // Pass this zombie reference to the pathfinding coroutine
    }

    private void Update()
    {
        if (path != null && path.Count > 0)
        {
            MoveTowardsCurrentWaypoint();
        }
    }

    public void SetPath(List<Waypoint> newPath)
    {
        if (newPath == null || newPath.Count == 0)
        {
            Debug.LogError("Path is empty or null!"); // Add a check in case the path is invalid
            return;
        }

        path = newPath; // Assign the path to the zombie
        currentWaypointIndex = 0; // Start from the first waypoint

        // Ensure the zombie moves toward the first waypoint immediately
        Vector3 firstWaypointPosition = path[0].transform.position;
        transform.position = firstWaypointPosition; // Place the zombie at the first waypoint if desired

        Debug.Log("Zombie received a path with " + path.Count + " waypoints. Moving to first waypoint.");
        MoveTowardsCurrentWaypoint(); // Immediately start moving towards the first waypoint
    }


    private void MoveTowardsCurrentWaypoint()
    {
        if (currentWaypointIndex < path.Count)
        {
            Vector3 targetPosition = path[currentWaypointIndex].transform.position;
            float step = 2f * Time.deltaTime; // Adjust speed here
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            Debug.Log("Moving towards waypoint " + currentWaypointIndex + " at position " + targetPosition);

            // Check if the zombie is close enough to the current waypoint
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f) // Close enough to waypoint
            {
                currentWaypointIndex++; // Move to the next waypoint
                Debug.Log("Reached waypoint " + currentWaypointIndex);
            }
        }
        else
        {
            Debug.Log("Path complete. Zombie reached the target.");
        }
    }

}
