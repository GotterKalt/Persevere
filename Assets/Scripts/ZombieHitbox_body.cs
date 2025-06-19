using UnityEngine;

public class ZombieHitbox_body : MonoBehaviour
{
    public enum HitType {Body }
    public HitType Body;
    public ZombieAI zombie;

    public void ApplyDamage()
    {
       
        zombie.TakeDamage(20);
    }
}