using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class AnimationObjectSystem : JobComponentSystem
{
    private EntityQuery m_Group;

    public struct AnimateObjectJob : IJobChunk
    {
        public ArchetypeChunkComponentType<ObjectAnimator> ObjectAnimatorChunk;

        public bool isMove;

        public bool isAttack;

        public bool isDead;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<ObjectAnimator> ObjectsAnimator = chunk.GetNativeArray(ObjectAnimatorChunk);

            for (int i = 0; i < chunk.Count; i++)
            {
                Animation tClip = Animation.Idle;

                if (isDead)
                {
                    tClip = Animation.Dead;
                }else if (isAttack)
                {
                    tClip = Animation.PunchTrigger;
                }
                else if (isMove)
                {
                    tClip = Animation.WalkForward;
                }

                ObjectsAnimator[i] = new ObjectAnimator
                {
                    MonoBehaviorID = ObjectsAnimator[i].MonoBehaviorID,
                    Animation = tClip
                };
            }
        }
    }

    protected override void OnCreateManager()
    {
        var query = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(ObjectAnimator) }
        };
        m_Group = GetEntityQuery(query);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        AnimateObjectJob tAnimateObjectJob = new AnimateObjectJob
        {
            ObjectAnimatorChunk = GetArchetypeChunkComponentType<ObjectAnimator>(),
            isAttack = Input.GetKeyDown(KeyCode.X),
            isMove = Input.GetAxisRaw("Horizontal") != 0,
            isDead = false
        };

        return tAnimateObjectJob.Schedule(m_Group, inputDeps);
    }
}
