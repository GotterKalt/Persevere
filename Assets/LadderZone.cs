using UnityEngine;

public class LadderZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Movement>().SetInLadderZone(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player_Movement>().SetInLadderZone(false);
        }
    }
}
