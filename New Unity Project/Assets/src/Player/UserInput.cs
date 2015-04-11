using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour {
    private Player player;
    private View view;

	// Use this for initialization
	void Start () {
        player = transform.root.GetComponent<Player>();
        view = transform.GetComponent<View>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player.human)
        {
            MoveCamera();
            ZoomCamera();
            MouseActivity();
        }
	}

    private void MouseActivity()
    {
        if (player.buildingMode)
        {
            player.MoveBuilding(FindHitPoint());

            if (Input.GetMouseButtonDown(0))
            {
                player.ConfirmBuilding();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                player.RejectBuilding();                
            }
        }
        else
        { 
            if (Input.GetMouseButtonDown(0)) 
            {
                LeftMouseClick();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                RightMouseClick();
            }
        }
    }

    private void MouseHover()
    {
        if (player.buildingMode)
        {
            Cursor.visible = false;

        }
        else
        {
            Cursor.visible = true;
        }
    }

    private void RightMouseClick()
    {
        if (player.SelectedObject)
        {
            player.SelectedObject.SetSelection(false);
            player.SelectedObject = null;
            view.UpdateSelectedObject();
        }
    }

    private void LeftMouseClick()
    {
        var hitObject = FindHitObject();
        if(hitObject!=null)
        { 
            Vector2 hitPoint = FindHitPoint();
            Debug.Log(System.String.Format("Hit point:{0}, hit object:{1}",hitPoint, hitObject.name));        
            if (!player.SelectedObject && hitObject)
            { 
                WorldObject worldObject = hitObject.GetComponent<WorldObject>();
                if (worldObject)
                {                
                    player.SelectedObject = worldObject;
                    worldObject.SetSelection(true);
                    view.UpdateSelectedObject();
                }
            }
        }
        
    }

    private GameObject FindHitObject()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(1, 0));        
        if(hit.collider!=null)
            return hit.collider.gameObject;
        return null;
    }


    private Vector2 FindHitPoint()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, new Vector2(1, 0));        
        return hit.point;
    }

    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0, 0, 0);

        //horizontal camera movement
        if ((xpos >= 0 && xpos < ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            movement.x -= ResourceManager.ScrollSpeed;
        }
        else if ((xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            movement.x += ResourceManager.ScrollSpeed;
        }        

        //vertical camera movement
        if ((ypos >= 0 && ypos < ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            movement.y -= ResourceManager.ScrollSpeed;
        }
        else if ((ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            movement.y += ResourceManager.ScrollSpeed;
        }

        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;        

        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
        }
    }

    private void ZoomCamera()
    {
        float weelMove = -Input.GetAxis("Mouse ScrollWheel") * ResourceManager.ZoomSpeed;
        
        float origin = Camera.main.orthographicSize;
        float destination = origin+weelMove;        
        if (destination != origin && destination>0)
        {
            Camera.main.orthographicSize = destination;
        }
            
    }
}
