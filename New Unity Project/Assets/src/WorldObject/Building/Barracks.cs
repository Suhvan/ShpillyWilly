using UnityEngine;
using System.Collections;

public class Barracks : Building {

    protected override void Start()
    {
        base.Start();
        actions = new string[] { "Brigand" };
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
