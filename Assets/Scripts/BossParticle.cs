using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossParticle : MonoBehaviour
{
    /// <summary>
    /// Pola:
    /// particle - system efektow specjalnego przeciwnika
    /// player - referencja instacji gracza
    /// </summary>
    ParticleSystem particle;
    Player player;
    // Start jest wywoływany przed pierwszym Update. Przydziela zmiennym odpowiednie obiekty i komponenty
    void Start()
    {
        player = FindObjectOfType<Player>();
        particle = GetComponent<ParticleSystem>();
    }

    // Update jest wywoływane co każdą klatkę. Jeżeli gra nie jest w stanie pauzy, to boss emituje cząsteczki (pociski).
    void Update()
    {
        if (player.GetCurrentState() == 1)
        {
            if(particle.isPaused)
            particle.Play();
        }
        else
        {
            particle.Pause();
        }
    }
}
