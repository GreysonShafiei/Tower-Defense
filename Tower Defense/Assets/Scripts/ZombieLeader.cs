using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieLeader : Zombie // Inherits from the base Zombie class
{
    public List<ZombieFollower> followers; // List of followers (can be used if needed)

    private void Start()
    {
        // Initialize the target as the first waypoint in the path
        target = Waypoints.points[0];  // Assuming Waypoints.points holds the waypoint path
        UpdateTargetBranch(target);    // Initialize the branch for the first waypoint
    }

    private void Update()
    {
        // Move towards the current target
        MoveTowardsTarget();

        // Check if the zombie reached the target waypoint
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            traversedPoints.Add(target); // Mark the current target as traversed
            GetNextWayPoint();           // Move to the next branch or waypoint
        }
    }

    private void MoveTowardsTarget()
    {
        // Calculate the direction vector toward the target waypoint
        Vector3 dir = target.position - transform.position;

        // Move toward the target
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Rotate to face the movement direction
        RotateTowardsMovementDirection(dir);  // Pass the 'dir' vector to the method
    }
}
