using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public GameObject tempGO;

    //[HideInInspector]
    public float radius, length, width, spawnRate, newSpawnTime;
    public int maxEntity, loopCounter;
    public Vector3 scale;

    public DynamicInspector dInspector;

    public enum SHAPE { NONE, POINT, SQUARE, CUBE, CIRCLE, SPHERE }
    public SHAPE spawnShape;

    public enum HEADING { NEUTRAL, TARGETED, OUTWARD, INWARD }
    public HEADING heading;

    public void OnEnable()
    {
        entMan = World.DefaultGameObjectInjectionWorld.EntityManager;

        tempGO = new GameObject { name = "Tool GameObject" };

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

    public void SpawnEntities()
    {
        if (loopCounter < maxEntity && Time.time > newSpawnTime)
        {
            CreateHybridEnt();
        }

    }

    public void CreateHybridEnt()
    {
        //var t = Task.Run(async delegate { await Task.Delay((int)(spawnRate * 1000f)); });

        if (loopCounter < maxEntity && Time.time > newSpawnTime)
        {

            for (int i = 0; i < maxEntity; i++)
            {
                Entity myEnt = entMan.Instantiate(entPref);
                Translation tempTranslation;
                Vector3 tempV3 = new Vector3();
                switch (spawnShape)
                {
                    case SHAPE.CUBE:
                       tempV3 = (new Vector3(UnityEngine.Random.Range(-scale.x * 0.5f, scale.x * 0.5f),
                       UnityEngine.Random.Range(-scale.y * 0.5f, scale.y * 0.5f),
                       UnityEngine.Random.Range(-scale.z * 0.5f, scale.z * 0.5f))
                       + gameObject.transform.position);

                        tempTranslation = new Translation
                        {
                            Value = tempV3
                        };
                        entMan.AddComponentData(myEnt, tempTranslation);
                        break;

                    case SHAPE.SQUARE:
                        tempV3 = (new Vector3(UnityEngine.Random.Range(-scale.x * 0.5f, scale.x * 0.5f),
                       UnityEngine.Random.Range(-scale.y * 0.5f, scale.y * 0.5f), 0)
                       + gameObject.transform.position);

                        tempTranslation = new Translation
                        {
                            Value = tempV3
                        };
                        entMan.AddComponentData(myEnt, tempTranslation);

                        break;

                    case SHAPE.SPHERE:
                        tempV3 = (new Vector3(UnityEngine.Random.Range(-radius * 0.5f, radius * 0.5f),
                       UnityEngine.Random.Range(-radius * 0.5f, radius * 0.5f),
                       UnityEngine.Random.Range(-radius * 0.5f, radius * 0.5f))
                       + gameObject.transform.position);

                        tempTranslation = new Translation
                        {
                            Value = tempV3
                        };
                        entMan.AddComponentData(myEnt, tempTranslation);

                        break;

                    case SHAPE.CIRCLE:
                        tempV3 = (new Vector3(UnityEngine.Random.Range(-radius * 0.5f, radius * 0.5f),
                       UnityEngine.Random.Range(-radius * 0.5f, radius * 0.5f), 0)
                       + gameObject.transform.position);

                        tempTranslation = new Translation
                        {
                            Value = tempV3
                        };
                        entMan.AddComponentData(myEnt, tempTranslation);

                        break;

                    case SHAPE.POINT:
                        entMan.AddComponentData(myEnt, new Translation { Value = gameObject.transform.position });

                        break;
                }

                switch (heading)
                {
                    case HEADING.NEUTRAL:
                        //This will spawn entities in whatever direction they're facing in their source prefab

                        break;
                    case HEADING.TARGETED:
                        //This will spawn entities ("entGOPref") facing a target object ("target") 
                        tempGO.transform.position = tempV3;
                        tempGO.transform.LookAt(targetGO.transform);

                        Quaternion qt1 = tempGO.transform.rotation;
                        entMan.AddComponentData(myEnt, new Rotation
                        {
                            Value = qt1
                        });

                        break;
                    case HEADING.INWARD:
                        //This will spawn entities facing towards the center of their spawner
                        tempGO.transform.position = tempV3;
                        tempGO.transform.LookAt(gameObject.transform);

                        qt1 = tempGO.transform.rotation;
                        entMan.AddComponentData(myEnt, new Rotation
                        {
                            Value = qt1
                        });
                        break;
                    case HEADING.OUTWARD:
                        //This will spawn entities facing outwards from the center of their spawner
                        tempGO.transform.position = tempV3;
                        tempGO.transform.LookAt(tempGO.transform.position - (gameObject.transform.position - tempGO.transform.position));
                        qt1 = tempGO.transform.rotation;
                        entMan.AddComponentData(myEnt, new Rotation
                        {
                            Value = qt1
                        });
                        break;

                }
            }
            loopCounter++;
            newSpawnTime = Time.time + spawnRate;
        }
    }

}

//[CanEditMultipleObjects]
[CustomEditor(typeof(EntitySpawnerScript))]
public class DynamicInspector : Editor
{
    SerializedProperty myShape, myHeading, myScale, myTargetGO,
        myRadius, myEntGOPref, myMaxEntities, amIEnabled, mySpawnRate;
    override public void OnInspectorGUI()
    {
        var eSS = target as EntitySpawnerScript;

        amIEnabled = serializedObject.FindProperty("isEnabled");
        myMaxEntities = serializedObject.FindProperty("maxEntity");
        mySpawnRate = serializedObject.FindProperty("spawnRate");
        myShape = serializedObject.FindProperty("spawnShape");
        myHeading = serializedObject.FindProperty("heading");
        myScale = serializedObject.FindProperty("scale");
        myTargetGO = serializedObject.FindProperty("targetGO");
        myRadius = serializedObject.FindProperty("radius");
        myEntGOPref = serializedObject.FindProperty("entGOPref");


        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(eSS.isEnabled)))
        {
            eSS.dInspector = this;

            EditorGUILayout.PropertyField(amIEnabled, new GUIContent("Enable Spawner"));
            if (group.visible == true)
            {
                EditorGUI.indentLevel++;
                //eSS.maxEntity = EditorGUILayout.FloatField("Maximum Entities:", eSS.maxEntity);
                EditorGUILayout.PropertyField(myEntGOPref, new GUIContent("Entity GameObject"));
                EditorGUILayout.PropertyField(myMaxEntities, new GUIContent("Max Entities"));
                EditorGUILayout.PropertyField(mySpawnRate, new GUIContent("Spawn Interval Delay"));
                EditorGUILayout.PropertyField(myShape, new GUIContent("Spawner Shape"));
                EditorGUILayout.PropertyField(myHeading, new GUIContent("Heading"));

                switch (eSS.spawnShape)
                {
                    case EntitySpawnerScript.SHAPE.NONE:
                        break;
                    case EntitySpawnerScript.SHAPE.POINT:
                        break;

                    case EntitySpawnerScript.SHAPE.SQUARE:
                        EditorGUILayout.PropertyField(myScale);
                        break;

                    case EntitySpawnerScript.SHAPE.CUBE:
                        EditorGUILayout.PropertyField(myScale);
                        break;

                    case EntitySpawnerScript.SHAPE.CIRCLE:
                        EditorGUILayout.PropertyField(myRadius);
                        break;

                    case EntitySpawnerScript.SHAPE.SPHERE:
                        EditorGUILayout.PropertyField(myRadius);
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
                if (GUILayout.Button("Manual Spawn"))
                {
                    eSS.CreateHybridEnt();
                    
                }
                

            }
            serializedObject.ApplyModifiedProperties();
        }

       
    }

  
}

//public class BasicSystem : ComponentSystem
//{
//    EntitySpawnerScript eSS;
//    protected override void OnUpdate()
//    {
//        switch (eSS.heading)
//        {
//            case EntitySpawnerScript.HEADING.TARGETED:
//                Entities.ForEach(())//Ask Jon about how to cull the effected entities to just the ones managed by each instance.
//                break;
//        }
//        Entities.ForEach((ref Translation tran) =>
//        {
//            float zPos = math.sin((float)Time.ElapsedTime);
//            tran.Value = new float3(tran.Value.x, tran.Value.y, zPos);
//        });
//    }
//}