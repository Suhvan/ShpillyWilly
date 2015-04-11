using UnityEngine;
using System.Collections;
using RTS;
using Pathfinding;
public class Unit : WorldObject
{


    public float nextWaypointDistance = 0.1f;
    public float moveSpeed;
    private Seeker seeker;
    public Path calculatedPath;
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;

 
    protected bool m_moving;
    protected bool Moving
    {
        set
        {
            m_moving = value;
            if (value)
            {
                animator.SetInteger("State", (int)STATE.WALK);
            }
            else
            {
                adjusting = false;
                animator.SetInteger("State", (int)STATE.IDLE);
            }
        }
        get { return m_moving; }
    }
    private Animator animator;
    enum STATE
    {
        IDLE = 0,
        WALK = 1,
    }
    private Vector3 m_destination;

    private Vector3 Destination
    
    {
        get { return m_destination; }
        set 
        {
            m_destination = value;            
            var delta = m_destination - transform.position;            
            animator.SetBool("Left",delta.x <= 0);          
        }
    }
    

    protected override void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
        seeker = GetComponent<Seeker>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {     
        base.Update();
        if (Moving)
        {
            MakeMove();
        }
        else if(!HasTarget)
        {
            GoToEnemyBase();
        }
    }

    private void GoToEnemyBase()
    {
        GameObject enemy = ResourceManager.GetEnemyPlayerObject(Owner);
        MainBase enemyBase = enemy.transform.GetComponentInChildren<MainBase>();
        if (enemyBase != null)
            StartMove(enemyBase.transform.position);
        else
        {
            WorldObject anyObject = enemy.transform.GetComponentInChildren<WorldObject>();
            if(anyObject!=null)
            StartMove(anyObject.transform.position);
        }
    }

    bool calledNewPath = false;
    public void StartMove(Vector3 destination)
    { 
        if(!calledNewPath)
        { 
            seeker.StartPath(transform.position, destination, OnPathComplete);
            Destination = destination;        
            //targetRotation = Quaternion.LookRotation(destination - transform.position);
            //rotating = true;
            Moving = true;
            calledNewPath = true;
        }
    }

    public void OnPathComplete(Path p)
    {
        calledNewPath = false;
        if (!p.error)
        {
            calculatedPath = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
        else
        {
            Debug.LogError("Path error: " + p.error);
        }
    }

    private void MakeMove()
    {
        if (calculatedPath == null)
        {
            //We have no path to move after yet
            Moving = false;
            return;
        }

        if (currentWaypoint >= calculatedPath.vectorPath.Count)
        {
            //Debug.Log("End Of Path Reached");
            Moving = false;
            return;
        }

        var vect = Vector3.MoveTowards(transform.position, calculatedPath.vectorPath[currentWaypoint], Time.deltaTime * moveSpeed);
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

    protected override void UseWeaponOnTarget()
    {
        base.UseWeaponOnTarget();
        animator.SetTrigger("Attack");
    }
    bool adjusting = false;

    protected override void AdjustPosition()
    {        
        if(!adjusting)
        { 
            Vector3 attackPosition = FindNearestAttackPosition();
            StartMove(attackPosition);
            adjusting = true;
        }
    }

    private Vector3 FindNearestAttackPosition()
    {
        Vector3 targetLocation = target.transform.position;
        Vector3 direction = targetLocation - transform.position;
        float targetDistance = direction.magnitude;
        float distanceToTravel = targetDistance - (0.9f * weaponRange);
        return Vector3.Lerp(transform.position, targetLocation, distanceToTravel / targetDistance);
    }

  
}
