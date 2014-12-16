using UnityEngine;
using System.Collections;

public class Barracks : Building {

    protected override void Start()
    {
        base.Start();
        action = "Brigand";
    }
	

     public override void PerformAction(string actionToPerform)
     {
         base.PerformAction(actionToPerform);
         CreateUnit(actionToPerform);
     }
}
