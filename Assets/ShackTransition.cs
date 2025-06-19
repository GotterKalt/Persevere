using UnityEngine;

public class ShackTransition : MonoBehaviour
{
    public GameObject outsideView;
    public GameObject insideView;

    private bool playerInside = false;
    private Player_Movement playerMovement;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<Player_Movement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerInside)
        {
            Debug.Log("Player Entered House");
            playerInside = true;
            ShowInside();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerInside)
        {
            Debug.Log("Player Left House");
            playerInside = false;
            ShowOutside();
        }
    }

    void ShowInside()
    {
        // Не переключать, если игрок лезет по лестнице
        if (playerMovement != null && playerMovement.isClimbing)
            return;

        // Включаем интерьер
        SetVisuals(insideView, true);
        SetVisuals(outsideView, false);
    }

    void ShowOutside()
    {
        // Не переключать, если игрок лезет по лестнице
        if (playerMovement != null && playerMovement.isClimbing)
            return;

        // Включаем экстерьер
        SetVisuals(insideView, false);
        SetVisuals(outsideView, true);
    }

    void SetVisuals(GameObject obj, bool state)
    {
        foreach (var sr in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = state;
        }
    }
}
