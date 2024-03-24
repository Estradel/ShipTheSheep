using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static State STATE = State.Pause;
    public ConfinedDetector confinedDetector;


    public float PerceptionRadius = 10f;
    public float SeparationDistance = 1f;
    public float ShepherdPerceptionRadius = 10f;
    public float FlocCenterForce = 0.01f;
    public float MatchVelocityForce = 0.1f;
    public float AvoidanceForce = 0.1f;
    public float ShepherdVelocity = 10f;
    public float SheepVelocity = 5f;
    [NonSerialized] public LevelController levelController;
    private Shepherd selectedShepherd;
    [NonSerialized] public List<Sheep> Sheeps;


    // Not Serializable
    [NonSerialized] public List<Shepherd> Shepherds;

    private void Awake()
    {
    }

    private void Start()
    {
        STATE = State.Pause;
        Shepherds = new List<Shepherd>();
        var shepherdObjects = GameObject.FindGameObjectsWithTag("Shepherd");
        foreach (var shepherdObject in shepherdObjects) Shepherds.Add(shepherdObject.GetComponent<Shepherd>());

        Sheeps = new List<Sheep>();
        var sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
        foreach (var sheepObject in sheepObjects) Sheeps.Add(sheepObject.GetComponent<Sheep>());

        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
    }

    private void Update()
    {
        if (STATE == State.Pause) return;
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var raycastHits = Physics.RaycastAll(ray, 100f);
            var hasTouchedShepherd = false;
            foreach (var raycastHit in raycastHits)
                if (raycastHit.transform != null && raycastHit.transform.gameObject.CompareTag("Shepherd"))
                    hasTouchedShepherd = true;
            foreach (var raycastHit in raycastHits)
                if (raycastHit.transform != null)
                {
                    if (raycastHit.transform.gameObject.CompareTag("Shepherd"))
                    {
                        if (selectedShepherd != null) selectedShepherd.UnSelect();

                        selectedShepherd = raycastHit.transform.gameObject.GetComponent<Shepherd>();
                        selectedShepherd.Select();
                        return;
                    }

                    if (raycastHit.transform.gameObject.CompareTag("Terrain") && !hasTouchedShepherd)
                        if (selectedShepherd != null)
                            selectedShepherd.Move(raycastHit.point);
                }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selectedShepherd != null)
            {
                selectedShepherd.UnSelect();
                selectedShepherd = null;
            }
        }
    }
}