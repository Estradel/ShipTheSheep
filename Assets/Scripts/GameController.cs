using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                if (raycastHit.transform != null && raycastHit.transform.gameObject.CompareTag("Shepherd") && raycastHit.transform.gameObject.GetComponent<Shepherd>() != selectedShepherd)
                    hasTouchedShepherd = true;
            foreach (var raycastHit in raycastHits)
                if (raycastHit.transform != null)
                {
                    if (raycastHit.transform.gameObject.CompareTag("Shepherd") && raycastHit.transform.gameObject.GetComponent<Shepherd>() != selectedShepherd)
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            levelController.Back();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            int index = 0;
            if (selectedShepherd != null)
            {
                selectedShepherd.UnSelect();
                index = Shepherds.IndexOf(selectedShepherd);
            }

            selectedShepherd = Shepherds[(index + 1) % Shepherds.Count];
            selectedShepherd.Select();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectShepherd(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectShepherd(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectShepherd(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectShepherd(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectShepherd(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectShepherd(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectShepherd(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectShepherd(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selectShepherd(9);
        }
    }

    private void selectShepherd(int index)
    {
        if (Shepherds.Count >= index)
        {
            if (selectedShepherd != null)
            {
                selectedShepherd.UnSelect();
            }

            selectedShepherd = Shepherds[index - 1];
            selectedShepherd.Select();
        }
    }
}