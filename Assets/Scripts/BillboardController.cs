using UnityEngine;

public class BillboardController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}