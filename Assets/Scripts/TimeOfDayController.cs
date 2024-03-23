using UnityEngine;

public class TimeOfDayController : MonoBehaviour
{
    [SerializeField] private Light _sun;
    [SerializeField] private Vector3 _startOrientation;
    [SerializeField] private Vector3 _endOrientation;
    private Quaternion _endRotation;
    private Quaternion _startRotation;

    private float _timeToComplete;

    // Start is called before the first frame update
    private void Start()
    {
        // _sun.transform.rotation = Quaternion.Euler(_startOrientation);
    }

    public void StartDay(float timeToComplete)
    {
        _sun.transform.rotation = Quaternion.Euler(_startOrientation);
        _timeToComplete = timeToComplete;
        _startRotation = Quaternion.Euler(_startOrientation);
        _endRotation = Quaternion.Euler(_endOrientation);
    }

    public void UpdateTime(float timeRemaining)
    {
        var t = 1 - timeRemaining / _timeToComplete;
        // _sun.transform.rotation = Quaternion.Euler(Vector3.Lerp(_startOrientation, _endOrientation, t));
        _sun.transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, t);
    }
}