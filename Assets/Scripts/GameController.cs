using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    // Not Serializable
    [System.NonSerialized]
    public List<Shepherd> Shepherds;
    [System.NonSerialized]
    public List<Sheep> Sheeps;

    private void Awake() 
    {
        Debug.Log("Awake2");
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this);
        } 
        else 
        { 
            Instance = this;
        }
        
        
    }

    private void Start()
    {
        Shepherds = new List<Shepherd>();
        GameObject[] shepherdObjects = GameObject.FindGameObjectsWithTag("Shepherd");
        foreach (GameObject shepherdObject in shepherdObjects)
        {
            Shepherds.Add(shepherdObject.GetComponent<Shepherd>());
        }
        
        Sheeps = new List<Sheep>();
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
        Debug.Log(sheepObjects.Length);
        foreach (GameObject sheepObject in sheepObjects)
        {
            Sheeps.Add(sheepObject.GetComponent<Sheep>());
        }
    }
}
