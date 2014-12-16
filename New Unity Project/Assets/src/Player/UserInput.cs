using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour {
    private Player player;

	// Use this for initialization
	void Start () {
        player = transform.root.GetComponent<Player>();

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
        if (Input.GetMouseButtonDown(0)) LeftMouseClick();
        else if (Input.GetMouseButtonDown(1)) RightMouseClick();
    }

    private void RightMouseClick()
    {
        if (player.SelectedObject)
        {
            player.SelectedObject.SetSelection(false);
            player.SelectedObject = null;
        }
    }

    private void LeftMouseClick()
    {
        var hitObject = FindHitObject();
        Vector2 hitPoint = FindHitPoint();
        Debug.Log(System.String.Format("Hit point:{0}, hit object:{1}",hitPoint, hitObject.name));
        if (player.SelectedObject) player.SelectedObject.MouseClick(hitObject, hitPoint, player);
        else if (hitObject && hitObject.name != "Ground")
        { 
            WorldObject worldObject = hitObject.GetComponent<WorldObject>();
            if (worldObject)
            {
                //we already know the player has no selected object
                player.SelectedObject = worldObject;
                worldObject.SetSelection(true);
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
        if ((xpos >= 0 && xpos < ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x -= ResourceManager.ScrollSpeed;
        }
        else if ((xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth)||Input.GetKey(KeyCode.RightArrow))
        {
            movement.x += ResourceManager.ScrollSpeed;
        }        

        //vertical camera movement
        if ((ypos >= 0 && ypos < ResourceManager.ScrollWidth) || Input.GetKey(KeyCode.DownArrow))
        {
            movement.y -= ResourceManager.ScrollSpeed;
        }
        else if ((ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth)||Input.GetKey(KeyCode.UpArrow))
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
