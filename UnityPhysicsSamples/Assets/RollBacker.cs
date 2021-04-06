/*using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using System.Collections.Generic;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class RollBacker : ComponentSystem
{
    //public List<PhysicsWorld> pastWorldInstances = new List<PhysicsWorld>();

    World defaultWorld;
    World predictiveWorld;
    protected override void OnStartRunning()
    {
     //   m_BuildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();

        defaultWorld = World;

        predictiveWorld = new World("predictionWorld", WorldFlags.Simulation);

        var systems = DefaultWorldInitialization.GetAllSystems(WorldSystemFilterFlags.Default);

        DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(predictiveWorld, systems);

        FixedStepSimulationSystemGroup fixGroup = predictiveWorld.GetExistingSystem<FixedStepSimulationSystemGroup>();
        fixGroup.FixedRateManager = new FixedRateUtils.FixedRateSimpleManager(Time.DeltaTime);

        for (int i = 0; i < 5;i++)
        {
            //UpdateOnce();
        }
    }

    protected override void OnUpdate()
    {
      //  Debug.Log("Update ball position " + GetBallPosition());
       // Debug.Log("Tick " + pastWorldInstances.Count + " ball position is" + GetBallPosition());
        //pastWorldInstances.Add(m_BuildPhysicsWorld.PhysicsWorld.Clone());
          
     //   if (pastWorldInstances.Count == 10)
      //  {
           // RollBackTo(5);
       // }
    }

    void UpdateOnce()
    {
        World.Update();
        Debug.Log("Simulated " + GetBallPosition());
    }

    void RollBackTo(int tickNum)
    {
   *//*     Debug.Log("Changing world back to tick " + tickNum);
        //Changes current physicsworld
      //  ChangeWorld(predictiveWorld,defaultWorld);
        
        Debug.Log("BallPostion after world change " + GetBallPosition());
        //Steps the physics until catches up to current tick
       // StepWorldXTimes(pastWorldInstances.Count - tickNum);

        for (int i = 0; i < pastWorldInstances.Count;i++)
        {
            UpdateOnce();
        }*//*
    }

    private SimulationContext SimulationContext;
    void StepWorldXTimes(int stepAmount)
    {
        PhysicsStep stepComponent = PhysicsStep.Default;

        var stepInput = new SimulationStepInput
        {
            World = new PhysicsWorld(),
            TimeStep = Time.DeltaTime,
            NumSolverIterations = stepComponent.SolverIterationCount,
            SolverStabilizationHeuristicSettings = stepComponent.SolverStabilizationHeuristicSettings,
            Gravity = stepComponent.Gravity,
            SynchronizeCollisionWorld = true
        };

        for (int i = 0; i < stepAmount;i++)
        {
            SimulationContext.Reset(stepInput);
            Debug.Log("Ball position in simulation " + GetBallPosition());
            Simulation.StepImmediate(stepInput, ref SimulationContext);
        }
    }

    void ChangeWorld(World worldToCopyTo, World worldToCopyFrom)
    {
        worldToCopyTo.EntityManager.CopyAndReplaceEntitiesFrom(worldToCopyFrom.EntityManager);
        worldToCopyTo.SetTime(new Unity.Core.TimeData(worldToCopyFrom.Time.ElapsedTime, worldToCopyFrom.Time.DeltaTime));
    }

    void ChangePhysicsWorld(PhysicsWorld physicsWorld)
    {

        //m_BuildPhysicsWorld.Update();
    }

    Vector3 GetBallPosition()
    {
        Vector3 pos = Vector3.zero;
        Entities.ForEach((ref Translation position) =>
        {
            pos = new Vector3(position.Value.x,position.Value.y,position.Value.z);
        });

        return pos;
    }
}
*/