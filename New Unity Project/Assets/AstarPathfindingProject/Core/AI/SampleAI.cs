using UnityEngine;
using System.Collections;
using Pathfinding;

public class SampleAI : MonoBehaviour {

    public Vector3 targetPosition;
    public Path calculatedPath;

    //The AI's speed per second
    public float speed = 0.01f;

    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 0.01f;

    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    public void Start()
    {
        //Get a reference to the Seeker component we added earlier
        Seeker seeker = GetComponent<Seeker>();

        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        seeker.StartPath(transform.position, targetPosition, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
        if (!p.error)
        {
            calculatedPath = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
    }

    public void Update()
    {
        if (calculatedPath == null)
        {
            //We have no path to move after yet
            return;
        }

        if (currentWaypoint >= calculatedPath.vectorPath.Count)
        {
            //Debug.Log("End Of Path Reached");
            return;
        }

        //Direction to the next waypoint
     

        //controller.SimpleMove(dir);
        var vect = Vector3.MoveTowards(transform.position, calculatedPath.vectorPath[currentWaypoint], Time.deltaTime * speed);
        vect.z = -1;
        transform.position = vect;
        
        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance(transform.position, calculatedPath.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }
}
