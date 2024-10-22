using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wayPointIndex = 0;
    private List<Transform> traversedWaypoints = new List<Transform>();

    private void Start()
    {
        target = Waypoints.points[0];
    }

    private void Update()
    {
        moveTowardsEnd();
    }

    void moveTowardsEnd()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.transform.position) <= 0.4f)
        {
            chooseNextWaypoint();
        }
    }

    void chooseNextWaypoint()
    {
        // Get the Waypoints component from the current target Transform
        Waypoints waypointComponent = target.GetComponent<Waypoints>();
        Boolean alreadyTraversed = false;
        foreach (Waypoints connectedWaypoint in waypointComponent.connectedWaypoints)
        {
            if (traversedWaypoints.Contains(connectedWaypoint.transform))
            {
                alreadyTraversed = true;
                break;
            }
        }

        if (waypointComponent != null && waypointComponent.isBranchPoint && alreadyTraversed == false)
        {
            // Select one of the connected waypoints at random
            target = waypointComponent.connectedWaypoints[UnityEngine.Random.Range(0, waypointComponent.connectedWaypoints.Length)].transform;
        }
        else if (waypointComponent != null && waypointComponent.restrictedNextWaypoints.Length > 0)
        {
            // Only move to restricted next waypoints
            target = waypointComponent.restrictedNextWaypoints[0].transform;
        }
        else
        {
            if (wayPointIndex >= Waypoints.points.Length - 1)
            {
                Destroy(gameObject);
                return;
            }

            wayPointIndex++;
            target = Waypoints.points[wayPointIndex];
        }

        traversedWaypoints.Add(target);
    }
}
