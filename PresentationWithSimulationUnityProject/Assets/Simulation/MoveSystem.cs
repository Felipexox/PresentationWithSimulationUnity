using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MoveSystem : JobComponentSystem
{
    public EntityQuery m_EntityQuery;
    public struct MoveJob : IJobChunk
    {
        public ArchetypeChunkComponentType<C_MoveComponent> m_MoveComponentChunk;
        public ArchetypeChunkComponentType<Translation> m_TranslationChunk;
       
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<C_MoveComponent> tMoveComponents = chunk.GetNativeArray(m_MoveComponentChunk);
            NativeArray<Translation> tTranlations = chunk.GetNativeArray(m_TranslationChunk);

            for (int i = 0; i < chunk.Count; i++)
            {
                tTranlations[i] = new Translation
                {
                    Value = tTranlations[i].Value + 
                            tMoveComponents[i].Velocity * tMoveComponents[i].Direction
                };
            }
            
        }
    }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        m_EntityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new[] {ComponentType.ReadOnly<C_MoveComponent>(), ComponentType.ReadWrite<Translation>()}
        });
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        MoveJob tMoveJob = new MoveJob
        {
            m_TranslationChunk = GetArchetypeChunkComponentType<Translation>(),
            m_MoveComponentChunk = GetArchetypeChunkComponentType<C_MoveComponent>()
        };

        return tMoveJob.Schedule(m_EntityQuery, inputDeps);
    }
}

public struct C_MoveComponent : IComponentData
{
    public float Velocity;
    public float3 Direction;
}