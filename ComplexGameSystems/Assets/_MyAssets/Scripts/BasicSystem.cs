using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Entities;


public class BasicSystem : ComponentSystem
{
    //public List<EntitySpawnerScript> eSSList = new List<EntitySpawnerScript>();

    //EntitySpawnerScript eSS;
    //protected override void OnStartRunning()
    //{
    //    eSSList
    //}

    protected override void OnUpdate()
    {
        //Entities.WithAll<EntitySpawnerScript.HEADING>().ForEach((ref Transform tran, ref EntitySpawnerScript.HEADING entityHeading) =>
        //{
            //if (entityHeading == EntitySpawnerScript.HEADING.TARGETED)
            //{
                //tran.LookAt(EntitySpawnerScript.targetGO);
            //}
        //});

        //switch (eSS.heading)
        //{
            //case EntitySpawnerScript.HEADING.TARGETED:
                ////Entities.ForEach(())//Ask Jon about how to cull the effected entities to just the ones managed by each instance.
                
                //break;
        //}
        //Entities.ForEach((ref Translation tran) =>
        //{
        //    float zPos = math.sin((float)Time.ElapsedTime);
        //    tran.Value = new float3(tran.Value.x, tran.Value.y, zPos);
        //});
    }
}
