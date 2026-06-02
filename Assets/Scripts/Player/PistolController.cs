using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PistolController : MonoBehaviour
{
    [SerializeField] private float range = 20.0f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptySound;

    public Action<int, int> OnAmmoChanged;
    public Action OnShot;

    private AudioSource audioSource;
    private readonly float fireRate = 0.1f;
    private readonly int magazineCapacity = 6;
    private int remainingAmmo;
    private float cooldown = 0.1f;
    private bool isReloading = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        remainingAmmo = magazineCapacity;
        OnAmmoChanged?.Invoke(remainingAmmo, magazineCapacity);
    }

    private void Update()
    {
        cooldown += Time.deltaTime;
    }

    /// <summary>
    /// Casts a ray from the  weapon's shooting point, if it hits something with an <see cref="IKillable"/> interface
    /// </summary>
    public void Fire()
    {
        if (cooldown < fireRate || isReloading) return;

        if (remainingAmmo == 0)
        {
            audioSource.PlayOneShot(emptySound);
            return;
        }

        remainingAmmo--;
        OnAmmoChanged?.Invoke(remainingAmmo, magazineCapacity);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.right, range, hitMask);

        audioSource.PlayOneShot(shootSound);
        OnShot?.Invoke();

        if (!hit) return;

        IKillable killable = hit.collider.GetComponent<IKillable>();
        killable?.Die();
    }

    public IEnumerator Reload(float reloadTime = 0f)
    {
        if (isReloading) yield break;
        if (remainingAmmo == magazineCapacity) yield break;

        isReloading = true;

        if (reloadTime > 0f)
        {
            audioSource.PlayOneShot(reloadSound);
            yield return new WaitForSeconds(reloadTime);
        }

        remainingAmmo = magazineCapacity;
        OnAmmoChanged?.Invoke(remainingAmmo, magazineCapacity);

        isReloading = false;
    }
}
