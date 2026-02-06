using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GunLaser : MonoBehaviour
{
    [SerializeField] private float range = 20.0f;
    [SerializeField] private LayerMask layerMask;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }


    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.right;
        Vector3 endPoint;

        lineRenderer.SetPosition(0, origin);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, range, layerMask);

        if (hit.collider != null) endPoint = hit.point;
        else endPoint = origin + direction * range;

        lineRenderer.SetPosition(1, endPoint);
    }
}
