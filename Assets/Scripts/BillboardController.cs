using UnityEngine;

public class BillboardController : MonoBehaviour
{
    private LevelController levelController;
    // Start is called before the first frame update
    private void Start()
    {
        levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.forward = levelController.MainCamera.transform.forward;
    }
}