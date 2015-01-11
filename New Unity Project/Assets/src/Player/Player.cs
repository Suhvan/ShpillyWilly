using UnityEngine;
using System.Collections;
using RTS;

public class Player : MonoBehaviour {

    public string username;
    public bool human;
    public WorldObject SelectedObject { get; set; }
    public int money;    
    public Units units;
    public Buildings buildings;
    public float incomeCooldown;
    private int income = 5;

    public bool buildingMode = false;
    public GameObject tmpBuilding;
    
    public void ChangeIncome(int val)
    {
        income += val;
    } 
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        incomeCooldown += Time.deltaTime;
        if (incomeCooldown > ResourceManager.IncomeCooldown)
        {
            incomeCooldown = 0;
            money += income;
        }
	}

    internal void AddUnit(string unitName, Vector3 spawnPoint)
    {        
        GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, new Quaternion());
        newUnit.transform.parent = units.transform;
    }

    public void StartBuilding(string buildingName)
    {        
        if (ResourceManager.GetBuilding(buildingName).GetComponent<Building>().cost <= money)
        {           
            buildingMode = true;
            Debug.Log(buildingName);
            tmpBuilding = (GameObject)Instantiate(ResourceManager.GetBuilding(buildingName), Vector3.zero, new Quaternion());
            tmpBuilding.transform.parent = buildings.transform;
            var building = tmpBuilding.GetComponent<Building>();
            building.Pause = true;
        }
    }

    public void MoveBuilding(Vector3 position)    
    {
        position.z = -3;
        tmpBuilding.transform.position = position;
    }

    public void ConfirmBuilding()
    {
        buildingMode = false;
        var building = tmpBuilding.GetComponent<Building>();
        income += building.incomeChange;
        money -= building.cost;
        building.Pause = false;
    }

    public void RejectBuilding()
    {
        buildingMode = false;
        Destroy(tmpBuilding);
    }
}
