using UnityEngine;
using System.Collections;

public class BossZombie : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 500;
    public int health;

    [Header("Movement Settings")]
    public float speed = 2f;
    private Transform player;
    private Rigidbody2D rb;

    [Header("Attack Settings")]
    public int attackDamage;
    public float attackDelay = 2f;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    [Header("Drops")]
    public GameObject healthPrefab;
    public GameObject bulletPrefab;

    [Header("Audio")]
    public AudioClip attackSound;
    private AudioSource audioSource;

    [Header("Animator")]
    private Animator anim;

    private PlayerMovement playerMovement;

    void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        if (player == null) return;

        if (!isAttacking)
        {
            MoveTowardsPlayer();
            attackTimer = attackDelay; // saldýrý iptal olduðunda timer dolu olsun
        }
        else
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                StartCoroutine(DoAttack());
                attackTimer = attackDelay; // timer sýfýrlandý, bir sonraki saldýrý için bekleme
            }
        }

        CheckHealth();
    }

    private IEnumerator DoAttack()
    {
        // Animasyonu tetikle ve 1 frame bekle
        anim.SetTrigger("Attack");
        yield return null;

        // Sadece bir kere hasar uygula
        if (playerMovement != null)
        {
            playerMovement.health -= attackDamage;
        }

        // Saldýrý sesi
        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);
    }


    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }
    private void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");

        float dropChance = Random.value;
        if (dropChance < 0.10f && healthPrefab != null)
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
        else if (dropChance < 0.30f && bulletPrefab != null)
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        FindFirstObjectByType<ZombieSpawner>()?.ZombieDied();

        if (playerMovement != null)
            playerMovement.coin += 300;

        Destroy(gameObject, 1.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = true;
            attackTimer = attackDelay; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
        }
    }
}
