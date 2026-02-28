using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movement Settings")]
    public float moveSpeed;

    [Header("Health")]
    public int health = 100;
    public int maxHealth = 100;

    [Header("Coin")]
    public int coin = 0;

    [Header("UI")]
    public GameObject tradePanel;
    public Image BackGround;
    public Image HealthBar;

    [Header("Joystick Settings")]
    public Joystick movementJoystick;
    public Joystick aimJoystick;

    public TMP_Text CoinText;

    [SerializeField] private Transform gunSlot;
    [SerializeField] private Transform pistolSlot;

    public bool facingRight = true;

    private float bossZombieAttackDelay = 3.5f;
    private float normalZombieAttackTimer = 0f;
    private float bossZombieAttackTimer = 0f;

    private Guns guns;
    private ZombieSpawner zombieSpawner;
    private Animator animator;

    public bool Karakteroldu = false;
    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        guns = GetComponentInChildren<Guns>();
        zombieSpawner = FindFirstObjectByType<ZombieSpawner>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        normalZombieAttackTimer += Time.deltaTime;
        bossZombieAttackTimer += Time.deltaTime;

        if (health <= 0)
        {
            Dead();
        }

        HealthBar.fillAmount = (float)health / maxHealth;
        if (BackGround.fillAmount != HealthBar.fillAmount)
        {
            BackGround.fillAmount = Mathf.Lerp(BackGround.fillAmount, HealthBar.fillAmount, 0.1f);
        }

        AimAndFlip();
        CoinText.text="Coin:" + coin;
    }

    private void FixedUpdate()
    {
        Walk();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HealthDrop"))
        {
            health += 20;
            if (health > maxHealth) health = maxHealth;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("BulletDrop"))
        {
            guns = GetComponentInChildren<Guns>();
            guns.totalAmmo += guns.maxMagazine;
            guns.currentAmmo += guns.maxMagazine;

            if (guns.totalAmmo > guns.maxTotalAmmo) guns.totalAmmo = guns.maxTotalAmmo;
            if (guns.currentAmmo > guns.maxMagazine) guns.currentAmmo = guns.maxMagazine;

            Destroy(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Normal Zombie"))
        {
            NormalZombie zombie = collision.gameObject.GetComponent<NormalZombie>();
            if (zombie != null && zombie.CanAttack())
            {
                health -= zombie.attackDamage;
            }
        }
        else if (collision.gameObject.CompareTag("Boss Zombie"))
        {
            if (bossZombieAttackTimer >= bossZombieAttackDelay)
            {
                BossZombie zombie = collision.gameObject.GetComponent<BossZombie>();
                if (zombie != null) health -= zombie.attackDamage;
                bossZombieAttackTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!zombieSpawner.waveActive && collision.gameObject.CompareTag("TradeShop"))
        {
            tradePanel.SetActive(true);
        }
    }

    private void Dead()
    {
        if (health <= 0)
        {
            Karakteroldu = true;
            animator.SetTrigger("Dead");
            rb.linearVelocity = Vector2.zero;
            SceneManager.LoadScene("ÖlümSahnesi");

        }
    }


    public void TradeShopOff()
    {
        tradePanel.SetActive(false);
    }

    #region Movement & Aim
    void Walk()
    {
        Vector2 movement;

        float moveXJoy = movementJoystick.Horizontal;
        float moveYJoy = movementJoystick.Vertical;

        if (Mathf.Abs(moveXJoy) < 0.1f && Mathf.Abs(moveYJoy) < 0.1f)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            movement = new Vector2(moveX, moveY);
        }
        else
        {
            movement = new Vector2(moveXJoy, moveYJoy);
        }

        animator.SetFloat("Walk", movement.magnitude > 0 ? 1f : 0f);
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void AimAndFlip()
    {
        Vector2 direction = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);
        if (direction.magnitude < 0.1f) return;

        if (direction.x > 0 && !facingRight) Flip();
        else if (direction.x < 0 && facingRight) Flip();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!facingRight) angle += 180f;

        gunSlot.rotation = Quaternion.Euler(0, 0, angle);
        pistolSlot.rotation = Quaternion.Euler(0, 0, angle);

    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    #endregion
}
