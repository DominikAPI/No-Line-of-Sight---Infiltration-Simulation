using UnityEngine;

public class CorpseController : MonoBehaviour, IDetectable
{
    private DetectionResponse detectionResponse;

    private void Awake()
    {
        detectionResponse = new CorpseDetectionResponse();
    }

    public DetectionResponse GetDetectionResponse() => detectionResponse;

    public void FocusOn()
    {
        Vector3 newPosition = transform.position;
        newPosition.z = Camera.main.transform.position.z;
        Camera.main.transform.position = newPosition;
    }
}
