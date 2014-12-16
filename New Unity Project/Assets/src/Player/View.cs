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
        if (player.SelectedObject != null)
        {
            panel.someText.text = player.SelectedObject.name;
        }
        else
        {
            panel.someText.text = "";
        }
	}
}
