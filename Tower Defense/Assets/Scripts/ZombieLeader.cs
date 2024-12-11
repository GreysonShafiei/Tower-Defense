using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieLeader : MonoBehaviour
{
    public float health = 100f;
    public float speed = 10f;
    public float rotationSpeed = 5f; // Speed at which the zombie rotates
    private Transform target;
    private List<Transform> traversedPoints = new List<Transform>(); // Keeps track of visited waypoints
    private Transform selectedBranch;

    private Transform endNode; // Reference to the EndNode
    public List<Transform> pathTaken = new List<Transform>(); // The path that the leader has taken
    public List<ZombieFollower> followers = new List<ZombieFollower>(); // List of followers

    GameManager GameManager;

    void Start()
    {
        // Find the EndNode by its tag
        GameObject endNodeObject = GameObject.FindGameObjectWithTag("End");
        if (endNodeObject != null)
        {
            endNode = endNodeObject.transform; // Assign the EndNode's transform
        }

        target = Waypoints.points[0]; // Starting point
        UpdateTargetBranch(target);   // Initialize the branch for the first waypoint
    }

    private void Update()
    {
        if (endNode == null) return; // Ensure the EndNode has been found

        // Check if the zombie has reached the EndNode
        if (Vector3.Distance(transform.position, endNode.position) <= 0.4f)
        {
            // Destroy the zombie leader upon reaching the end node
            Destroy(gameObject);
            return; // Exit Update to avoid further execution
        }

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
        // Check for branching nodes
        BranchingNodes branchingNode = target.GetComponent<BranchingNodes>();

        if (branchingNode != null)
        {
            // If the branch has already been traversed, pick an alternative
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
                // Use the existing branch if it hasn't been traversed yet
                selectedBranch = branchingNode.selectedBranch;
            }

            // Set the next waypoint as the target
            target = selectedBranch;

            // Immediately update followers' paths before the leader starts moving toward the new target
            pathTaken.Add(target); // Add the next target to the leader's path
            foreach (ZombieFollower follower in followers)
            {
                follower.UpdatePath(pathTaken);
            }
        }
    }

    void UpdateTargetBranch(Transform waypoint)
    {
        // Update the branch to follow based on the next waypoint
        BranchingNodes branchingNode = waypoint.GetComponent<BranchingNodes>();
        if (branchingNode != null)
        {
            selectedBranch = branchingNode.selectedBranch;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage; // Decrease health
        if (health <= 0f)
        {
            Die(); // Call the death method
        }
    }

    // Despawn the zombie
    void Die()
    {
        // Add cash to the GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.cash += 75;
        }

        // Destroy this game object
        Destroy(gameObject);
    }

}
