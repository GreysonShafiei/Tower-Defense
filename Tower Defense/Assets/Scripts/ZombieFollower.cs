using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFollower : Zombie
{
    private ZombieLeader leader;
    private List<Transform> leaderWaypoints;
    private int currentWaypointIndex = 0;

    public void SetLeader(ZombieLeader newLeader)
    {
        leader = newLeader; // Set the leader for the follower
        leaderWaypoints = newLeader.GetTraversedWaypoints(); // Get the leader's waypoints
        if (leaderWaypoints.Count > 0)
        {
            target = leaderWaypoints[0]; // Set the first waypoint for the follower
        }
    }

    private void Update()
    {
        if (leader != null && leaderWaypoints != null && currentWaypointIndex < leaderWaypoints.Count)
        {
            // Move towards the current waypoint in the leader's traversed waypoints
            MoveTowardsTarget();

            // Check if the follower reached the current waypoint
            if (Vector3.Distance(transform.position, target.position) <= 0.4f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex < leaderWaypoints.Count)
                {
                    target = leaderWaypoints[currentWaypointIndex]; // Move to the next waypoint in the leader's path
                }
            }
        }
    }

    private void MoveTowardsTarget()
    {
        // Calculate the direction vector toward the target waypoint
        Vector3 dir = target.position - transform.position;

        // Move toward the target
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Rotate to face the movement direction
        RotateTowardsMovementDirection(dir);
    }
}
