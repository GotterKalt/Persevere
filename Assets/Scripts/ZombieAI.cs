using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour
{
    [Header("Movement")] // Judėjimas
    public float moveSpeed = 1.5f;                // Judėjimo greitis
    public float idleDuration = 5f;               // Kiek laiko stovi vietoje
    public float walkDuration = 2f;               // Kiek laiko vaikšto

    [Header("Health")] // Gyvybės
    public int maxHealth = 100;                   // Maksimalus gyvybių kiekis
    private int currentHealth;                    // Dabartinės gyvybės

    [Header("Flash")] // Mirksėjimas gavus žalą
    public SpriteRenderer spriteRenderer;         // Renderer'is kuriam bus keičiama spalva
    public Color hitColor = Color.red;            // Spalva kai gauna žalą
    public float flashTime = 0.2f;                // Kiek laiko išlieka raudonas

    [Header("Ragdoll")] // Ragdoll komponentai
    public Rigidbody2D[] ragdollBodies;           // Visos ragdoll dalys
    public Collider2D[] ragdollColliders;         // Visi ragdoll collideriai
    public GameObject animatedRoot;               // Objektas su Animatoriumi

    private Rigidbody2D rb;                       // Rigidbody komponentas
    private Animator anim;                        // Animatorius
    private Vector3 originalScale;                // Pradinė objekto skalė

    private bool moveLeft = true;                 // Ar eiti į kairę
    private float stateTimer;                     // Laikmatis tarp vaikščiojimo ir stovėjimo
    private bool isWalking = false;               // Ar dabar vaikšto
    private bool headshot = false;
    private Color originalColor;                  // Pradinė spalva prieš žalos efektą

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;                 // Kad nenukristų ant šono

        anim = GetComponent<Animator>();
        originalScale = transform.localScale;

        currentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>(); // Ieško pagrindinio rendererio jei nepriskirtas

        originalColor = spriteRenderer.color;

        DisableRagdoll();                         // Pradiniame būsenoje ragdoll turi būti išjungtas

        EnterIdleState();                         // Pradeda nuo stovėjimo
    }

    void Update()
    {
        if (currentHealth <= 0) return;           // Jei negyvas – nieko nedaryti

        stateTimer -= Time.deltaTime;             // Skaičiuojam laiką iki būseno pakeitimo

        if (stateTimer <= 0f)
        {
            if (isWalking) EnterIdleState();      // Baigė vaikščioti – pradeda stovėti
            else EnterWalkingState();             // Baigė stovėti – pradeda vaikščioti
        }

        if (isWalking)
        {
            float direction = moveLeft ? -1f : 1f;    // Kryptis: kairė ar dešinė
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y); // Judėjimas

            // Apverčia skalę jei reikia
            transform.localScale = new Vector3(
                moveLeft ? -Mathf.Abs(originalScale.x) : Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); // Jei stovi – X greitis 0
        }

        anim.SetBool("walk", isWalking); // Nustato animacijos būseną
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;           // Atima gyvybes
        StartCoroutine(FlashRed());        // Paleidžia mirksėjimo efektą

        if (currentHealth <= 0)
        {
            Die();                         // Jei gyvybių nebeliko – suaktyvina ragdoll
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = hitColor;               // Nustato raudoną spalvą
        yield return new WaitForSeconds(flashTime);    // Palaukia
        spriteRenderer.color = originalColor;          // Grąžina pradinę spalvą
    }

    void Die()
    {
        headshot = true;
        anim.SetBool("headshot", true);
        //this.enabled = false;
        Destroy(gameObject, 60f);
    }

    void EnterIdleState()
    {
        isWalking = false;              // Nebevaikšto
        stateTimer = idleDuration;     // Nustato kiek stovės
    }

    void EnterWalkingState()
    {
        isWalking = true;                                   // Pradeda vaikščioti
        moveLeft = Random.value > 0.5f;                     // Atsitiktinai pasirenka kryptį
        stateTimer = Random.Range(1f, walkDuration);        // Nustato kiek vaikščios
    }

    /*void ActivateRagdoll()
    {
        if (animatedRoot != null)
            animatedRoot.SetActive(false);       // Išjungia animatorinę versiją

        foreach (var body in ragdollBodies)
        {
            body.bodyType = RigidbodyType2D.Dynamic;           // Įjungia fizika
            body.simulated = true;
        }

        foreach (var col in ragdollColliders)
        {
            col.enabled = true;                  // Įjungia colliderius
        }

        rb.simulated = false;                    // Išjungia pagrindinį Rigidbody
        if (anim != null)
            anim.enabled = false;                // Išjungia Animator
    }*/

    void DisableRagdoll()
    {
        foreach (var body in ragdollBodies)
        {
            body.bodyType = RigidbodyType2D.Kinematic;            // Išjungia fizika
            body.simulated = false;
        }

        foreach (var col in ragdollColliders)
        {
            col.enabled = false;                 // Išjungia colliderius
        }
    }
}
