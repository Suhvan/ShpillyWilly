using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {
    public string objectName;
    public Texture2D buildImage;
    public int cost, sellValue, hitPoints, maxHitPoints;   
   

    protected bool currentlySelected = false;
    
    protected WorldObject target = null;
    public float searchRange = 1f;
    public float weaponRange = 0.3f;
    public float weaponRechargeTime = 1.0f;
    public int  attack = 1;
    private float currentWeaponCooldown;    
    public bool canAttack = false;
    public bool melee = true;

    public float HPPercentage
    {
        get
        {
            return (float)hitPoints / (float)maxHitPoints;
        }
    }

    protected Player player;
    public Player Owner
    {
        get
        {
            return player;
        }
    }

    protected  bool HasTarget
    {
        get
        {
            return target != null;
        }
    }

    private bool ReadyToFire
    {
        get
        {
            return currentWeaponCooldown <= 0;
        }
    }

    protected virtual void Awake()
    {

    }

	// Use this for initialization
    protected virtual void Start()
    {
        player = transform.root.GetComponentInChildren<Player>();
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {
        if(currentWeaponCooldown >=0 )
        { 
            currentWeaponCooldown -= Time.deltaTime;
        }
        if (canAttack && !HasTarget)
        {
            SearchTarget();
        }
        if (HasTarget)
        { 
            PerformAttack();
        }
	}

    private void SearchTarget()
    {
        Vector2 center = this.transform.position;
        var hits = Physics2D.CircleCastAll(this.transform.position, searchRange, Vector2.zero);
        foreach(RaycastHit2D h in hits)
        {
            WorldObject obj = h.collider.gameObject.GetComponent<WorldObject>();
            if (obj != null)
            {
                if (player.username != obj.Owner.username)
                { 
                    target = obj;
                    return;
                }                
            }
        }
    }

    public void SetSelection(bool selected)
    {
        currentlySelected = selected;
    }    

    public virtual void PerformAction(string actionToPerform)
    {
        //it is up to children with specific actions to determine what to do with each of those actions
    }

    public virtual void MouseClick(GameObject hitObject, Vector2 hitPoint, Player controller)
    {
        //only handle input if currently selected
        if (currentlySelected && hitObject && hitObject.name != "Ground")
        {
            WorldObject worldObject = hitObject.GetComponent<WorldObject>();
            //clicked on another selectable object
            if (worldObject)
            {
                ChangeSelection(worldObject, controller);
                
                if (player && player.human)
                { //this object is controlled by a human player
                    //start attack if object is not owned by the same player and this object can attack, else select
                    if (player.username != worldObject.Owner.username && canAttack) BeginAttack(worldObject);
                        else ChangeSelection(worldObject, controller);
                }
                else ChangeSelection(worldObject, controller);
                
            }
        }
    }
   

    private void ChangeSelection(WorldObject worldObject, Player controller)
    {
        //this should be called by the following line, but there is an outside chance it will not
        SetSelection(false);
        if (controller.SelectedObject) controller.SelectedObject.SetSelection(false);
        controller.SelectedObject = worldObject;
        worldObject.SetSelection(true);
    }

    private bool TargetInRange()
    {
        Vector2 targetLocation = target.transform.position;
        Vector2 direction = targetLocation - (Vector2)transform.position;
        if (direction.sqrMagnitude < weaponRange * weaponRange)
        {
            return true;
        }
        return false;
    }

    protected virtual void BeginAttack(WorldObject target)
    {
        this.target = target;
        if (TargetInRange())
        {            
            PerformAttack();
        }
        else AdjustPosition();
    }

    private void PerformAttack()
    {
        if (!HasTarget)
        {            
            return;
        }
        if (!TargetInRange()) AdjustPosition();        
        else if (ReadyToFire) UseWeaponOnTarget();
    }

    protected virtual void UseWeaponOnTarget()
    {
        currentWeaponCooldown = weaponRechargeTime;
        if (melee)
            target.TakeDamage(attack);
        else
        { 
            //пустить снаряд
        }
    }

    //По умолчанию мы не умеем менять позицию, так что меняем цель
    protected virtual void AdjustPosition()
    {        
        target = null;
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0) Destroy(gameObject);
    }

}
