using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float bulletSpeed;
    public float lifetime;
    public int bulletDamage;

    [Header("Effects")]
    public GameObject bloodEffectPrefab; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bloodEffectPrefab != null &&
    (collision.CompareTag("Normal Zombie") ||
     collision.CompareTag("Bomber Zombie") ||
     collision.CompareTag("Faster Zombie") ||
     collision.CompareTag("Boss Zombie")))
        {
            Vector3 hitPosition = transform.position; 
            GameObject blood = Instantiate(bloodEffectPrefab, hitPosition, Quaternion.identity);
            Destroy(blood, 2f); 
        }


        if (collision.CompareTag("Normal Zombie"))
        {
            NormalZombie normalzombie = collision.GetComponent<NormalZombie>();
            normalzombie.health -= bulletDamage;
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Bomber Zombie"))
        {
            BomberZombie bomberzombie = collision.GetComponent<BomberZombie>();
            bomberzombie.bomberZombieHealth -= bulletDamage;
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Faster Zombie"))
        {
            FasterZombie fasterzombie = collision.GetComponent<FasterZombie>();
            fasterzombie.health -= bulletDamage;
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Boss Zombie"))
        {
            BossZombie bosszombie = collision.GetComponent<BossZombie>();
            bosszombie.health -= bulletDamage;
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Bullet Destroyer"))
        {
            Destroy(gameObject);
        }
    }
}
