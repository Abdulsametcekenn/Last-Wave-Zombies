using UnityEngine;

public class FasterZombie : MonoBehaviour
{
    [Header("Zombie Speed")]
    public float speed  ;
    private Rigidbody2D rb;
    private Collider2D col;
    private Transform player;

    [Header("Zombie Health")]
    public int maxHealth = 150;
    public int health;

    [Header("Zombie Attack")]
    public int attackDamage  ;
    public float attackDelay  ;
    private float attackTimer ;
    private bool isAttacking = false;

    [Header("Drop")]
    public GameObject healthPrefab;
    public GameObject bulletPrefab;

    [Header("Ses")]
    public AudioClip attackSound;

    private Animator animator;
    private AudioSource audioSource;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        attackTimer += Time.deltaTime;

        if (!isAttacking)
        {
            // Oyuncuya doðru hareket et
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
            animator.SetBool("Attack", false);
        }
        else
        {
            // Saldýrý animasyonu
            animator.SetBool("Attack", true);

            if (attackTimer >= attackDelay)
            {
                // Ses efektini saldýrý anýnda çal
                if (attackSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(attackSound);
                }

                // Oyuncuya hasar ver
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.health -= attackDamage;
                }

                attackTimer = 0f; // tekrar saldýrmak için sýfýrla
            }
        }
    }

    void HealthCheck()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            col.enabled = false;
            rb.simulated = false;
            speed = 0f;
            animator.SetTrigger("Die");

            // Drop sistemi
            float dropChance = Random.value;
            if (dropChance < 0.15f)
            {
                Instantiate(healthPrefab, transform.position, Quaternion.identity);
            }
            else if (dropChance < 0.40f)
            {
                Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            }

            // Spawner’a bildir
            FindFirstObjectByType<ZombieSpawner>().ZombieDied();

            // Oyuncuya para ekle
            PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
            playerMovement.coin += 150;

            Destroy(gameObject, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = true;
            attackTimer = attackDelay; // Ýlk saldýrýyý hemen yapsýn
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        HealthCheck();
    }
}
