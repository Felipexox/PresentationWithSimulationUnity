using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[UpdateAfter(typeof(MoveObjectSystem))]
public class MonobehaviorTransformPresentation : ComponentSystem
{    
    public List<ReferenceTransformMono> m_ReferenceMonos;

    protected override void OnCreateManager()
    {
        m_ReferenceMonos = new List<ReferenceTransformMono>();
    }

    protected override void OnUpdate()
    {
        var tMonoComparer = new MonoTransformComparer();
        var tReferenceMono = new ReferenceTransformMono();
      
        
        Entities.ForEach((ref ObjectTransform pObjectEntity) =>
                {  
                    tReferenceMono.ObjectID = pObjectEntity.MonoBehaviorID;
                    int tIndex = m_ReferenceMonos.BinarySearch(tReferenceMono, tMonoComparer);
                    if (tIndex > -1)
                    {
                        m_ReferenceMonos[tIndex].ObjectTransformReference.position = pObjectEntity.Position;
                        m_ReferenceMonos[tIndex].ObjectTransformReference.rotation =
                            Quaternion.LookRotation(pObjectEntity.Direction, Vector3.up);
                    }
                });

    }
}

[UpdateAfter(typeof(MoveObjectSystem))]
public class MonobehaviorAnimationPresentation : ComponentSystem
{
    public List<ReferenceAnimatorMono> m_ReferenceMonos;

    protected override void OnCreateManager()
    {
        m_ReferenceMonos = new List<ReferenceAnimatorMono>();
    }

    protected override void OnUpdate()
    {
        var tMonoComparer = new MonoAnimationComparer();
        
        var tReferenceMono = new ReferenceAnimatorMono();

        Entities.ForEach((ref ObjectAnimator pObjectEntity) =>
        {
            tReferenceMono.ObjectID = pObjectEntity.MonoBehaviorID;
            int tIndex = m_ReferenceMonos.BinarySearch(tReferenceMono, tMonoComparer);
            if (tIndex > -1)
            {
                AnimatorClipInfo tCurrentClip =
                    m_ReferenceMonos[tIndex].ObjectAnimatorReference.GetCurrentAnimatorClipInfo(0)[0];

                if (String.Compare(tCurrentClip.clip.name, pObjectEntity.Animation.ToString(),
                        StringComparison.Ordinal) != 0)
                {
                    var tAnimatorControllerParams = m_ReferenceMonos[tIndex].ObjectAnimatorReference.parameters;
                    for (var j = 0; j < tAnimatorControllerParams.Length; j++)
                    {
                        if(tAnimatorControllerParams[j].type == AnimatorControllerParameterType.Bool)
                            m_ReferenceMonos[tIndex].ObjectAnimatorReference.SetBool(tAnimatorControllerParams[j].name, false);

                        if (String.CompareOrdinal(tAnimatorControllerParams[j].name,
                                pObjectEntity.Animation.ToString()) == 0)
                        {
                            if (tAnimatorControllerParams[j].type == AnimatorControllerParameterType.Bool)
                            {
                                m_ReferenceMonos[tIndex].ObjectAnimatorReference
                                    .SetBool(pObjectEntity.Animation.ToString(), true);
                            }
                            else if (tAnimatorControllerParams[j].type == AnimatorControllerParameterType.Trigger)
                            {
                                m_ReferenceMonos[tIndex].ObjectAnimatorReference
                                    .SetTrigger(pObjectEntity.Animation.ToString());
                            }
                        }
                    }
                    
                }
            }

        });

    }
}

public struct ReferenceTransformMono
{
    public int ObjectID;
    public Transform ObjectTransformReference;
}

public struct ReferenceAnimatorMono
{
    public int ObjectID;
    public Animator ObjectAnimatorReference;
}

public class MonoTransformComparer: IComparer<ReferenceTransformMono>
{
    public int Compare(ReferenceTransformMono x, ReferenceTransformMono y)
    {
        return x.ObjectID - y.ObjectID;
    }
}

public class MonoAnimationComparer: IComparer<ReferenceAnimatorMono>
{
    public int Compare(ReferenceAnimatorMono x, ReferenceAnimatorMono y)
    {
        return x.ObjectID - y.ObjectID;
    }
}

