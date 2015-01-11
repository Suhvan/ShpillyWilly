using UnityEngine;
using System.Collections;

public class Barracks : Building {

    protected override void Start()
    {
        base.Start();
        productionUnit = "Brigand";
    }	

     public override void PerformAction(string actionToPerform)
     {
         base.PerformAction(actionToPerform);
         CreateUnit(actionToPerform);
     }
}
