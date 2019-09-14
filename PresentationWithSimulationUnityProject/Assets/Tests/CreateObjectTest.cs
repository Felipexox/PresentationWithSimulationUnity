using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CreateObjectTest : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ReferenceObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateEntity(m_ReferenceObject);
        }
    }

    public void CreateEntity(GameObject pReference)
    {

        EntityManager tEntityManager = World.Active.EntityManager;

        GameObject tObjectMono = Instantiate(pReference);

        EntityArchetype tArchetype = tEntityManager.CreateArchetype(typeof(ObjectTransform), typeof(ObjectAnimator));

        Entity tEntity = tEntityManager.CreateEntity(tArchetype);

        tEntityManager.SetComponentData(tEntity, new ObjectTransform
        {
            Direction = new float3(0,0,1),
            Position = float3.zero,
            Velocity = 0.06f,
            MonoBehaviorID = tObjectMono.GetInstanceID()
        });

        tEntityManager.SetComponentData(tEntity, new ObjectAnimator
        {
            MonoBehaviorID = tObjectMono.GetInstanceID(),
            Animation = Animation.Idle
        });



        MonobehaviorTransformPresentation tMonoTranformPresentation = World.Active.GetOrCreateSystem< MonobehaviorTransformPresentation >();

        ReferenceTransformMono tReferenceTranformMono = new ReferenceTransformMono
        {
            ObjectID = tObjectMono.GetInstanceID(),
            ObjectTransformReference = tObjectMono.transform
        };

        tMonoTranformPresentation.m_ReferenceMonos.Add(tReferenceTranformMono);


        MonobehaviorAnimationPresentation tMonoAnimatorPresentation = World.Active.GetOrCreateSystem<MonobehaviorAnimationPresentation>();

        ReferenceAnimatorMono tReferenceAnimatorMono = new ReferenceAnimatorMono
        {
            ObjectID = tObjectMono.GetInstanceID(),
            ObjectAnimatorReference = tObjectMono.GetComponent<Animator>()
        };

        tMonoAnimatorPresentation.m_ReferenceMonos.Add(tReferenceAnimatorMono);
    }
}
