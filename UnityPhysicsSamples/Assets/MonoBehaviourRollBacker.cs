using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using System.Collections.Generic;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;

public class MonoBehaviourRollBacker : MonoBehaviour
{
    static bool isSimulating = false;

    World defaultWorld;

    public static MonoBehaviourRollBacker singleton;
    private void Start()
    {
        singleton = this;
        defaultWorld = null;
    }

    bool done;
    private void Update()
    {
        if (ECSRollBacker.positions.Count == 100 && done == false)
        {
            Debug.Log("MonoBehaviour setting Back");
            done = true;
            ECSRollBacker.shouldRollBack = true;
            ECSRollBacker.rollBackInt = 50;
        }
    }

    public void UpdateXTimes()
    {
        int x = 10;
        ECSRollBacker.isSimulating = true;
        for (int i = 0; i < x; i++)
        {
            Debug.Log("Updating");
            if (i == 2)
            {
                Debug.Log("Changed vel");
                ECSRollBacker.velocityToAdd = new float3(0,10,0);
            }
            else
            {
                if (i == 3)
                {
                    ECSRollBacker.velocityToAdd = float3.zero;
                }
            }
            ECSRollBacker.myWorld.Update();
        }
        Debug.Log("Settung sim to true");
        ECSRollBacker.isSimulating = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(ECSRollBacker.rollBackPosition, 0.5f);
        Gizmos.color = Color.blue;

        for (int i = 0; i < ECSRollBacker.positions2.Count;i++)
        {
            Gizmos.DrawWireSphere(ECSRollBacker.positions2[i][0],0.5f);
        }
    }
}

