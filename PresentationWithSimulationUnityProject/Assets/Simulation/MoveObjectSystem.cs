using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

public class MoveObjectSystem : JobComponentSystem
{
    private EntityQuery m_Group;
    public struct MoveObjectJob : IJobChunk
    {
        public ArchetypeChunkComponentType<ObjectTransform> ObjectEntityChunk;

        public float HorizontalDirection;

        public float VerticalDirection;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<ObjectTransform> tObjectEntities = chunk.GetNativeArray(ObjectEntityChunk);

            for (int i = 0; i < chunk.Count; i++)
            {
                float3 tNewDirection =  Vector3.Lerp(tObjectEntities[i].Direction, new float3(HorizontalDirection, 0, VerticalDirection), 0.05f);
                
                float3 tDirection = Math.Abs(HorizontalDirection) > 0.01f || Math.Abs(VerticalDirection) > 0.01f
                    ? tNewDirection
                    : tObjectEntities[i].Direction;

                float tVelocity = Math.Abs(HorizontalDirection) > 0.01f || Math.Abs(VerticalDirection) > 0.01f
                    ? tObjectEntities[i].Velocity
                    : 0; 
                
                tObjectEntities[i] = new ObjectTransform
                {
                    MonoBehaviorID = tObjectEntities[i].MonoBehaviorID,
                    Direction = tDirection,
                    Position = tObjectEntities[i].Position + tObjectEntities[i].Direction * tVelocity,
                    Velocity = tObjectEntities[i].Velocity

                };
            }
        }
    }

    protected override void OnCreateManager()
    {
        var query = new EntityQueryDesc
        { 
            All = new ComponentType[] { typeof(ObjectTransform) }
        };
        m_Group = GetEntityQuery(query);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        MoveObjectJob tMoveObjectJob = new MoveObjectJob
        {
            ObjectEntityChunk = GetArchetypeChunkComponentType<ObjectTransform>(),
            HorizontalDirection = Input.GetAxisRaw("Horizontal")
        };

        return tMoveObjectJob.Schedule(m_Group, dependsOn: inputDeps);
    }
}
