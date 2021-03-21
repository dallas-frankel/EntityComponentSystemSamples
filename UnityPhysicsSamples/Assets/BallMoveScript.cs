
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class BallMoveScript : ComponentSystem
{

    float3[] positions;
    float3[] velocites;
    
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveStates();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadStates();
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

    public void LoadStates()
    {
        int index = 0;
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            position.Value = positions[index];
            physicsVelocity.Linear = velocites[index];
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
    }
}
