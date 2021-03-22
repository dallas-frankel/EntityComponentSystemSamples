using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using Unity.Physics.Systems;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class BallMoveScript : ComponentSystem
{

    float3[] positions;
    float3[] velocites;

    public List<float3[]> positionsList = new List<float3[]>();
    public List<float3[]> velocitesList = new List<float3[]>();
    private BuildPhysicsWorld m_BuildPhysicsWorld;
    private SimulationContext SimulationContext;
    private NativeArray<float3> Positions;

    protected override void OnUpdate()
    {
        SaveStates();
        positionsList.Add((float3[])positions.Clone());
        Debug.Log(positions[0] + " Added to " + positionsList.Count);
        velocitesList.Add((float3[])velocites.Clone());
        if (Input.GetKeyDown(KeyCode.R))
        {
            RollBack();
        }

    }

    protected override void OnCreate()
    {
        m_BuildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        LocalWorld = new PhysicsWorld();
        SimulationContext = new SimulationContext();
    }
    private PhysicsWorld LocalWorld;
    private float GhostScale = 0.01f;

    public void RollBack()
    {
        Debug.Log("Rolling Back");

        LoadStates(positionsList.Count - 20);

        ref var world = ref m_BuildPhysicsWorld.PhysicsWorld;
        
        if (LocalWorld.NumBodies > 0)
        {
            LocalWorld.Dispose();
        }
        LocalWorld = world.Clone();

        float timeStep = Time.DeltaTime;

        PhysicsStep stepComponent = PhysicsStep.Default;
        if (HasSingleton<PhysicsStep>())
        {
            stepComponent = GetSingleton<PhysicsStep>();
        }

        var stepInput = new SimulationStepInput
        {
            World = LocalWorld,
            TimeStep = timeStep,
            NumSolverIterations = stepComponent.SolverIterationCount,
            SolverStabilizationHeuristicSettings = stepComponent.SolverStabilizationHeuristicSettings,
            Gravity = stepComponent.Gravity,
            SynchronizeCollisionWorld = true
        };

        int NumSteps = 20;

        for (int i = 0; i < NumSteps; i++)
        {
            // Dispose and reallocate input velocity buffer, if dynamic body count has increased.
            // Dispose previous collision and trigger event streams and allocator new streams.
            SimulationContext.Reset(stepInput);
            Debug.Log("Stepping");
            new StepLocalWorldJob()
            {
                StepInput = stepInput,
                SimulationContext = SimulationContext,
                jobPositions = Positions
            }.Schedule().Complete();


            var positions1 = Positions;

            for (int u = 0; u < positions1.Length; u++)
            {
                Debug.Log(positions1[u].ToString());
            }

        }

        var positions = Positions;
        var ghostScale = GhostScale;
        var numSteps = NumSteps;

        for (int i = 0; i < positions.Length;i++)
        {
          //  Debug.Log(.ToString());
        }

        int index = 0;
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            Debug.Log("Setting position to " + positions[index]);
            position.Value = positions[index];
            index++;
        });

    }


    [BurstCompile]
    struct StepLocalWorldJob : IJob
    {
        public SimulationStepInput StepInput;
        public SimulationContext SimulationContext;

        [NativeDisableContainerSafetyRestriction]
        public NativeArray<float3> jobPositions;
        [NativeDisableContainerSafetyRestriction]
        public NativeArray<float3> jobVelocities;
        public void Execute()
        {
            // Update the trails
            for (int b = 0; b < StepInput.World.DynamicBodies.Length; b++)
            {
                jobPositions[b] = StepInput.World.DynamicBodies[b].WorldFromBody.pos;
 //               jobVelocities[b] = StepInput.World.DynamicBodies[b].
            }
            // Step the local world
            Simulation.StepImmediate(StepInput, ref SimulationContext);
        }
    }

    public void SaveStates()
    {
        int index = 0;
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            positions[index] = position.Value;
            velocites[index] = physicsVelocity.Linear;
            index++;
        });
    }

    public void LoadStates(int listIndex)
    {
        int index = 0;
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
  
            position.Value = positionsList[listIndex][index];
            Debug.Log(position.Value + " for " + listIndex);
            Debug.Log("List Value is " + positionsList[listIndex][index]);
            physicsVelocity.Linear = velocitesList[listIndex][index];
            index++;
        });
    }

    protected override void OnStartRunning()
    {
        int count = 0;
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            physicsVelocity.Linear.x = 5f;
            position.Value.x = position.Value.x + 2;
            count++;
        });
        positions = new float3[count];
        velocites = new float3[count];

        if (Positions.IsCreated) Positions.Dispose();
        Positions = new NativeArray<float3>(1, Allocator.Persistent);
    }
}
