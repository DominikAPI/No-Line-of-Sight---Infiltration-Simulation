using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PistolController : MonoBehaviour
{
    [SerializeField] private float range = 20.0f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private AudioClip shootSound;

    public Action OnShot;
    private AudioSource audioSource;
    private readonly float fireRate = 0.1f;
    private float cooldown = 0.1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        if (cooldown < fireRate) return;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, transform.right, range, hitMask);

        audioSource.PlayOneShot(shootSound);
        OnShot?.Invoke();

        if (!hit) return;

        IKillable killable = hit.collider.GetComponent<IKillable>();
        killable?.Die();
    }

    public void Reload()
    {

    }
}
