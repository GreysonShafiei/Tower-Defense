using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 5f; // Speed at which the zombie rotates
    protected Transform target;
    protected List<Transform> traversedPoints = new List<Transform>(); // Keeps track of visited waypoints
    protected Transform selectedBranch;

    private void Start()
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
            GetNextWayPoint();           // Move to the next branch or waypoint
        }
    }

    public void RotateTowardsMovementDirection(Vector3 direction)
    {
        // Check if the direction is valid (we don't want to rotate if the zombie is not moving)
        if (direction != Vector3.zero)
        {
            // Create a target rotation based on the direction vector
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void GetNextWayPoint()
    {
        // Check if the current target is a branching node
        BranchingNodes branchingNode = target.GetComponent<BranchingNodes>();

        if (branchingNode != null)
        {
            // Check if the selected branch has already been traversed
            if (traversedPoints.Contains(branchingNode.selectedBranch))
            {
                // Find an unvisited branch
                List<Transform> availableBranches = new List<Transform>();
                foreach (Transform branch in branchingNode.potentialBranches)
                {
                    if (!traversedPoints.Contains(branch))
                    {
                        availableBranches.Add(branch);
                    }
                }

                // If there are available branches, choose one randomly
                if (availableBranches.Count > 0)
                {
                    selectedBranch = availableBranches[UnityEngine.Random.Range(0, availableBranches.Count)];
                }
                else
                {
                    // All branches traversed, destroy zombie
                    Destroy(gameObject);
                    return;
                }
            }
            else
            {
                // Use the current branch if it hasn't been traversed yet
                selectedBranch = branchingNode.selectedBranch;
            }

            target = selectedBranch; // Move to the next waypoint (branch)
        }
    }

    public void UpdateTargetBranch(Transform waypoint)
    {
        // Update the target branch for the current waypoint
        BranchingNodes branchingNode = waypoint.GetComponent<BranchingNodes>();
        if (branchingNode != null)
        {
            selectedBranch = branchingNode.selectedBranch;
        }
    }
}
