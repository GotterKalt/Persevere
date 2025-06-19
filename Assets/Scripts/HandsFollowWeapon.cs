using UnityEngine;

public class HandsFollowWeapon : MonoBehaviour
{
    [SerializeField] private Transform handLeft;
    [SerializeField] private Transform handRight;

    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform rightHandTarget; // 

    void LateUpdate()
    {
        // Kairė ranka "prilimpa" prie taško ant ginklo
        if (handLeft != null && leftHandTarget != null)
        {
            handLeft.position = leftHandTarget.position;
            handLeft.rotation = leftHandTarget.rotation;
        }

        // Dešinė ranka 
        if (handRight != null && rightHandTarget != null)
        {
            handRight.position = rightHandTarget.position;
            handRight.rotation = rightHandTarget.rotation;
        }
    }
}
