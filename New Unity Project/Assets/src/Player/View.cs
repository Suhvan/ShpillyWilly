using UnityEngine;
using System.Collections;

public class View : MonoBehaviour {

    private Player player;
    private BottomPanel panel;
	// Use this for initialization
	void Start () {
        player = transform.root.GetComponent<Player>();
        panel = FindObjectOfType<BottomPanel>();
	}
	
	// Update is called once per frame
	void Update () {        
        if (player.SelectedObject != null )
        {
            panel.hpText.text = player.SelectedObject.hitPoints.ToString();
            panel.hpSlider.value = player.SelectedObject.HPPercentage;
            if (player.SelectedObject is Building)
            {
                Building building = (Building)player.SelectedObject;
                panel.process.value = building.getBuildPercentage();
            }
            else
            {
                panel.process.value = 0;
            }
        }
        else
        {
            panel.process.value = 0;
            panel.hpText.text = "";
            panel.hpSlider.value = 0f;
        }
	}


    public void UpdateSelectedObject()
    {
        if (player.SelectedObject != null)
        {            
            panel.someText.text = player.SelectedObject.name;
            panel.hpText.text = player.SelectedObject.hitPoints.ToString();
            panel.action.onClick.RemoveAllListeners();
            if (player.username == player.SelectedObject.Owner.username)
            {
                if (player.SelectedObject is Building)
                {
                    Building building = (Building)player.SelectedObject;
                    panel.action.onClick.AddListener(() =>
                    {
                        building.PerformAction(building.ProductionUnit);
                    });
                }
                else if (player.SelectedObject is Unit)
                {
                    panel.action.onClick.AddListener(() =>
                    {
                        Debug.Log("Hatton!");
                    });
                }
            }
        }
        else
        {
            panel.action.onClick.RemoveAllListeners();
            panel.someText.text = "";
            panel.process.value = 0;
            panel.hpText.text = "";
            panel.hpSlider.value = 0f;
        }
    }
}
