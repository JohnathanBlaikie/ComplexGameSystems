using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using System;


public class EntitySpawnerScript : MonoBehaviour
{
    public bool isEnabled = false;


    EntityManager entMan;
    public Mesh mMesh;
    public Material mMat;


    [Header("Spawn Parameters")]
    public GameObject entGOPref;
    public GameObject target;
    public Entity entPref;

    //[HideInInspector]
    public float radius, length, width, maxEntity, spawnRate;
    public Vector3 scale;

    public DynamicInspector dInspector;

    public enum SHAPE { NONE, POINT, SQUARE, CUBE, CIRCLE, SPHERE }
    public SHAPE spawnShape;

    public enum HEADING { NEUTRAL, TARGETED, OUTWARD, INWARD}
    public HEADING heading;

    private void OnDrawGizmos()
    {
        switch (spawnShape)
        {
            case SHAPE.NONE:
                break;
            case SHAPE.POINT:
                Gizmos.DrawWireSphere(transform.position, 1);
                break;
            case SHAPE.SQUARE:
                Gizmos.DrawWireCube(transform.position, scale);
                break;
            case SHAPE.CUBE:
                Gizmos.DrawWireCube(transform.position, scale);
                break;
            case SHAPE.CIRCLE:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
            case SHAPE.SPHERE:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
        }

    }

    public void SpawnPreview(EntitySpawnerScript.SHAPE shape)
    {


    }

    public void CreateHybridEnt()
    {
        entMan = World.DefaultGameObjectInjectionWorld.EntityManager;

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        entPref = GameObjectConversionUtility.ConvertGameObjectHierarchy(entGOPref, settings);
        for (int i = 0; i < maxEntity; i++)
        {
            Entity myEnt = entMan.Instantiate(entPref);
            //entMan.AddComponentData(myEnt, new Translation { Value = new float3() });
            if (spawnShape == SHAPE.CUBE || spawnShape == SHAPE.SQUARE)
            {
                entMan.AddComponentData(myEnt, new Translation { Value = 
                    (new Vector3(UnityEngine.Random.Range(-scale.x, scale.x), 
                    UnityEngine.Random.Range(-scale.y, scale.y), 
                    UnityEngine.Random.Range(-scale.z, scale.z)) 
                    + gameObject.transform.position) });
            }
            else if (spawnShape == SHAPE.SPHERE || spawnShape == SHAPE.CIRCLE)
            {
                //entMan.AddComponentData(myEnt, new Translation { Value = new float3() });
                entMan.AddComponentData(myEnt, new Translation { Value = 
                    (new Vector3(UnityEngine.Random.Range(-radius, radius), 
                    UnityEngine.Random.Range(-radius, radius), 
                    UnityEngine.Random.Range(-radius, radius)) 
                    + gameObject.transform.position)});
            }
            else if (spawnShape == SHAPE.POINT)
            {
                entMan.AddComponentData(myEnt, new Translation { Value = gameObject.transform.position});
    }
            else { }

            switch (heading)
            {
                case HEADING.NEUTRAL:

                    break;
                case HEADING.TARGETED:
                    //This will spawn the entities ("entGOPref") facing a target object ("target") 


                    //entMan.AddComponentData(myEnt, new Rotation { Value = 
                    //((entGOPref.GetComponent<Rigidbody>().transform.LookAt(target.transform))) });
                    //entMan.AddComponentData(myent, entGOPref.GetComponent<Rigidbody>().transform.LookAt(target.transform));
                    entMan.AddComponentData(myEnt, new Rotation {
                        //Value =
                        //new Rotation(entGOPref.GetComponent<Rigidbody>().transform.LookAt(target.transform))
                        //new Rotation(entGOPref.transform.LookAt(target.transform))
                    });
                    //entMan.AddChunkComponentData(myEnt, new Transform(entGOPref.transform.LookAt(target.transform)));

                    break;
                case HEADING.INWARD:

                    break;
                case HEADING.OUTWARD:

                    break;

            }

        }
    }

}

//[CanEditMultipleObjects]
[CustomEditor(typeof(EntitySpawnerScript))]
public class DynamicInspector : Editor
{
    SerializedProperty myShape, myHeading;
    override public void OnInspectorGUI()
    {
        var eSS = target as EntitySpawnerScript;

        eSS.isEnabled = EditorGUILayout.Toggle("Enable Spawning:", eSS.isEnabled);
        myShape = serializedObject.FindProperty("spawnShape");
        myHeading = serializedObject.FindProperty("heading");

        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(eSS.isEnabled)))
        {
            eSS.dInspector = this;
            if (group.visible == true)
            {
                EditorGUI.indentLevel++;
                eSS.maxEntity = EditorGUILayout.FloatField("Maximum Entities:", eSS.maxEntity);
                //serializedObject.Update();
                eSS.entGOPref = (GameObject)EditorGUILayout.ObjectField("Entity Prefab:", eSS.entGOPref, typeof(UnityEngine.Object), true);
                //EditorGUILayout.PropertyField(eSS.entGOPref);
                //serializedObject.ApplyModifiedProperties();

                //eSS.spawnShape = (EntitySpawnerScript.SHAPE)EditorGUILayout.EnumPopup("Spawn Shape:", eSS.spawnShape);
                EditorGUILayout.PropertyField(myShape);
                EditorGUILayout.PropertyField(myHeading);
                serializedObject.ApplyModifiedProperties();
                switch (eSS.spawnShape)
                {
                    case EntitySpawnerScript.SHAPE.NONE:
                        break;
                    case EntitySpawnerScript.SHAPE.POINT:

                        break;
                    case EntitySpawnerScript.SHAPE.SQUARE:
                        eSS.scale = EditorGUILayout.Vector2Field("Scale", eSS.scale);

                        break;
                    case EntitySpawnerScript.SHAPE.CUBE:
                        eSS.scale = EditorGUILayout.Vector3Field("Scale", eSS.scale);

                        break;
                    case EntitySpawnerScript.SHAPE.CIRCLE:
                        eSS.radius = EditorGUILayout.FloatField("Radius", eSS.radius);
                        //eSS.scale = EditorGUILayout.Vector2Field("Scale", eSS.scale);
                        break;
                    case EntitySpawnerScript.SHAPE.SPHERE:
                        eSS.radius = EditorGUILayout.FloatField("Radius", eSS.radius);
                        //eSS.scale = EditorGUILayout.Vector3Field("Scale", eSS.scale);
                        break;
                }
                
                switch (eSS.heading)
                {
                    case EntitySpawnerScript.HEADING.NEUTRAL:
                        break;
                    case EntitySpawnerScript.HEADING.TARGETED:
                        eSS.target = (GameObject)EditorGUILayout.ObjectField("TargetObject", eSS.target, typeof(UnityEngine.GameObject), true);
                        break;
                    case EntitySpawnerScript.HEADING.INWARD:

                        break;
                    case EntitySpawnerScript.HEADING.OUTWARD:

                        break;
                    
                }
                if (GUILayout.Button("Preview"))
                {
                    eSS.SpawnPreview(eSS.spawnShape);
                    eSS.CreateHybridEnt();
                }
            }
        }

       
    }

  
}
