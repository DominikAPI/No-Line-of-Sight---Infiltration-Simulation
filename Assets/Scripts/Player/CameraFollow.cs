using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followObject;

    public bool Enabled { get; set; } = true;

    // Update is called once per frame
    void Update()
    {
        if (!Enabled) return;

        Vector3 position = followObject.position;
        position.z = transform.position.z;

        transform.position = position;
    }
}
