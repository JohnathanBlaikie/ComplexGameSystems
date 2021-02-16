using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


[GenerateAuthoringComponent]
public struct IsTargeted : IComponentData
{
    //public EntitySpawnerScript.HEADING heading;
    public bool Value;
}
