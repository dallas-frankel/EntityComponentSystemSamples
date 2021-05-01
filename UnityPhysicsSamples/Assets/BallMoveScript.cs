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

/*    float3[] positions;
    float3[] velocites;

    public List<float3[]> positionsList = new List<float3[]>();
    public List<float3[]> velocitesList = new List<float3[]>();
    private BuildPhysicsWorld m_BuildPhysicsWorld;
*//*    private SimulationContext SimulationContext;*/
/*    private NativeArray<float3> Positions;
    private NativeArray<float3> Velocities;*//*

    public static Vector3 ballPosition;

    public static List<Vector3> ballPositions = new List<Vector3>();

    bool shouldSimulateNextFrame;

    public List<PhysicsWorld> worldList = new List<PhysicsWorld>();*/

    protected override void OnUpdate()
    {
      /*  SaveStates();


        positionsList.Add((float3[])positions.Clone());
        velocitesList.Add((float3[])velocites.Clone());


        Debug.Log("---------------");
        Debug.Log("tick " + (positionsList.Count - 1));
        Debug.Log(positions[0]);
        Debug.Log(velocites[0]);

        if (shouldSimulateNextFrame)
        {
            RollBack();
            shouldSimulateNextFrame = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            loadStates(positionsList.Count - 20);
            shouldSimulateNextFrame = true;
        }


        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            ballPosition = new Vector3(position.Value.x,position.Value.y,position.Value.z);
            ballPositions.Add(ballPosition);
        });

        if (positionsList.Count == 200)
        {
            Debug.Log("Yo " + ballPosition.x);
            Debug.Log("Yo " + ballPosition.y);
            Debug.Log("Yo " + ballPosition.z);
        }


        worldList.Add(LocalWorld.Clone());*/
    }

    protected override void OnCreate()
    {
     /*   m_BuildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        LocalWorld = new PhysicsWorld();
        SimulationContext = new SimulationContext();*/
    }
    /*
    private PhysicsWorld LocalWorld;
    private float GhostScale = 0.01f;
    public static List<Vector3> simulatedBallPositions;

    int simPhase;
    public void RollBack()
    {
        Debug.Log("Rolling Back");



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
       // ballPositions.RemoveRange(ballPositions.Count - 20,20);
        int NumSteps = 19;

        for (int i = 0; i < NumSteps; i++)
        {
            // Dispose and reallocate input velocity buffer, if dynamic body count has increased.
            // Dispose previous collision and trigger event streams and allocator new streams.
            SimulationContext.Reset(stepInput);

            new StepLocalWorldJob()
            {
                StepInput = stepInput,
                SimulationContext = SimulationContext,
                jobPositions = Positions,
                jobVelocities = Velocities

            }.Schedule().Complete();

            var positions1 = Positions;
            var velocities1 = Velocities;

            for (int u = 0; u < positions1.Length; u++)
            {
                simulatedBallPositions.Add( new Vector3(positions1[u].x, positions1[u].y, positions1[u].z));
                Debug.Log("Stepping " + (positionsList.Count - 20 + i));
                Debug.Log(positions1[u]);
                Debug.Log(velocities1[u]);
            }

            ballPositions.Add(ballPosition);
        }

        var positions = Positions;
        var ghostScale = GhostScale;
        var numSteps = NumSteps;
        var velocities = Velocities;

        int index = 0;
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            Debug.Log("Setting position to " + positions[index]);
            position.Value = positions[index];
            physicsVelocity.Linear = velocites[index];
            index++;
            simulationEndPosition = new Vector3(position.Value.x,position.Value.y,position.Value.z);
        });
    }

    public static Vector3 simulationEndPosition;

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
                jobVelocities[b] = StepInput.World.DynamicsWorld.MotionVelocities[b].LinearVelocity;
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

    public static Vector3 loadState;

    public void LoadStates(int listIndex)
    {
        int index = 0;
        Debug.Log("Loading " + listIndex);
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            position.Value = positionsList[listIndex][index];
        
            physicsVelocity.Linear = velocitesList[listIndex][index];
            loadState = new Vector3(position.Value.x,position.Value.y,position.Value.z);

            Debug.Log(position.Value);
            Debug.Log(physicsVelocity.Linear);

            index++;
        });
    }

  /*  public void LoadState(int listIndex)
    {
        
    }
*/
   
    protected override void OnStartRunning()
    {
     /*   ballPositions = new List<Vector3>();
        simulatedBallPositions = new List<Vector3>();*/
        int count = 0;
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            if (rotation.Value.value.w == 1f)
            {
                physicsVelocity.Linear.x = 5f;
                
            }
            else
            {
               // physicsVelocity.Linear.y = 5f;
            }
            physicsVelocity.Linear.x = 5f;
            position.Value.x = position.Value.x + 2;
            count++;
        });
  /*      positions = new float3[count];
        velocites = new float3[count];

        if (Positions.IsCreated) Positions.Dispose();
        Positions = new NativeArray<float3>(1, Allocator.Persistent);
        if (Velocities.IsCreated) Velocities.Dispose();
        Velocities = new NativeArray<float3>(1, Allocator.Persistent);*/
    }
}

