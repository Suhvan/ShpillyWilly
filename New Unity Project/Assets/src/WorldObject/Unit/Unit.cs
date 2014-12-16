using UnityEngine;
using System.Collections;

public class Unit : WorldObject
{
    public float moveSpeed;
    protected bool moving;

    private Vector3 destination;
    

    protected override void Awake()
    {

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {     
        base.Update();
        //if (rotating) TurnToTarget();
        if (moving) MakeMove();
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
        
        this.destination =  destination;        
        //targetRotation = Quaternion.LookRotation(destination - transform.position);
        //rotating = true;
        moving = true;
    }

    private void MakeMove()
    {        
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);

        if (transform.position == destination)
        {            
            moving = false;
        }
    }
}
