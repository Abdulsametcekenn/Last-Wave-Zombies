using UnityEngine;

public class BomberZombie : MonoBehaviour
{
    [Header("Bomber Zombie Health")]
    public int bomberZombieHealth;
    [Header("Bomber Zombie Movement Settings")]
    public float speed;
    [Header("Bomber Zombie Settings")]
    public float explosionDelay;
    public float stopDistance;
    public float explosionRadius;
    public float explosionDamage;

    [Header("Drop")]
    public GameObject healthPrefab;
    public GameObject BulletPrefab;

    [Header("Ses")]
    public AudioClip Attack;

    private Animator animator;
    private AudioSource audioSource;

    private int maxHealth = 120;
    private Transform player;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool isExploding = false;
    private float explodeTimer = 0f;

    void Start()
    {
        bomberZombieHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isExploding)
        {
            explodeTimer += Time.deltaTime;
            if (explodeTimer >= explosionDelay)
            {
                Explode();
            }
            return; 
        }

        // Normal hareket
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= explosionRadius) 
        {
            isExploding = true;
            explodeTimer = 0f;
            rb.linearVelocity = Vector2.zero; 

            if (animator != null)
            {
                animator.SetTrigger("Patlama");
                StartCoroutine(PlayExplosionSoundWithDelay(0.8f));
            }
        }

        Health();
    }

    void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                PlayerMovement playerMovement = collider.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.health -= (int)explosionDamage;
                }
            }
        }

        FindFirstObjectByType<ZombieSpawner>().ZombieDied();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    bool isDead = false;
    void Health()
    {
        if (bomberZombieHealth <= 0&&!isDead )
        {
            isDead = true;
            col.enabled = false;
            rb.simulated = false;
            speed = 0f;
            animator.SetTrigger("Die");
            float dropChance = Random.value;
            if (dropChance < 0.10f)
            {
                Instantiate(healthPrefab, transform.position, Quaternion.identity);
            }
            else if (dropChance < 0.30f)
            {
                Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            }

            FindFirstObjectByType<ZombieSpawner>().ZombieDied();
            Destroy(gameObject,1.5f);
            PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
            playerMovement.coin += 100;
        }
    }
    private System.Collections.IEnumerator PlayExplosionSoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(Attack);
    }
}
