using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Shepherd selectedShepherd;
    public ConfinedDetector confinedDetector;
    

    public float PerceptionRadius = 10f;
    public float SeparationDistance = 1f;
    public float ShepherdPerceptionRadius = 10f;
    public float FlocCenterForce = 0.01f;
    public float MatchVelocityForce = 0.1f;
    public float AvoidanceForce = 0.1f;
    public float ShepherdVelocity = 10f;
    public float SheepVelocity = 5f;

    public static State STATE = State.Pause;


    // Not Serializable
    [System.NonSerialized] public List<Shepherd> Shepherds;
    [System.NonSerialized] public List<Sheep> Sheeps;
    [System.NonSerialized] public LevelController levelController;

    private void Awake()
    {
    }

    private void Start()
    {
        STATE = State.Pause;
        Shepherds = new List<Shepherd>();
        GameObject[] shepherdObjects = GameObject.FindGameObjectsWithTag("Shepherd");
        foreach (GameObject shepherdObject in shepherdObjects)
        {
            Shepherds.Add(shepherdObject.GetComponent<Shepherd>());
        }

        Sheeps = new List<Sheep>();
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
        foreach (GameObject sheepObject in sheepObjects)
        {
            Sheeps.Add(sheepObject.GetComponent<Sheep>());
        }

        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }

    private void Update()
    {
        if (GameController.STATE == State.Pause)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray, 100f);
            bool hasTouchedShepherd = false;
            foreach (RaycastHit raycastHit in raycastHits)
            {
                
                if (raycastHit.transform != null && raycastHit.transform.gameObject.CompareTag("Shepherd"))
                {
                    hasTouchedShepherd = true;
                }
            }
            foreach (RaycastHit raycastHit in raycastHits)
            {
                if (raycastHit.transform != null)
                {
                    if (raycastHit.transform.gameObject.CompareTag("Shepherd"))
                    {
                        if (selectedShepherd != null)
                        {
                            selectedShepherd.UnSelect();
                        }

                        selectedShepherd = raycastHit.transform.gameObject.GetComponent<Shepherd>();
                        selectedShepherd.Select();
                        return;
                    }

                    if (raycastHit.transform.gameObject.CompareTag("Terrain") && !hasTouchedShepherd)
                    {
                        if (selectedShepherd != null)
                        {
                            selectedShepherd.Move(raycastHit.point);
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedShepherd.UnSelect();
            selectedShepherd = null;
        }
    }
}