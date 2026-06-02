using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private PistolController pistol;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.position = target.position;
    }

    private void OnEnable()
    {
        pistol.OnAmmoChanged += UpdateUI;
    }

    private void OnDisable()
    {
        pistol.OnAmmoChanged -= UpdateUI;
    }

    private void UpdateUI(int current, int max)
    {
        ammoText.text = $"{current} / {max}";
    }
}
