using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float climbSpeed = 3f;

    [SerializeField] private GameObject shackOutside;
    [SerializeField] private GameObject shackInside;


    private int normalLayer;
    private int climbingLayer;


    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool run;
    private bool grounded;

    private bool inLadderZone = false;
    public bool _isClimbing = false;
    public bool isClimbing => _isClimbing;

    private Vector3 originalScale;

    private void Awake()
    {
        if (shackOutside == null || shackInside == null)
            Debug.LogWarning("Shack references not assigned!");

        normalLayer = LayerMask.NameToLayer("Default");
        climbingLayer = LayerMask.NameToLayer("Climbing");

        // Paimti nuorodas iš Rigidbody ir Animator
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Išsaugoti originalų mastelį (kad atspindžiai veiktų teisingai)
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (inLadderZone && Input.GetKeyDown(KeyCode.E))
            {
                EnterClimb();
            }

            if (isClimbing)
            {
                float verticalInput = Input.GetAxisRaw("Vertical"); // W/S arba стрелки
                body.linearVelocity = new Vector2(0, verticalInput * climbSpeed);

                // Nustatyti animacijos parametrą
                anim.SetBool("Climb", true);

                // Grįžti prie vaikščiojimo, jei nebėra judesio (galima не ставить)
                if (!inLadderZone)
                    ExitClimb();

                return; // sustabdyti įprastą judėjimą
            }

            float horizontalInput = Input.GetAxis("Horizontal");
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            body.linearVelocity = new Vector2(horizontalInput * currentSpeed, body.linearVelocity.y);

            // Pasisukimai į kairę ir į dešinę
            if (horizontalInput > 0.01f)
            {
                // Dešinė
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (horizontalInput < -0.01f)
            {
                // Kairė
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }

            if (Input.GetKey(KeyCode.Space) && grounded) Jump();
            run = Input.GetKey(KeyCode.LeftShift);

            // Animacijos parametrai
            anim.SetBool("walk", Mathf.Abs(horizontalInput) > 0.01f && !isRunning);
            anim.SetBool("run", Mathf.Abs(horizontalInput) > 0.01f && isRunning);
            anim.SetBool("grounded", grounded);
            anim.SetBool("Climb", false); // išjungti Climb jei ne lipama
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, walkSpeed + 5f);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    // Iškviečia Ladder trigger
    public void SetInLadderZone(bool state)
    {
        inLadderZone = state;

        if (!state && isClimbing)
            ExitClimb();
    }

    private void EnterClimb()
    {

        _isClimbing = true;
        body.gravityScale = 0;
        body.linearVelocity = Vector2.zero;
        gameObject.layer = climbingLayer;
        anim.SetBool("Climb", true);
        shackOutside.SetActive(false);
        shackInside.SetActive(true);
    }

    private void ExitClimb()
    {
        _isClimbing = false;
        body.gravityScale = 1;
        body.linearVelocity = Vector2.zero;
        gameObject.layer = normalLayer;
        anim.SetBool("Climb", false);

        
    }
}
