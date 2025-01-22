using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("Gun Properties")]
    [SerializeField] private WeaponProperties currentWeapon;
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletsPrefab;
    [SerializeField] public int bulletsLeft;

    private bool holdingWeapon = true;
    private bool shooting;
    private bool reloading;
    private bool canShoot;

    private void Awake()
    {
        holdingWeapon = true;
        bulletsLeft = currentWeapon.magSize;
        canShoot = true;
    }

    private void Update()
    {
        // Input Managing and automatic managing
        WeaponInputManager();
    }

    private void WeaponInputManager()
    {
        // Control automatic vs non automatic
        if (currentWeapon.isAutomatic)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Control reloading buttons
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < currentWeapon.magSize && !reloading)
            Reload();

        // Shoot if the requirements are met.
        if (canShoot && shooting && !reloading && bulletsLeft > 0 && holdingWeapon)
            FireWeapon();
    }


    private void FireWeapon()
    {
        canShoot = false;

        // Loop through until no bullets left
        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            // Spawn bullet
            GameObject bulletCopy = Instantiate(bulletsPrefab, firePoint.position, Quaternion.identity);
            bulletCopy.GetComponent<Rigidbody2D>().AddForce(firePoint.up * currentWeapon.bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
        }

        // Start animation (Cam shake, play sound, muzzle flash)

        // Lower amounts of bullets left
        bulletsLeft--;
        Invoke("ResetShot", currentWeapon.shootingCooldown);
    }

    private void Reload()
    {
        // Set reloading bool to true
        reloading = true;

        // Play sound

        // Finish reload
        Invoke("FinishReload", currentWeapon.reloadTime);
    }

    private void FinishReload()
    {
        // Finish reloading
        reloading = false;
        bulletsLeft = currentWeapon.magSize;
    }

    public void DropGun()
    {

    }

    private void ResetShot()
    {
        canShoot = true;
    }
}
