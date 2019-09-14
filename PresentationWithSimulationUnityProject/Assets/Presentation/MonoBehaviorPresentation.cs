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
        Entities.ForEach((ref ObjectTransform pObjectEntity) =>
        {
            for (int i = 0; i < m_ReferenceMonos.Count; i++)
            {
                if (pObjectEntity.MonoBehaviorID == m_ReferenceMonos[i].ObjectID)
                {
                    m_ReferenceMonos[i].ObjectTransformReference.position = pObjectEntity.Position;
                    m_ReferenceMonos[i].ObjectTransformReference.rotation = Quaternion.LookRotation(pObjectEntity.Direction, Vector3.up);
                }
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
        Entities.ForEach((ref ObjectAnimator pObjectEntity) =>
        {
            for (int i = 0; i < m_ReferenceMonos.Count; i++)
            {
           
                if (pObjectEntity.MonoBehaviorID == m_ReferenceMonos[i].ObjectID )
                {
                    AnimatorClipInfo tCurrentClip =
                        m_ReferenceMonos[i].ObjectAnimatorReference.GetCurrentAnimatorClipInfo(0)[0];

                    if (String.Compare(tCurrentClip.clip.name, pObjectEntity.Animation.ToString(),
                            StringComparison.Ordinal) != 0)
                    {
                        var tAnimatorControllerParams = m_ReferenceMonos[i].ObjectAnimatorReference.parameters;
                        for (var j = 0; j < tAnimatorControllerParams.Length; j++)
                        {
                            m_ReferenceMonos[i].ObjectAnimatorReference.SetBool(tAnimatorControllerParams[j].name, false);
                        }
                        m_ReferenceMonos[i].ObjectAnimatorReference.SetBool(pObjectEntity.Animation.ToString(), true);
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