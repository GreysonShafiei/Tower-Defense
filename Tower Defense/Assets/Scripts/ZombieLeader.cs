using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieLeader : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 5f; // Speed at which the zombie rotates
    private Transform target;
    private List<Transform> traversedPoints = new List<Transform>(); // Keeps track of visited waypoints
    private Transform selectedBranch;

    public List<Transform> pathTaken = new List<Transform>(); // The path that the leader has taken
    public List<ZombieFollower> followers = new List<ZombieFollower>(); // List of followers

    void Start()
    {
        target = Waypoints.points[0]; // Starting point
        UpdateTargetBranch(target);   // Initialize the branch for the first waypoint
    }

    private void Update()
    {
        // Calculate the direction vector toward the target waypoint
        Vector3 dir = target.position - transform.position;

        // Move toward the target
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // Rotate to face the direction of movement
        RotateTowardsMovementDirection(dir);

        // If the zombie is close to the target waypoint, move to the next waypoint
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            traversedPoints.Add(target); // Mark the current target as traversed
            pathTaken.Add(target); // Add this target to the leader's path

            // Update the followers' path to match the leader's path
            foreach (ZombieFollower follower in followers)
            {
                follower.UpdatePath(pathTaken);
            }

            GetNextWayPoint();           // Move to the next branch or waypoint
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

    void GetNextWayPoint()
    {
        BranchingNodes branchingNode = target.GetComponent<BranchingNodes>();

        if (branchingNode != null)
        {
            if (traversedPoints.Contains(branchingNode.selectedBranch))
            {
                List<Transform> availableBranches = new List<Transform>();
                foreach (Transform branch in branchingNode.potentialBranches)
                {
                    if (!traversedPoints.Contains(branch))
                    {
                        availableBranches.Add(branch);
                    }
                }

                if (availableBranches.Count > 0)
                {
                    selectedBranch = availableBranches[UnityEngine.Random.Range(0, availableBranches.Count)];
                }
                else
                {
                    Destroy(gameObject);
                    return;
                }
            }
            else
            {
                selectedBranch = branchingNode.selectedBranch;
            }

            target = selectedBranch;
        }
    }

    void UpdateTargetBranch(Transform waypoint)
    {
        BranchingNodes branchingNode = waypoint.GetComponent<BranchingNodes>();
        if (branchingNode != null)
        {
            selectedBranch = branchingNode.selectedBranch;
        }
    }
}
