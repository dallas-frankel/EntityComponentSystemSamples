/*using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using System.Collections.Generic;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class RollBacker2 : ComponentSystem
{
    static World simulationWorld;

    static List<float3> positions = new List<float3>();
    static List<float3> velocites = new List<float3>();

    static bool isSimulating = false;

    protected override void OnStartRunning()
    {
        if (isSimulating){ return;}

        simulationWorld = new World("lockStepWorld", WorldFlags.Simulation);

        var systems = DefaultWorldInitialization.GetAllSystems(WorldSystemFilterFlags.Default);
        DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(simulationWorld, systems);

        FixedStepSimulationSystemGroup fixGroup = simulationWorld.GetExistingSystem<FixedStepSimulationSystemGroup>();
        fixGroup.FixedRateManager = new FixedRateUtils.FixedRateSimpleManager(Time.DeltaTime);
    }

    protected override void OnUpdate()
    {
        if (isSimulating)
        {
            Debug.Log("Simulating " + GetBallPosition());
        }
        else
        {

            Debug.Log("Normal " + positions.Count + " " + GetBallPosition());

            AddBallStateToLists();

            if (positions.Count == 10)
            {
                RollBackTo(5);
            }
        }
    }

    void RollBackTo(int tickToRollBackTo)
    {
        Debug.Log("Rolling back to " + tickToRollBackTo);

        SetBallPosAndVelToTick(tickToRollBackTo);

        simulationWorld.EntityManager.DestroyAndResetAllEntities();
        simulationWorld.EntityManager.CopyAndReplaceEntitiesFrom(World.EntityManager);
        simulationWorld.SetTime(new Unity.Core.TimeData(World.Time.ElapsedTime, World.Time.DeltaTime));


        SimulateTicks(positions.Count - tickToRollBackTo - 1);

        World.EntityManager.DestroyAndResetAllEntities();
        World.EntityManager.CopyAndReplaceEntitiesFrom(simulationWorld.EntityManager);
    }

    void SimulateTicks(int tickNumber)
    {
        isSimulating = true;

        Debug.Log("Simulating the next " + tickNumber + " ticks");
        for (int i = 0; i < tickNumber; i++)
        {
            simulationWorld.Update();
        }
        isSimulating = false;
    }

    public void SetBallPosAndVelToTick(int tick)
    {
        float3 ballPosition = positions[tick];
        float3 ballVelocity = velocites[tick];

        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            position.Value = ballPosition;
            physicsVelocity.Linear = ballVelocity;
        });
    }

    void AddBallStateToLists()
    {
        positions.Add(GetBallPosition());
        velocites.Add(GetBallVelocity());
    }
    
    float3 GetBallPosition()
    {
        float3 pos = float3.zero;
        Entities.ForEach((ref Translation position) =>
        {
            pos = position.Value;
        });
        return pos;
    }

    float3 GetBallVelocity()
    {
        float3 vel = float3.zero;
        
        Entities.ForEach((ref PhysicsVelocity velocity) =>
        {
            vel = velocity.Linear;
        });
        return vel;
    }
}
*/