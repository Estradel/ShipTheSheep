using UnityEngine;

public class BillboardController : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.forward = mainCamera.transform.forward;
    }
}