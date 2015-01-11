using UnityEngine;
using System.Collections;
using RTS;
public class Unit : WorldObject
{

    enum STATE 
    {
        IDLE = 0,
        WALK = 1,
    }
    public float moveSpeed;
    protected bool m_moving;
    protected bool Moving
    {
        set
        {
            m_moving = value;
            if (value)
                animator.SetInteger("State", (int)STATE.WALK);
            else
                animator.SetInteger("State", (int)STATE.IDLE);
        }
        get { return m_moving; }
    }
    private Animator animator;

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

    public override void MouseClick(GameObject hitObject, Vector2 hitPoint, Player controller)
    {
        base.MouseClick(hitObject, hitPoint, controller);
        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            if (hitObject.name == "Ground")
            {
                float x = hitPoint.x;
                //makes sure that the unit stays on top of the surface it is on
                float y = hitPoint.y ;
                float z = player.SelectedObject.transform.position.z;
                StartMove(new Vector3(x, y, z));
            }
        }
    }

    public void StartMove(Vector3 destination)
    {

        Destination = destination;        
        //targetRotation = Quaternion.LookRotation(destination - transform.position);
        //rotating = true;
        Moving = true;
    }

    private void MakeMove()
    {        
        transform.position = Vector3.MoveTowards(transform.position, Destination, Time.deltaTime * moveSpeed);
        
        if (transform.position == Destination)
        {
            Moving = false;            
        }
    }

    protected override void UseWeaponOnTarget()
    {
        base.UseWeaponOnTarget();
        animator.SetTrigger("Attack");
    }

    protected override void AdjustPosition()
    {        
        Vector3 attackPosition = FindNearestAttackPosition();
        StartMove(attackPosition);
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
