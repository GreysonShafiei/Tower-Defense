using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    public bool isBranchPoint;  // Does this waypoint allow route changes?
    public Waypoints[] connectedWaypoints;  // All possible connected waypoints
    public Waypoints[] restrictedNextWaypoints;  // Restricted next waypoints, if any

    private void Awake()
    {
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++) {
            points[i] = transform.GetChild(i);
        }
    }
}
