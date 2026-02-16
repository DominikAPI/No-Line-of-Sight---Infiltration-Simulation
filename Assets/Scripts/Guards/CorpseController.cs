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
        newPosition.z = -10f;
        Camera.main.transform.position = newPosition;
    }
}
