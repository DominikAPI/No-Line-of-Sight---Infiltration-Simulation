using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(MeshRenderer))]
public class AmmoBar : MonoBehaviour
{
    private MeshRenderer rend;
    [SerializeField] private GameObject player;
    private PistolController gun;
    private Vector3 offset = new Vector3(0, 0.75f, -1f);
    private MaterialPropertyBlock mpb;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        gun = player.GetComponentInChildren<PistolController>();
        mpb = new MaterialPropertyBlock();
        gun.OnAmmoChanged += HandleAmmoChange;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }

    private void HandleAmmoChange(int current, int max)
    {
        float ammo = (float)current / max;
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_Progress", ammo);
        rend.SetPropertyBlock(mpb);
    }


}
