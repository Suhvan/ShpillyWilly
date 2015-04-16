using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public partial class Player
{
    private void Think()
    {
        if(money > 15)
        {
            TryFindFreeSpot();
            if(SelectedObject!=null)
            {
                StartBuilding("Barracks");
            }
        }
    }

    private bool TryFindFreeSpot()
    {
        SelectedObject = GetComponentInChildren<BuildingSpot>();
        return false;
    }
}

