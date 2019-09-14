using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class CreateObjectTest : MonoBehaviour
{
    [System.Serializable]
    public class ProjectileRender
    {
        public Mesh m_Mesh;
        public Material m_Material;
    }
    
    [SerializeField]
    private GameObject m_ReferenceObject;

    [SerializeField] private ProjectileRender m_ProjectitleRenderer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateProjectile();
        }
    }

    public void CreateProjectile()
    {
        EntityManager tEntityManager = World.Active.EntityManager;
        
        Entity tEntity = tEntityManager.CreateEntity();
        
        CreateProjectileRender(tEntity);
        
        CreateProjectileComponents(tEntity);
    }

    public void CreateProjectileRender(Entity pEntity)
    {
        EntityManager tEntityManager = World.Active.EntityManager;

        tEntityManager.AddComponent(pEntity, typeof(WorldRenderBounds));
        tEntityManager.AddComponent(pEntity, typeof(RenderMesh));
        tEntityManager.AddComponent(pEntity, typeof(ChunkWorldRenderBounds));
        tEntityManager.AddComponent(pEntity, typeof(PerInstanceCullingTag));
        tEntityManager.AddComponent(pEntity, typeof(LocalToWorld));

        tEntityManager.SetComponentData(pEntity, new WorldRenderBounds());

        tEntityManager.SetComponentData(pEntity, new ChunkWorldRenderBounds());

        tEntityManager.SetComponentData(pEntity, new LocalToWorld());

        tEntityManager.SetSharedComponentData(pEntity, new RenderMesh
        {
            material = m_ProjectitleRenderer.m_Material,
            mesh = m_ProjectitleRenderer.m_Mesh
        });



    }

    public void CreateProjectileComponents(Entity pEntity)
    {
        EntityManager tEntityManager = World.Active.EntityManager;
        
        tEntityManager.AddComponent(pEntity, typeof(Translation));
        tEntityManager.AddComponent(pEntity, typeof(Rotation));
        
        tEntityManager.AddComponent(pEntity, typeof(C_MoveComponent));
        
        tEntityManager.SetComponentData(pEntity, new Translation
        {
            Value = new float3(-4, 5, 0)
        });
        
        tEntityManager.SetComponentData(pEntity, new Rotation
        {
            Value = quaternion.identity
        });
        tEntityManager.SetComponentData(pEntity, new C_MoveComponent
        {
            Direction = new float3(1, 0, 0),
            Velocity = 0.2f
        });
        
    }
    
    public void CreateCharacter(GameObject pReference)
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

        tMonoTranformPresentation.m_ReferenceMonos.Sort(new MonoTransformComparer());
        
        MonobehaviorAnimationPresentation tMonoAnimatorPresentation = World.Active.GetOrCreateSystem<MonobehaviorAnimationPresentation>();

        ReferenceAnimatorMono tReferenceAnimatorMono = new ReferenceAnimatorMono
        {
            ObjectID = tObjectMono.GetInstanceID(),
            ObjectAnimatorReference = tObjectMono.GetComponent<Animator>()
        };

        tMonoAnimatorPresentation.m_ReferenceMonos.Add(tReferenceAnimatorMono);

        tMonoAnimatorPresentation.m_ReferenceMonos.Sort(new MonoAnimationComparer());

    }
}
