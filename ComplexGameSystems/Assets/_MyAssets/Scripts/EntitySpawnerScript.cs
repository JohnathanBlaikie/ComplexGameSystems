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
    public GameObject targetGO;
    public Entity entPref;

    //[HideInInspector]
    public float radius, length, width, maxEntity, spawnRate;
    public Vector3 scale;

    public DynamicInspector dInspector;

    public enum SHAPE { NONE, POINT, SQUARE, CUBE, CIRCLE, SPHERE }
    public SHAPE spawnShape;

    public enum HEADING { NEUTRAL, TARGETED, OUTWARD, INWARD}
    public HEADING heading;

    public void OnEnable()
    {
        entMan = World.DefaultGameObjectInjectionWorld.EntityManager;

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        settings.ConversionFlags = GameObjectConversionUtility.ConversionFlags.AssignName;

        entPref = GameObjectConversionUtility.ConvertGameObjectHierarchy(entGOPref, settings);
    }

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

        for (int i = 0; i < maxEntity; i++)
        {
            Entity myEnt = entMan.Instantiate(entPref);
            Translation tempTranslation;
            Transform tempTransform = entGOPref.transform;

            //entMan.AddComponentData(myEnt, new Translation { Value = new float3() });
            if (spawnShape == SHAPE.CUBE || spawnShape == SHAPE.SQUARE)
            {
                tempTranslation = new Translation
                {
                    Value =
                    (new Vector3(UnityEngine.Random.Range(-scale.x * 0.5f, scale.x * 0.5f),
                    UnityEngine.Random.Range(-scale.y * 0.5f, scale.y * 0.5f),
                    UnityEngine.Random.Range(-scale.z * 0.5f, scale.z * 0.5f))
                    + gameObject.transform.position)
                };
                entMan.AddComponentData(myEnt, tempTranslation);
            }
            else if (spawnShape == SHAPE.SPHERE || spawnShape == SHAPE.CIRCLE)
            {
                //entMan.AddComponentData(myEnt, new Translation { Value = new float3() });
                tempTranslation = new Translation
                {
                    Value =
                    (new Vector3(UnityEngine.Random.Range(-radius * 0.5f, radius * 0.5f),
                    UnityEngine.Random.Range(-radius * 0.5f, radius * 0.5f),
                    UnityEngine.Random.Range(-radius * 0.5f, radius * 0.5f))
                    + gameObject.transform.position)
                };
                entMan.AddComponentData(myEnt, tempTranslation);
            }
            else if (spawnShape == SHAPE.POINT)
            {
                entMan.AddComponentData(myEnt, new Translation { Value = gameObject.transform.position });
            }
            else { }

            switch (heading)
            {
                case HEADING.NEUTRAL:
                    //This will spawn entities in whatever direction they're facing in their source

                    break;
                case HEADING.TARGETED:
                    //This will spawn entities ("entGOPref") facing a target object ("target") 

                    tempTransform.LookAt(targetGO.transform);

                    Quaternion qt1 = tempTransform.rotation;
                    entMan.AddComponentData(myEnt, new Rotation
                    {
                        Value = qt1
                    });
                    //entMan.AddChunkComponentData(myEnt, new Transform(entGOPref.transform.LookAt(target.transform)));

                    break;
                case HEADING.INWARD:
                    //This will spawn entities facing towards the center of their spawner

                    break;
                case HEADING.OUTWARD:
                    //This will spawn entities facing outwards from the center of their spawner

                    break;

            }

        }
    }

}

//[CanEditMultipleObjects]
[CustomEditor(typeof(EntitySpawnerScript))]
public class DynamicInspector : Editor
{
    SerializedProperty myShape, myHeading, myScale, myTargetGO, myRadius;
    override public void OnInspectorGUI()
    {
        var eSS = target as EntitySpawnerScript;

        eSS.isEnabled = EditorGUILayout.Toggle("Enable Spawning:", eSS.isEnabled);
        myShape = serializedObject.FindProperty("spawnShape");
        myHeading = serializedObject.FindProperty("heading");
        myScale = serializedObject.FindProperty("scale");
        myTargetGO = serializedObject.FindProperty("targetGO");
        myRadius = serializedObject.FindProperty("radius");


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

                EditorGUILayout.PropertyField(myShape);
                EditorGUILayout.PropertyField(myHeading);

                switch (eSS.spawnShape)
                {
                    case EntitySpawnerScript.SHAPE.NONE:
                        break;
                    case EntitySpawnerScript.SHAPE.POINT:

                        break;
                    case EntitySpawnerScript.SHAPE.SQUARE:
                        //EditorGUILayout.Vector2Field(myScale);
                        //eSS.scale = myScale;
                        EditorGUILayout.PropertyField(myScale);
                       
                        //eSS.scale = EditorGUILayout.Vector2Field("Scale", eSS.scale);

                        break;
                    case EntitySpawnerScript.SHAPE.CUBE:
                        EditorGUILayout.PropertyField(myScale);
                        //eSS.scale = EditorGUILayout.Vector3Field("Scale", eSS.scale);

                        break;
                    case EntitySpawnerScript.SHAPE.CIRCLE:
                        EditorGUILayout.PropertyField(myRadius);

                        //eSS.radius = EditorGUILayout.FloatField("Radius", eSS.radius);
                        //eSS.scale = EditorGUILayout.Vector2Field("Scale", eSS.scale);
                        break;
                    case EntitySpawnerScript.SHAPE.SPHERE:
                        EditorGUILayout.PropertyField(myRadius);

                        //eSS.radius = EditorGUILayout.FloatField("Radius", eSS.radius);
                        //eSS.scale = EditorGUILayout.Vector3Field("Scale", eSS.scale);
                        break;
                }
                
                switch (eSS.heading)
                {
                    case EntitySpawnerScript.HEADING.NEUTRAL:
                        break;
                    case EntitySpawnerScript.HEADING.TARGETED:
                        //eSS.target = (GameObject)EditorGUILayout.ObjectField("TargetObject", eSS.target, typeof(UnityEngine.GameObject), true);
                        EditorGUILayout.PropertyField(myTargetGO);
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
                serializedObject.ApplyModifiedProperties();

            }
        }

       
    }

  
}
