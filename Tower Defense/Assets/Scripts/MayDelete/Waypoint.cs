using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> connectedWaypoints; // List to hold connected waypoints

    public float GCost; // Cost from start node to this node
    public float HCost; // Heuristic cost from this node to target node
    public Waypoint Parent; // Parent node for path retracing

    void Start()
    {
        foreach (Waypoint wp in connectedWaypoints)
        {
            Debug.Log(gameObject.name + " is connected to " + wp.gameObject.name);
        }
    }

    private void Awake()
    {
        connectedWaypoints = new List<Waypoint>();
        GCost = 0f; // Initialize GCost
        HCost = 0f; // Initialize HCost
        Parent = null; // Initialize Parent
    }

    // Method to add a connection to another waypoint
    public void AddConnection(Waypoint waypoint)
    {
        if (!connectedWaypoints.Contains(waypoint))
        {
            connectedWaypoints.Add(waypoint);
            waypoint.AddConnection(this); // Ensure the connection is bidirectional
        }
    }
}
