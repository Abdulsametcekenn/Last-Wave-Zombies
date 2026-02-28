using UnityEngine;
using System.Collections;

public class Guns : MonoBehaviour
{
    [Header("Gun Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireCooldown;
    public float bulletSpeed;
    public int bulletDamage;
    public bool pistol = false;
    public bool shotGun = false;
    public bool smg = false;
    public bool assaultRifle = false;

    [SerializeField] private GameObject muzzleFlash;  
    [SerializeField] private float flashDuration = 0.05f;  

    [Header("Shotgun Settings")]
    public int pelletsPerShot = 8; 
    public float spreadAngle = 15f; 

    [Header("Ammo Settings")]
    public int maxMagazine;
    public int currentAmmo;
    public int totalAmmo;
    public int maxTotalAmmo;

    [Header("Reload Settings")]
    public float reloadTime;

    [Header("Recoil Settings")]
    public Transform gunTransform;
    public float recoilDistance = 0.1f;
    public float recoilBackSpeed = 0.05f;  
    public float recoilReturnSpeed = 0.1f; 

    [Header("Ammo Settings")]
    public int ammoPrice = 1;

    [Header("Joystick Control")]
    public Joystick aimJoystick;

    public Transform shellEjectPoint; 
    public GameObject shellPrefab;   
    public float shellForce = 2f;

    [Header("Sound")]
    public AudioClip fireSound;
    public AudioClip reloadSound;

    private Vector3 originalGunPos;
    private float fireTimer = 0f;
    private bool isReloading = false;

    private Animator animator;
    private AudioSource audioSource;
    private PlayerMovement playermovement;

    void Start()
    {
        currentAmmo = maxMagazine;
        maxTotalAmmo = totalAmmo;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        originalGunPos = gunTransform.localPosition; 

        if (aimJoystick == null)
        {
            GameObject joystickObj = GameObject.FindGameObjectWithTag("AimJoystick");
            if (joystickObj != null)
            {
                aimJoystick = joystickObj.GetComponent<Joystick>();
            }
        }
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        if(currentAmmo<=0&&totalAmmo>0&&!isReloading)
        {
            StartCoroutine(Reload());
        }

        if (isReloading)
            return;

        Vector2 direction = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

        if (direction.magnitude > 0.3f && fireTimer >= fireCooldown && currentAmmo > 0)
        {
            Fire();
            fireTimer = 0f;
        }

    }


    void Fire()
    {
        Vector2 direction = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

        if (direction.magnitude < 0.1f)
            direction = playermovement.facingRight ? Vector2.right : Vector2.left;

        direction.Normalize();

        if (shotGun)
        {
            for (int i = 0; i < pelletsPerShot; i++)
            {
                float angleOffset = Random.Range(-spreadAngle, spreadAngle);
                Quaternion pelletRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset);
                GameObject pellet = Instantiate(bulletPrefab, firePoint.position, pelletRotation);
                Bullet bulletScript = pellet.GetComponent<Bullet>();
                bulletScript.bulletDamage = bulletDamage;
                bulletScript.bulletSpeed = bulletSpeed;
            }
        }
        else
        {
            Quaternion bulletRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.bulletDamage = bulletDamage;
            bulletScript.bulletSpeed = bulletSpeed;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = direction * bulletSpeed;
        }
        audioSource.PlayOneShot(fireSound);
        Shoot();
        StartCoroutine(ApplyRecoil());
        StartCoroutine(MuzzleFlashEffect());
        currentAmmo--;
    }

    public IEnumerator Reload()
    {
        if (isReloading) yield break;
        if (currentAmmo == maxMagazine || totalAmmo <= 0) yield break;

        isReloading = true;

        if (audioSource && reloadSound)
            audioSource.PlayOneShot(reloadSound);

        if (animator != null)
            animator.SetTrigger("Reload");

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = maxMagazine - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, totalAmmo);

        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        yield return new WaitForSeconds(0.1f);

        isReloading = false;
    }


    IEnumerator MuzzleFlashEffect()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(true);
            muzzleFlash.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
            muzzleFlash.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-20f, 20f));
            yield return new WaitForSeconds(flashDuration);
            muzzleFlash.SetActive(false);
        }
    }
    IEnumerator ApplyRecoil()
    {
        if (gunTransform == null) yield break;

        Vector3 recoilPos = originalGunPos - new Vector3(recoilDistance, 0, 0);

        float elapsed = 0f;
        while (elapsed < recoilBackSpeed)
        {
            gunTransform.localPosition = Vector3.Lerp(originalGunPos, recoilPos, elapsed / recoilBackSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gunTransform.localPosition = recoilPos;

        elapsed = 0f;
        while (elapsed < recoilReturnSpeed)
        {
            gunTransform.localPosition = Vector3.Lerp(recoilPos, originalGunPos, elapsed / recoilReturnSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gunTransform.localPosition = originalGunPos;
    }

    void Shoot()
    {

        GameObject shell = Instantiate(shellPrefab, shellEjectPoint.position, shellEjectPoint.rotation);

        Rigidbody2D rb = shell.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 forceDir = new Vector2(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
            rb.AddForce(forceDir * shellForce, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse); // Dönme efekti
        }

        Destroy(shell, 1.5f);
    }
}
