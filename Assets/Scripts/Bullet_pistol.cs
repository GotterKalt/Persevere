using UnityEngine;

public class Bullet_pistol : MonoBehaviour
{
    public float speed = 350f;
    public float lifetime = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
        ZombieHitbox headHitbox = collision.GetComponent<ZombieHitbox>();
        if (headHitbox != null)
        {
            headHitbox.ApplyDamage();
            Destroy(gameObject);
            return;
        }

        // pataikymas į kūną
        ZombieHitbox_body bodyHitbox = collision.GetComponent<ZombieHitbox_body>();
        if (bodyHitbox != null)
        {
            bodyHitbox.ApplyDamage();
            Destroy(gameObject);
            return;
        }
    }
}
