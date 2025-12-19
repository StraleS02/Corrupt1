using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    public Transform[] waypoints;
    public Waypoint currentWaypoint;
    public float speed;

    private bool doubleLane = false;
    private bool isTurning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //OnDrawGizmos();
        if(currentWaypoint == null) return;

        if (!isTurning)
        {
            MoveTowards(currentWaypoint.transform.position);

            float distance = Vector3.Distance(transform.position, currentWaypoint.transform.position);
            if (distance < 10f)
            {
                // izaberi sledeći waypoint
                Waypoint next = ChooseNextWaypoint();
                if (next != null)
                {
                    StartCoroutine(TurnTowards(next));
                }
            }
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 offset = new Vector3();
        Vector3 adjustedTarget = new Vector3();

        if (!doubleLane)
        {
            offset = transform.right * 8f;
            adjustedTarget = target + offset;
        }
        else
        {
            int choice = UnityEngine.Random.Range(0, 2);
            float side = (choice == 0) ? -10f : 10f;

            offset = transform.right * side;
            adjustedTarget = target + offset;
        }

        Vector3 dir = (adjustedTarget - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 4f * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    Waypoint ChooseNextWaypoint()
    {
        if (currentWaypoint.nextWaypoints.Length == 0) return null;
        int num = 0;
        HashSet<Waypoint> checkedWaypoints = new HashSet<Waypoint>();

        while (true)
        {
            num++;
            int index = UnityEngine.Random.Range(0, currentWaypoint.nextWaypoints.Length);
            Vector3 wpPos = currentWaypoint.nextWaypoints[index].transform.position;

            Vector3 dirToWp = (currentWaypoint.nextWaypoints[index].transform.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(transform.forward, dirToWp, Vector3.up);
            float distance = Vector3.Distance(transform.position, wpPos);

            checkedWaypoints.Add(currentWaypoint.nextWaypoints[index]);

            if (Mathf.Abs(angle) < 135f && distance > 85f) // znači ispred auta
            {
                if (currentWaypoint.nextWaypoints[index].hasMultipleLanes) doubleLane = true;
                else doubleLane = false;
                return currentWaypoint.nextWaypoints[index];
            }
            if(Mathf.Abs(angle) < 135f && checkedWaypoints.Count == currentWaypoint.nextWaypoints.Length)
            {
                if (currentWaypoint.nextWaypoints[index].hasMultipleLanes) doubleLane = true;
                else doubleLane = false;
                return currentWaypoint.nextWaypoints[index];
            }
            if(num >= 50)
            {
                Debug.LogWarning("Safety break: no valid waypoint found!");
                return null;
            }
        }
    }

    IEnumerator TurnTowards(Waypoint next)
    {
        isTurning = true;

        Vector3 targetPos = next.transform.position;

        // glatko okretanje i prelazak ka sledećem waypointu
        while (Vector3.Distance(transform.position, targetPos) > 10f)
        {
            MoveTowards(targetPos);
            yield return null;
        }

        currentWaypoint = next;
        isTurning = false;
    }
}
