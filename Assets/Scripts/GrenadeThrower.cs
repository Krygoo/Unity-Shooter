using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GrenadeThrower : MonoBehaviour
{
    /// <summary>
    /// player  - referncja do obiektu gracza
    /// grenadePrefab - referencja do prefabrykatu granata
    /// </summary>
    Player player;
    [SerializeField] GameObject grenadePrefab;

    // Start jest wywoływany przed pierwszym Update, przydziela zmiennej player obiekt gracza
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update jest wywoływany co każdą klatkę. Wywołuje metodę ThrowGrenade()
    void Update()
    {
        ThrowGrenade();
    }

    // Po wciśnięciu klawisza Fire2, tworzy nowy granat i aplikuje na niego siłę
    private void ThrowGrenade()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire2"))
        {
            if (player.GetGrenades() > 0)
            {
                GameObject grenade = Instantiate(grenadePrefab, (player.transform.position+player.transform.forward), player.transform.rotation);
                Rigidbody grenadeRb = grenade.GetComponent<Rigidbody>();
                grenadeRb.AddForce(transform.forward * 10, ForceMode.Impulse);
                player.DecGrenades();
            }
        }
    }
}
