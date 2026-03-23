
using UnityEngine;

public interface IDetectable
{
    public DetectionResponse GetDetectionResponse();

    public void FocusOn();

    public Vector3 GetPosition();
}
