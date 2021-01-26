using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class HelloEntity : MonoBehaviour
{
    EntityManager myEntityManager;
    public Mesh myMesh;
    public Material myMaterial;
    // Start is called before the first frame update
    void Start()
    {
        CreateEnt();

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
}
