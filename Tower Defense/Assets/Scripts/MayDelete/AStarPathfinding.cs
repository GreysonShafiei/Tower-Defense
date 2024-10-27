using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public Transform target; // The target (e.g., player) to reach
    private List<Waypoint> waypoints; // List of all waypoints in the scene

    private void Start()
    {
        waypoints = new List<Waypoint>(FindObjectsOfType<Waypoint>()); // Get all waypoint objects in the scene
    }

    public IEnumerator FindPath(Vector3 start, Vector3 target, ZombieAstar zombie)
    {
        Waypoint startNode = GetClosestWaypoint(start);
        Waypoint targetNode = GetClosestWaypoint(target);

        List<Waypoint> openList = new List<Waypoint>();
        HashSet<Waypoint> closedList = new HashSet<Waypoint>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Waypoint currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (GetFCost(openList[i], targetNode) < GetFCost(currentNode, targetNode) ||
                    (GetFCost(openList[i], targetNode) == GetFCost(currentNode, targetNode) && GetHCost(openList[i], targetNode) < GetHCost(currentNode, targetNode)))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                List<Waypoint> path = RetracePath(startNode, targetNode); // Get the path
                zombie.SetPath(path); // Assign the path to the zombie so it can start moving
                yield break;
            }

            foreach (Waypoint neighbor in currentNode.connectedWaypoints)
            {
                if (closedList.Contains(neighbor))
                    continue;

                float newCostToNeighbor = GetGCost(currentNode) + Vector3.Distance(currentNode.transform.position, neighbor.transform.position);

                if (newCostToNeighbor < GetGCost(neighbor) || !openList.Contains(neighbor))
                {
                    SetCosts(neighbor, currentNode, newCostToNeighbor, targetNode);
                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }

            yield return null; // Wait for the next frame
        }
    }

    private void SetCosts(Waypoint neighbor, Waypoint parent, float newCost, Waypoint target)
    {
        neighbor.GetComponent<Waypoint>().GCost = newCost;
        neighbor.GetComponent<Waypoint>().HCost = Vector3.Distance(neighbor.transform.position, target.transform.position);
        neighbor.GetComponent<Waypoint>().Parent = parent; // Set the parent for path retracing
    }

    private float GetGCost(Waypoint waypoint)
    {
        return waypoint.GetComponent<Waypoint>().GCost;
    }

    private float GetHCost(Waypoint waypoint, Waypoint target)
    {
        return Vector3.Distance(waypoint.transform.position, target.transform.position);
    }

    private float GetFCost(Waypoint waypoint, Waypoint target)
    {
        return GetGCost(waypoint) + GetHCost(waypoint, target);
    }

    private Waypoint GetClosestWaypoint(Vector3 position)
    {
        Waypoint closestWaypoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (Waypoint waypoint in waypoints)
        {
            float distance = Vector3.Distance(position, waypoint.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestWaypoint = waypoint;
            }
        }

        return closestWaypoint;
    }

    private List<Waypoint> RetracePath(Waypoint startNode, Waypoint endNode)
    {
        List<Waypoint> path = new List<Waypoint>();
        Waypoint currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent; // Move to the parent waypoint
        }

        path.Reverse(); // Reverse the path to get it from start to end

        return path; // Return the path to be used by the zombie
    }
}
