using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DetectionBarController : MonoBehaviour
{
    private Renderer rend;
    private IDetectionTimeSource currentSource;
    private static readonly Vector3 offset = new(0, 0.75f, 0);
    private MaterialPropertyBlock mpb;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void ActivateUpdate(IDetectionTimeSource source)
    {
        if (currentSource != null) currentSource.OnDetectionTimeChange -= UpdateBar;

        currentSource = source;
        currentSource.OnDetectionTimeChange += UpdateBar;
    }

    public void DeactivateUpdate(IDetectionTimeSource source)
    {
        if (currentSource != null) currentSource.OnDetectionTimeChange -= UpdateBar;
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_Progress", 0f);
        rend.SetPropertyBlock(mpb);
        gameObject.SetActive(false);
    }
    private void UpdateBar(float currentTime, float detectionTime)
    {
        float progress = Mathf.Clamp01(currentTime / detectionTime);

        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_Progress", progress);
        rend.SetPropertyBlock(mpb);

        gameObject.SetActive(progress > 0.01f);
    }

    private void LateUpdate()
    {
        Transform parent = transform.parent;
        Vector3 worldOffset = offset;
        Vector3 localOffset = Quaternion.Inverse(parent.rotation) * worldOffset;

        transform.localPosition = localOffset;
        transform.rotation = Quaternion.identity;
    }
}
