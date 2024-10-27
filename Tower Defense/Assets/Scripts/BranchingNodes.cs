using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchingNodes : MonoBehaviour
{
    public bool branching; // Does the node have branches?
    public Transform[] potentialBranchesInspector; // Set in the Inspector
    public Transform[] potentialBranches; // Used by zombies to select branches
    public Transform selectedBranch; // Selected branch for this specific waypoint

    private void Start()
    {
        // Initialize branches at the start
        UpdateBranches();
    }

    private void Update()
    {
        // Continuously update branches in case something changes during gameplay
        UpdateBranches();
    }

    // This method updates the branch selection each frame or when called
    public void UpdateBranches()
    {
        // Ensure potentialBranches is synced with the Inspector array
        potentialBranches = potentialBranchesInspector;

        // Randomly choose a branch if the node supports branching
        if (potentialBranches.Length > 1 && branching)
        {
            selectedBranch = potentialBranches[UnityEngine.Random.Range(0, potentialBranches.Length)];
        }
        else
        {
            selectedBranch = potentialBranches.Length > 0 ? potentialBranches[0] : null;
        }

        // Optionally, log this to see branch updates in the console
        Debug.Log("Updated branches for waypoint: " + gameObject.name + " - Selected branch: " + selectedBranch?.name);
    }
}
