using UnityEngine;

public class ConfinedDetector : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.CompareTag("Sheep"))
            other.transform.parent.GetComponent<Sheep>().Confine();
    }
}