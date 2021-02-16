using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Entities;

public class BasicSystem : ComponentSystem
{
    //Headings are overwritten by OnUpdate's search, fix this asap.

    GameObject targetGo;
    float3 up = new float3(0, 1, 0);
    float3 newRotationVector;
        
    protected override void OnStartRunning()
    {
        targetGo = GameObject.FindObjectOfType<EntitySpawnerScript>().targetGO;
        base.OnStartRunning();
    }
    protected override void OnUpdate()
    {
        //float3 targetPos = new float3(targetGo.transform.position.x, targetGo.transform.position.y, targetGo.transform.position.z);
        //Entities.WithAll<AmTargetted>().ForEach((ref Translation tran, ref Rotation rota) =>
        //{
        //    float3 newRotationVector = tran.Value - targetPos;
        //    rota.Value = quaternion.LookRotationSafe(newRotationVector, up);
        //});
        float3 targetPos = new float3(targetGo.transform.position.x, targetGo.transform.position.y, targetGo.transform.position.z);
        Entities.WithAll<IsTargeted>().ForEach((ref Translation tran, ref Rotation rota, ref IsTargeted entityHeading) =>
        {
            //TODO: Factor out non-targeted headings
            if (entityHeading.Value == true)
            {
                newRotationVector = targetPos - tran.Value - ((tran.Value) - targetPos);
                rota.Value = quaternion.LookRotationSafe(newRotationVector, up);
            }
               // tran.LookAt(targetGo.transform);
        });
    }
}

