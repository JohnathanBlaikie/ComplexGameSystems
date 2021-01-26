using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class HybridEntity : MonoBehaviour
{
    EntityManager myEntityManager;
    public Mesh myMesh;
    public Material myMaterial;

    public GameObject entityGOPrefab;
    public Entity entityPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //CreateEnt();
        CreateHybridEnt();
    }
    void CreateEnt()
    {
        myEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype myAType = myEntityManager.CreateArchetype(
            typeof(Translation), typeof(Rotation),
            typeof(RenderMesh), typeof(RenderBounds),
            typeof(LocalToWorld));

        Entity myEnt = myEntityManager.CreateEntity(myAType);
        myEntityManager.AddComponentData(myEnt, new Translation { Value = new float3(5f, 0, 4f) });
        myEntityManager.AddSharedComponentData(myEnt, new RenderMesh
        {
            mesh = myMesh,
            material = myMaterial
        });
    }

    void CreateHybridEnt()
    {
        myEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        GameObjectConversionSettings settings = 
            GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(entityGOPrefab, settings);

        Entity myEnt = myEntityManager.Instantiate(entityPrefab);
        myEntityManager.AddComponentData(myEnt, new Translation { Value = new float3(5f, 0, 4f) });


    }
}
