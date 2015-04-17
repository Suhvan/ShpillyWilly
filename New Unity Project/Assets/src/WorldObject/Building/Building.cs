using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Building : WorldObject {

    public float maxBuildProgress;    
    private float currentBuildProgress = 0.0f;    
    public bool production;
    public int incomeChange=1;
    public Vector3 spownPointDelta;

    public bool Pause {get; set;}

    public string productionUnit = "none";
    public string ProductionUnit { get { return productionUnit; } }  

    public float getBuildPercentage()
    {
        return currentBuildProgress / maxBuildProgress;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Pause)
            return;
        base.Update();
        if (production)
            ProcessBuildQueue();
    }

    protected override void onDestroy()
    {
        base.onDestroy();
        player.ChangeIncome(-incomeChange);
    }

    protected void ProcessBuildQueue()
    {
       //if (buildQueue.Count > 0)
        {
            currentBuildProgress += Time.deltaTime * ResourceManager.BuildSpeed;
            if (currentBuildProgress > maxBuildProgress)
            {
                if (Owner)
                {
                    Owner.AddUnit(productionUnit, transform.position + spownPointDelta); 
                }
                currentBuildProgress = 0.0f;
            }
        }
    }
   
  
}
