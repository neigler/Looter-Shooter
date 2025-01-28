using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class WeaponScript : MonoBehaviour
{
    [Header("Gun Properties")]
    [HideInInspector] public WeaponProperties currentWeapon;
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private Sprite noWeaponSprite;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletsPrefab;
    [SerializeField] private GameObject shell;

    [Header("Melee Properties")]
    [SerializeField] private Animator animator;
    public float coolDownPeriodInSeconds;
    public int meleeDamage;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    [HideInInspector] public bool canPunch;

    [Header("Screen Shake Properties")]
    [SerializeField] private float scLength;
    [SerializeField] private float scPower;
    [SerializeField] private PlayerCamera cam;

    [Header("Mag Size")]
    public TextMeshProUGUI ammoText;
    public int magSize;
    public int bulletsLeft;

    [Header("Muzzle Flash Properties")]
    [SerializeField] private GameObject flashSprite;
    [Range(1, 25)][SerializeField] public int framesToFlash = 1;

    [Header("GUI")]
    public Image[] projectileImages;

    private bool shooting;
    private bool reloading;
    [HideInInspector] public bool holdingWeapon = true;
    [HideInInspector] public bool canShoot;

    private void Start()
    {
        holdingWeapon = false;
        canShoot = true;
        canPunch = true;
        StartCoroutine(StartCooldown());

        UpdateAmmoUI();
    }


    private void Update()
    {
        UpdateAmmoUI();

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

            // Shoot if the requirements are met.
            if (canShoot && shooting && !reloading && bulletsLeft > 0 && holdingWeapon)
                FireWeapon();

            if (Input.GetKeyDown(KeyCode.Q) && holdingWeapon && !reloading)
                DropGun();
        }
        else
        {
            if (!holdingWeapon && Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
            {
                MeleePunch();
            }
        }

        if (!holdingWeapon)
        {
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
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
        //Play Sound
        AudioManager.PlaySound(SoundType.SHOOT);

        // Camera Shake
        cam.StartShake(scLength, scPower);

        // Muzzle Flash
        StartCoroutine(MuzzleFlash());

        // Eject Shell
        EjectShell();

        UpdateAmmoUI();

        // Lower amounts of bullets left
        bulletsLeft--;
        Invoke("ResetShot", currentWeapon.shootingCooldown);
    }

    void MeleePunch()
    {

        // Check if countdown is ready
        if (canPunch == false)
            return;

        // Play an attack animation
        animator.SetTrigger("Attack");

        // Detect enemies in hitbox
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Hurt enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyStats>().currentHealth -= meleeDamage;
        }

        StartCoroutine(StartCooldown());
    }

    public IEnumerator StartCooldown()
    {
        canPunch = false;

        yield return new WaitForSeconds(coolDownPeriodInSeconds);

        canPunch = true;
    }


    public void UpdateAmmoUI()
    {

        if (holdingWeapon == false)
        {
            ammoText.text = "";
        }
        else
        {
            ammoText.text = bulletsLeft + "/" + magSize;
        }
    }

    public void Reload()
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

    public void FinishReload()
    {
        // Finish reloading
        reloading = false;
        bulletsLeft = magSize;
        UpdateAmmoUI();
    }

    public void DropGun()
    {
        // Spawn Dropped gun
        GameObject droppedGun = Instantiate(currentWeapon.weaponPrefab, transform.position, Quaternion.identity);

        // Get the dropped gun as a interactable script
        Interactable interactableScript = droppedGun.GetComponent<Interactable>();

        //Set Magazine settings
        interactableScript.magSize = magSize;
        interactableScript.bulletsLeft = bulletsLeft;


        //Reset Booleans
        currentWeapon = null;
        holdingWeapon = false;
        UpdateAmmoUI();
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
        if (!(firePoint.rotation.eulerAngles.z >= 90 && firePoint.rotation.eulerAngles.z < 270))
        {
            xVnot *= -1;
        }
        ejectedShell.GetComponent<ShellCase>().xVnot = xVnot;
        ejectedShell.GetComponent<ShellCase>().yVnot = yVnot;
    }
    IEnumerator MuzzleFlash()
    {
        //Muzzle flash affect.
        flashSprite.SetActive(true);

        var framesFlashed = 0;
        while (framesFlashed < framesToFlash)
        {
            framesFlashed++;
            yield return null;
        }
        flashSprite.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
