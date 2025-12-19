using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    private List<GameObject> cars = new List<GameObject>();
    
    private List<GameObject> waypoints = new List<GameObject>();
    private List<GameObject> nearby = new List<GameObject>();

    public static bool sceneChanged = false;

    private int maxCars = 10;
    private float despawnDistance = 500f,
                  spawnRadius = 300f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject npcCars = GameObject.Find("NPCcars");

        foreach (Transform child in npcCars.transform)
        {
            cars.Add(child.gameObject);
        }

        GameObject waypointsParent = GameObject.Find("WayPoints");
        foreach (Transform child in waypointsParent.transform)
        {
            waypoints.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject npcCars = GameObject.Find("NPCcars");
        if (npcCars == null) return;

        if (sceneChanged)
        {
            cars.Clear();
            waypoints.Clear();
            nearby.Clear();

            foreach (Transform child in npcCars.transform)
            {
                cars.Add(child.gameObject);
            }

            GameObject waypointsParent = GameObject.Find("WayPoints");
            foreach (Transform child in waypointsParent.transform)
            {
                waypoints.Add(child.gameObject);
            }

            sceneChanged = false;
        }

        // despawn auta koji su predaleko
        for (int i = CarEnterTrigger.activeCars.Count - 1; i >= 0; i--)
        {
            if (CarEnterTrigger.activeCars[i] == null) // uništen u promeni scene
            {
                CarEnterTrigger.activeCars.RemoveAt(i);
                continue;
            }

            if (Vector3.Distance(transform.position, CarEnterTrigger.activeCars[i].transform.position) > despawnDistance)
            {
                Destroy(CarEnterTrigger.activeCars[i]);
                CarEnterTrigger.activeCars.RemoveAt(i);
            }
        }

        // spawn novih auta ako ih nema dovoljno
        while (CarEnterTrigger.activeCars.Count < maxCars)
        {
            SpawnCar();
        }
    }

    void SpawnCar()
    {


        nearby.Clear();
        foreach (var wp in waypoints)
        {
            if (Vector3.Distance(transform.position, wp.transform.position) >= spawnRadius && Vector3.Distance(transform.position, wp.transform.position) <= despawnDistance)
            {
                nearby.Add(wp);
            }
        }

        if (nearby.Count == 0)
        {
            Debug.LogWarning("Nema waypoinata u spawn radijusu!");
            return;
        }

        GameObject chosenWaypoint = nearby[Random.Range(0, nearby.Count)];

        Waypoint wpScript = chosenWaypoint.GetComponent<Waypoint>();

        Vector3 spawnPos = chosenWaypoint.transform.position;

        // nasumični prefab
        GameObject prefab = cars[Random.Range(0, cars.Count)];
        GameObject car = Instantiate(prefab, spawnPos, Quaternion.identity);
        car.SetActive(true);

        CarAI carScript = car.GetComponent<CarAI>();

        if (wpScript != null && wpScript.nextWaypoints.Length > 0)
        {
            Waypoint nextWp = wpScript.nextWaypoints[Random.Range(0, wpScript.nextWaypoints.Length)];
            carScript.currentWaypoint = nextWp;  // auto dobija sledeći cilj
        }
        else
        {
            Debug.LogWarning("Chosen waypoint nema definisane nextWaypoints!");
        }

        CarEnterTrigger.activeCars.Add(car);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 400f);
    }

}
