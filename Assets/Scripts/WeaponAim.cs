using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    public Transform weapon;             // Ссылка на оружие
    public Transform rightArm;           // Правая рука (кость)
    public Transform leftArm;            // Левая рука (кость)
    public Transform rightHandTarget;    // Цель для правой руки на оружии
    public Transform leftHandTarget;     // Цель для левой руки на оружии

    public float rotationSpeed = 10f;
    public float minDistance = 0.5f;

    private Camera cam;
    private Transform player;
    private bool facingRight = true;

    void Start()
    {
        cam = Camera.main;
        player = transform.root;
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 toMouse = mouseWorldPos - weapon.position;
            float angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;

            // Флип игрока
            if (mouseWorldPos.x < player.position.x && facingRight)
                Flip(false);
            else if (mouseWorldPos.x > player.position.x && !facingRight)
                Flip(true);

            if (toMouse.magnitude < minDistance) return;

            if (!facingRight)
                angle += 180f;

            angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

            if (weapon != null)
                weapon.rotation = Quaternion.Lerp(weapon.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Повернуть правую руку к таргету
            if (rightArm != null && rightHandTarget != null)
            {
                Vector3 dir = rightHandTarget.position - rightArm.position;
                float armAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rightArm.rotation = Quaternion.Euler(0, 0, armAngle);
            }

            // Повернуть левую руку к таргету
            if (leftArm != null && leftHandTarget != null)
            {
                Vector3 dir = leftHandTarget.position - leftArm.position;
                float armAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                leftArm.rotation = Quaternion.Euler(0, 0, armAngle);
            }
        }
    }

    void Flip(bool lookRight)
    {
        facingRight = lookRight;
        Vector3 scale = player.localScale;
        scale.x = lookRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        player.localScale = scale;
    }
}
