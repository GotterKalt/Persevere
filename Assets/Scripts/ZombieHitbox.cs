using UnityEngine;

public class ZombieHitbox : MonoBehaviour
{
    public enum HitType { Head }
    public HitType Head;
    public ZombieAI zombie;

    public void ApplyDamage()
    {
        
     zombie.TakeDamage(999);
        
    }
}
