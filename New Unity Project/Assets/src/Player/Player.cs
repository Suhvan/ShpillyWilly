using UnityEngine;
using System.Collections;
using RTS;

public class Player : MonoBehaviour {

    public string username;
    public bool human;
    public WorldObject SelectedObject { get; set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void AddUnit(string unitName, Vector3 spawnPoint, Quaternion rotation)
    {
        Units units = GetComponentInChildren<Units>();
        GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, new Quaternion());
        newUnit.transform.parent = units.transform;
    }
}
