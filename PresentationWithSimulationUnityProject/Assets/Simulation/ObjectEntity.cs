using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct ObjectTransform: IComponentData
{
    public int MonoBehaviorID;

    public float Velocity;
    public float3 Direction;
    public float3 Position;
}

public enum Animation
{
    Idle,
    WalkForward,
    Run,
    Attack,
    Dead
}
public struct ObjectAnimator : IComponentData
{
    public int MonoBehaviorID;

    public Animation Animation;

}
