
using UnityEngine;

public interface IResetable
{
    public Vector3 OriginalPosition { get; set; }
    public Quaternion OriginalRotation { get; set; }

    public void ResetObject();
}
