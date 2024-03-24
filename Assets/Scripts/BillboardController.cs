using UnityEngine;

public class BillboardController : MonoBehaviour
{
    private LevelController levelController;
    // Start is called before the first frame update
    private void Start()
    {
        if (GameObject.FindWithTag("LevelController") != null)
        {
            levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (levelController == null || levelController.MainCamera == null)
        {
            transform.forward = Camera.main.transform.forward;
            return;
        }
        transform.forward = levelController.MainCamera.transform.forward;
    }
}