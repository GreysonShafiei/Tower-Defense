using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 10f;

    private Transform target;
    private int wayPointIndex = 0;

    private void Start()
    {
        target = Waypoints.points[0];
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            chooseNextWaypoint();
        }
    }

    void chooseNextWaypoint()
    {
        if (wayPointIndex >= Waypoints.points.Length - 1) {
            Destroy(gameObject);
            return;
        }

        wayPointIndex++;
        target = Waypoints.points[wayPointIndex];
    }
}
