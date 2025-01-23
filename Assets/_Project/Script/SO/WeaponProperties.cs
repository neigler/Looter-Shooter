using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProperties", menuName = "Scriptable Objects/WeaponProperties")]
public class WeaponProperties : ScriptableObject
{
    [SerializeField] public Sprite heldWeaponSprite;
    [SerializeField] public float shootingCooldown;
    [SerializeField] public int damage;
    [SerializeField] public float spread;
    [SerializeField] public float bulletsPerShot;
    [SerializeField] public float reloadTime;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public GameObject weaponPrefab;

    [Header("Mag Properties")]
    [SerializeField] public int magSize;
    [SerializeField] public bool isAutomatic;
    [SerializeField] public bool autoReload;
}
