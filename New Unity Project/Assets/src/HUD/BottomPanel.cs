using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace HUD
{
    public class BottomPanel : MonoBehaviour
    {
        public enum Mode        
        { 
            UNIT,
            BUILDING,
            BUILD_SLOT,
            NONE
        }
        public Text someText;
        public HPPanel hpPanel;        
        public BuildPanel buildPanel;
        public ProductionPanel prodPanel;


        private Mode state;
        public void setState(Mode m)
        {
            if (state == m)
                return;
            state = m;
            switch (m)
            { 
                case Mode.BUILD_SLOT:
                    hpPanel.gameObject.SetActive(false);
                    buildPanel.gameObject.SetActive(true);
                    prodPanel.gameObject.SetActive(false);
                    break;
                case Mode.BUILDING:
                    hpPanel.gameObject.SetActive(true);
                    buildPanel.gameObject.SetActive(false);
                    prodPanel.gameObject.SetActive(true);
                    break;
                case Mode.UNIT:
                    hpPanel.gameObject.SetActive(true);
                    buildPanel.gameObject.SetActive(false);
                    prodPanel.gameObject.SetActive(false);
                    break;
                case Mode.NONE:
                    hpPanel.gameObject.SetActive(false);
                    buildPanel.gameObject.SetActive(false);
                    prodPanel.gameObject.SetActive(false);
                    break;
            }
        }

             
    }
}
