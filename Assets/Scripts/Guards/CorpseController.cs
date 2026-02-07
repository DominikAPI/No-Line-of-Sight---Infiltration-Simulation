using UnityEngine;

public class CorpseController : MonoBehaviour, IDetectable
{
    private DetectionResponse detectionResponse;

    private void Awake()
    {
        detectionResponse = new CorpseDetectionResponse();
    }

    public DetectionResponse GetDetectionResponse() => detectionResponse;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
