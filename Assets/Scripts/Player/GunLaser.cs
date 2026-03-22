using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(PistolController))]
public class GunLaser : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_05 = new(0.05f);

    [SerializeField] private float range = 20.0f;
    [SerializeField] private LayerMask layerMask;

    private LineRenderer lineRenderer;
    private PistolController pistolController;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        pistolController = GetComponent<PistolController>();
        
    }

    private void OnEnable()
    {
        pistolController.OnShot += HandleShot;
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

    private void HandleShot() => StartCoroutine(FlashLaser());

    private IEnumerator FlashLaser()
    {
        lineRenderer.startWidth *= 2f;
        lineRenderer.endWidth *= 2f;

        yield return _waitForSeconds0_05;

        lineRenderer.startWidth *= 0.5f;
        lineRenderer.endWidth *= 0.5f;
    }

    private void OnDisable()
    {
        pistolController.OnShot -= HandleShot;
    }
}
