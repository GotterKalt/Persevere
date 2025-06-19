using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class weapon_pistol : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float fireRate = 0.2f;
    private float fireCooldown;

    public int magazineSize = 15;
    private int currentAmmo;
    public float reloadTime = 3f;
    private bool isReloading = false;

    public Item item;
    [Header("UI")]
    public Image image;

    private Vector3 originalScale;

    private void Start()
    {
        currentAmmo = magazineSize;
        originalScale = transform.localScale;
        InitialiseItem(item);
    }

    public void InitialiseItem(Item newItem)
    {
        //image.sprite = newItem.image;
    }

    void Update()
    {
        if(!PauseMenu.isPaused)
        {
                if (isReloading) return;

                fireCooldown -= Time.deltaTime;

                // Pasisukimas į kairę / dešinę (remiantis žaidėjo skale)
                if (transform.root.localScale.x > 0)
                {
                    // žiūrim į dešinę
                    transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                    firePoint.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    // žiūrim į kairę
                    transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                    firePoint.localRotation = Quaternion.Euler(0, 0, 180);
                }

                // Perkrovimas
                if (Input.GetKeyDown(KeyCode.R))
                {
                    StartCoroutine(Reload());
                    return;
                }

                // Šaudymas
                if (Input.GetButton("Fire1") && fireCooldown <= 0f && currentAmmo > 0)
                {
                    Shoot();
                    fireCooldown = fireRate;
                }
        }
        
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        currentAmmo--;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
    }
}
