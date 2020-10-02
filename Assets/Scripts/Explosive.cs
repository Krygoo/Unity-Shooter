using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    /// <summary>
    /// hasExploded - ifnormuje czy ładunek wybuchł
    /// radius - zasięg eksplozji
    /// force - siła eksplozji
    /// damage - obrażenia zadawane przez eksplozje
    /// particleQuantity - liczba cząsteczek generowanych w wyniki eksplozji
    /// particleObject - obiekt reprezentujacy emitowane czasteczki
    /// audioSource - komponent odtwarzajacy dzwięki
    /// explosion - dzwiek przy eksplozji
    /// </summary>
    [SerializeField] bool hasExploded = false;
    [SerializeField] float radius = 7;
    [SerializeField] float force = 1000;
    [SerializeField] float damage = 30;
    [SerializeField] int particleQuantity = 1000;
    GameObject particleObject;
    ParticleSystem particle;
    AudioSource audioSource;
    [SerializeField] AudioClip explosion;
    // Start jest wywoływany przed pierwszym Update. Przydziela zmiennym obiekty i komponenty.    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        particleObject = transform.Find("Particle System").gameObject;
        particle = particleObject.GetComponent<ParticleSystem>();
    }
    // Metoda wywoływana przy kolizji beczki z cząsteczką (pocisk, wybuch beczki lub innego granata), wywołuje metodę explode
    private void OnParticleCollision(GameObject other)
    {
        Explode();
    }
    // Metoda odpowiedzialna za eksplozję beczki.
    public void Explode()
    {
        audioSource.PlayOneShot(explosion);     // Odtwarza dźwięk eksplozji
        particle.Emit(particleQuantity);        // Emituje cząsteczki w ilości równej wartości zminnej particleQuantity
        GetComponent<MeshRenderer>().enabled = false;   // Wyłącza renderer beczki, przez co ta po wybuchu nie jest widoczna
        GetComponent<CapsuleCollider>().enabled = false;// Wyłącza collider beczki, by po wybuchu nie kolidowała z innymi obiektami
        hasExploded = true;                     // Zapobiega kilkukrotnemu wybuchowi beczki
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);   //Tworzy sferę pobierającą wszystkie obiekty wewnątrz niej
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (rb.tag == "Enemy")          //Jeżeli obiekt w obszarze wybuchu to przeciwnik, to na jedną sekundę wyłączany jest NavMeshAgent, wartość zmiennej
                                                //isKinematic ustawiana jest na false po to, by obiekt został właściwie odrzucony przez wybuch oraz zadawane są obrażenia
                {
                    EnemyMovement enemyMovement = rb.GetComponent<EnemyMovement>();
                    enemyMovement.StopNav();
                    enemyMovement.MakeNonKinematic();
                    enemyMovement.Invoke("MakeKinematic", 1);
                    enemyMovement.Invoke("StartNav", 1);
                    Enemy enemy = rb.GetComponent<Enemy>();
                    enemy.Hit(damage);
                }
                if (rb.tag == "Player")         //Jeżeli obiekt w obszarze wybuchu to gracz, to zadawane są mu obrażenia.
                {
                    Player player = rb.GetComponent<Player>();
                    player.Hit(transform, false);
                }
                if (rb.tag == "Barrel")         //Jeżeli obiekt w obszarze wybuchu to beczka, to wywołana zostaje jej eksplozja
                {
                    Explosive explosive = rb.GetComponent<Explosive>();
                    explosive.Explode();
                }
                rb.AddExplosionForce(force, transform.position, radius);    //Dodawana jest siła odrzucająca obiekt od środka wybuchu zależnie od odległości od środka i wartości zmiennej force
            }
        }
        Destroy(gameObject, 2f);    //Po dwóch sekundach od wybuchu niszczony jest obiekt. Niszcząc obiekt od razu,
                                    //usunięte zostałyby cząsteczki po wybuchu oraz zachowanie przeciwników nie zostałoby przywrócone
    }
}
