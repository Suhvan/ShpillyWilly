using UnityEngine;
using System.Collections;
using HUD;

public class View : MonoBehaviour {

    private Player player;
    private BottomPanel panel;
    private TopPanel topPanel;

	// Use this for initialization
	void Start () {
        player = transform.root.GetComponent<Player>();
        panel = FindObjectOfType<BottomPanel>();
        topPanel = FindObjectOfType<TopPanel>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!player.human)
            return; 
        topPanel.Money.text = player.money.ToString();
        if ( player.SelectedObject != null)
        {
            panel.hpPanel.hpText.text = player.SelectedObject.hitPoints.ToString();
            panel.hpPanel.hpSlider.value = player.SelectedObject.HPPercentage;
            if (player.SelectedObject is Building)
            {
                Building building = (Building)player.SelectedObject;
                panel.prodPanel.process.value = building.getBuildPercentage();
            }
        }
        else
        {
            panel.setState(HUD.BottomPanel.Mode.NONE);
        }
	}


    public void UpdateSelectedObject()
    {
        if (player.SelectedObject != null)
        {
            panel.someText.text = player.SelectedObject.objectName;
            panel.hpPanel.hpText.text = player.SelectedObject.hitPoints.ToString();
            if (player.SelectedObject is Building)
            {
                panel.setState(HUD.BottomPanel.Mode.BUILDING);
            }
            else if (player.SelectedObject is BuildingSpot)
            {
                panel.setState(HUD.BottomPanel.Mode.BUILD_SLOT);
            }
            else if (player.SelectedObject is Unit)
            {
                panel.setState(HUD.BottomPanel.Mode.UNIT);
            }
        }
        else
        {
            panel.setState(HUD.BottomPanel.Mode.NONE);
        }
    }
}
