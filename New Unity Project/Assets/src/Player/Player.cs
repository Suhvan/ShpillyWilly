using UnityEngine;
using System.Collections;
using RTS;

public partial class Player : MonoBehaviour {

    public string username;
    public bool human;
    public WorldObject SelectedObject { get; set; }
    public int money;    
    public Units units;
    public Buildings buildings;
    public BuildSpots buildSpots;
    public float incomeCooldown;
    public int income = 1;    
    
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
        if (!human)
        {
            Think();
        }
	}

    internal void AddUnit(string unitName, Vector3 spawnPoint)
    {        
        GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, new Quaternion());
        newUnit.transform.parent = units.transform;
    }

    public void StartBuilding(string buildingName)
    {        
        if (ResourceManager.GetBuilding(buildingName).GetComponent<Building>().cost <= money && SelectedObject is BuildingSpot)
        {
            Debug.Log(buildingName);
            var pos = SelectedObject.transform.position;
            var delta = ((BuildingSpot)SelectedObject).spownPointDelta;
            Destroy(SelectedObject.gameObject);
            var tmpBuilding = (GameObject)Instantiate(ResourceManager.GetBuilding(buildingName), pos, new Quaternion());
            tmpBuilding.transform.parent = buildings.transform;
            var building = tmpBuilding.GetComponent<Building>();
            building.spownPointDelta = delta;
            income += building.incomeChange;
            money -= building.cost;            
        }
    }

}
