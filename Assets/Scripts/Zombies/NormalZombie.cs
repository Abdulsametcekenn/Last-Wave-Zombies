using UnityEngine;

public class NormalZombie : MonoBehaviour
{
    [Header("Zombie Health")]
    public int health;
    private int maxHealth = 100;

    [Header("Zombie Speed")]
    public Transform player;
    public float speed;

    [Header("Zombie Attack")]
    public int attackDamage = 10;
    public float attackDelay = 3f; 
    private float lastAttackTime = 0f; 

    [Header("Drop")]
    public GameObject healthPrefab;
    public GameObject BulletPrefab;

    [Header("Ses")]
    public AudioClip Attack;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Start()
    {
        attackDamage = 10;
        health = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        normalZombieHealth();
    }
    private void FixedUpdate()
    {
            if (player == null) return;

            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public bool CanAttack()
    {
        if (Time.time >= lastAttackTime + attackDelay)
        {
            lastAttackTime = Time.time;
            audioSource.PlayOneShot(Attack);
            return true;

        }
        return false;
    }

    public void normalZombieHealth()
    {
        if (health <= 0)
        {
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
            PlayerMovement playerMovement = FindFirstObjectByType<PlayerMovement>();
            playerMovement.coin += 50;
            Destroy(gameObject);
        }
    }
}
