using UnityEngine;

public class BoneLookAtMouse : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform arm_right;   // Dešinė ranka
    [SerializeField] private Transform arm_left;    // Kairė ranka

    [SerializeField] private float rotateSpeed = 10f;

    private Camera mainCam;
    private Transform playerRoot;
    private Vector3 originalScale;
    private bool facingRight = true;

    void Start()
    {
        mainCam = Camera.main;
        playerRoot = transform;
        originalScale = playerRoot.localScale;
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 directionToMouse = mouseWorldPos - playerRoot.position;

            // Flipas (žiūrėjimo kryptis)
            if (directionToMouse.x > 0 && !facingRight)
            {
                facingRight = true;
                playerRoot.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (directionToMouse.x < 0 && facingRight)
            {
                facingRight = false;
                playerRoot.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }

            // Visada sukame rankas ir galvą, bet apskaičiuojame kampą pagal žiūrėjimo pusę
            RotateHead(head, mouseWorldPos);
            RotateArm(arm_right, mouseWorldPos);
            RotateArm(arm_left, mouseWorldPos);
        }
    }

    void RotateHead(Transform bone, Vector3 target)
    {
        float angle = CalculateLookAngle(bone, target);
        angle = Mathf.Clamp(angle, -60f, 60f);
        ApplyRotation(bone, angle);
    }

    void RotateArm(Transform bone, Vector3 target)
    {
        float angle = CalculateLookAngle(bone, target);
        angle = Mathf.Clamp(angle, -100f, 100f);
        ApplyRotation(bone, angle);
    }

    float CalculateLookAngle(Transform bone, Vector3 target)
    {
        Vector3 dir = target - bone.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (!facingRight)
            angle += 180f;

        angle = Mathf.Repeat(angle + 180f, 360f) - 180f;
        return angle;
    }

    void ApplyRotation(Transform bone, float targetAngle)
    {
        float currentZ = bone.eulerAngles.z;
        float smoothAngle = Mathf.LerpAngle(currentZ, targetAngle, Time.deltaTime * rotateSpeed);
        bone.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
    }
}
