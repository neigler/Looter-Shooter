using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("Gun Properties")]
    [HideInInspector] public WeaponProperties currentWeapon;
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private Sprite noWeaponSprite;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletsPrefab;
    [SerializeField] public int bulletsLeft;
    [SerializeField] private GameObject shell;

    [Header("Screen Shake Properties")]
    [SerializeField] private float scLength;
    [SerializeField] private float scPower;
    [SerializeField] private PlayerCamera cam;
    private bool shooting;
    private bool reloading;
    [HideInInspector] public bool holdingWeapon = true;
    [HideInInspector] public bool canShoot;



    private void Awake()
    {
        holdingWeapon = false;
        canShoot = true;
        if (currentWeapon != null)
            bulletsLeft = currentWeapon.magSize;
    }

    private void Update()
    {
        // Input Managing and automatic managing
        WeaponInputManager();

        // Animation / Sprite changing Manager
        SpriteChangingManager();
    }

    private void WeaponInputManager()
    {
        // Control automatic vs non automatic
        if (currentWeapon != null)
        {
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

            if (Input.GetKeyDown(KeyCode.Q) && holdingWeapon && !reloading)
                DropGun();
        }
    }


    private void FireWeapon()
    {
        canShoot = false;

        // Loop through until no bullets left
        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            // Spawn bullet
            GameObject bulletCopy = Instantiate(bulletsPrefab, firePoint.position, this.transform.rotation);
            bulletCopy.GetComponent<Rigidbody2D>().AddForce(firePoint.up * currentWeapon.bulletSpeed, ForceMode2D.Impulse);
        }

        // Start animation (Cam shake, play sound, muzzle flash)
        AudioManager.PlaySound(SoundType.SHOOT);
        cam.StartShake(scLength, scPower);
        EjectShell();

        // Lower amounts of bullets left
        bulletsLeft--;
        Invoke("ResetShot", currentWeapon.shootingCooldown);
    }

    private void Reload()
    {
        // Set reloading bool to true
        reloading = true;

        // Play sound
        AudioManager.PlaySound(SoundType.RELOAD);

        // Finish reload
        Invoke("FinishReload", currentWeapon.reloadTime);
    }

    private void SpriteChangingManager()
    {
        if (!holdingWeapon)
        {
            bodySprite.sprite = noWeaponSprite;
        }
        else
        {
            if (currentWeapon != null)
                bodySprite.sprite = currentWeapon.heldWeaponSprite;
        }
    }

    private void FinishReload()
    {
        // Finish reloading
        reloading = false;
        bulletsLeft = currentWeapon.magSize;
    }

    public void DropGun()
    {
        Instantiate(currentWeapon.weaponPrefab, transform.position, Quaternion.identity);
        currentWeapon = null;
        holdingWeapon = false;
    }

    private void ResetShot()
    {
        canShoot = true;
    }

    private void EjectShell()
    {
        GameObject ejectedShell = Instantiate(shell, transform.position, transform.rotation);
        float xVnot = Random.Range(5f, 10f);
        float yVnot = Random.Range(5f, 10f);
        if (!(firePoint.rotation.eulerAngles.z >= 90 && firePoint.rotation.eulerAngles.z < 360))
        {
            xVnot *= -1;
        }
        ejectedShell.GetComponent<ShellCase>().xVnot = xVnot;
        ejectedShell.GetComponent<ShellCase>().yVnot = yVnot;
    }
}
