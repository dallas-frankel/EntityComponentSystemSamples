using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using System.Collections.Generic;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Events;
using System;

public class ECSRollBacker : ComponentSystem
{
    public static World myWorld;

    public PhysicsVelocity ballVel;
    public Translation ballPos;

    public PhysicsVelocity batVel;
    public Translation batPos;

    protected override void OnStartRunning()
    {
        Debug.Log("Started Running");
        myWorld = World;

        positions = new List<List<float3>>();
        velocities = new List<List<float3>>();
        positions2 = new List<List<float3>>();
        velocities2 = new List<List<float3>>();

        isSimulating = false;
        shouldRollBack = false;
        velocityToAdd = float3.zero;

        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            if (rotation.Value.value.w == 1f)
            {
                ballVel = physicsVelocity;
                ballPos = position;
            }
            else
            {
                batVel = physicsVelocity;
                batPos = position;
            }
        });
    }

    public static bool isSimulating;

    public static bool shouldRollBack = false;
    public static int rollBackInt = 0;
    protected override void OnUpdate()
    {
        if (!isSimulating)
        {
            saveVelocity();
            savePosition();
            if (shouldRollBack)
            {
                shouldRollBack = false;
                setToTick(rollBackInt);
            }
        }
        else
        {
            if (!velocityToAdd.Equals(float3.zero))
            {
                ChangeBallVelocity(velocityToAdd);
            }
            saveVelocity2();
            savePosition2();
        }
    }

    public void ChangeBallVelocity(float3 velocity)
    {
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            Debug.Log("Ball Vel changed to " + velocity);
            physicsVelocity.Linear = velocity;
        });
    }

    public void setToTick(int tick)
    {
        int count = 0;
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Translation position, ref Rotation rotation) =>
        {
            position.Value = positions[tick][count];
            physicsVelocity.Linear = velocities[tick][count];
            count++;
            Debug.Log("Set position to " + position.Value);
            rollBackPosition = new Vector3(position.Value.x, position.Value.y, position.Value.z);
        });

        MonoBehaviourRollBacker.singleton.UpdateXTimes();
    }

    public static List<List<float3>> velocities = new List<List<float3>>();
    public static List<List<float3>> positions = new List<List<float3>>();

    public static float3 velocityToAdd;

    public void saveVelocity()
    {
        List<float3> velocityList = new List<float3>();
        Entities.ForEach((ref PhysicsVelocity physicsVelocity) =>
        {
            velocityList.Add(physicsVelocity.Linear);
        });
        velocities.Add(velocityList);
    }

    public event EventHandler RolledBack;
    public void savePosition()
    {
        List<float3> positionList = new List<float3>();
        Entities.ForEach((ref Translation position) =>
        {
            positionList.Add(position.Value);
            Debug.Log(position.Value + " " + positions.Count);
        });
        positions.Add(positionList);
    }


    public static List<List<float3>> velocities2 = new List<List<float3>>();
    public static List<List<float3>> positions2 = new List<List<float3>>();
    public void saveVelocity2()
    {
        List<float3> velocityList = new List<float3>();
        Entities.ForEach((ref PhysicsVelocity physicsVelocity) =>
        {
            velocityList.Add(physicsVelocity.Linear);
        });
        velocities2.Add(velocityList);
    }

    public void savePosition2()
    {
        List<float3> positionList = new List<float3>();

        Entities.ForEach((ref Translation position) =>
        {
            positionList.Add(position.Value);
            Debug.Log(position.Value + " simed tick " + (positions.Count + positions2.Count - 10));
        });

        positions2.Add(positionList);
    }

    public static Vector3 rollBackPosition;
}
