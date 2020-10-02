using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    /// <summary>
    /// delay =  opóznienie wybuchu w sekundach
    /// countdown - czas, który upłynął od wyrzucenia granatu
    /// hasExploded - informacja czy ładunek już wybuchł
    /// radius - zasieg
    /// force - siła odrzutu
    /// damge - obrażenia od eksplozji
    /// particleQuantity - liczba generowanych cząsteczek przy wybuchu
    /// explosion - dzwięk wybuchu
    /// particleObject - efekt cząsteczkowy wybuchu
    /// particle - referencja do systemu efektów cząsteczkowych
    /// audioSource - referencja do systemu odtwarzania dźwięków
    /// </summary>
    [SerializeField] float delay = 3f;
    [SerializeField] float countdown;
    [SerializeField] bool hasExploded = false;
    [SerializeField] float radius = 7;
    [SerializeField] float force = 1000;
    [SerializeField] float damage = 30;
    [SerializeField] int particleQuantity = 1000;
    [SerializeField] AudioClip explosion;
    GameObject particleObject;
    ParticleSystem particle;
    AudioSource audioSource;

    // Start jest wywoływany przed pierwszym Update, inicjuje zmienne i przydziela im komponenty.
    void Start()
    {
        particleObject = transform.Find("Particle System").gameObject;
        particle = particleObject.GetComponent<ParticleSystem>();
        countdown = delay;
        audioSource = GetComponent<AudioSource>();
    }

    // Update jest wywoływany co każdą klatkę. Zmniejsza licznik odmierzający czas do wybuchu oraz wywołuje metodę explode po jego upłynięciu
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    // Metoda wywoływana przy kolizji granata z cząsteczką (pocisk, wybuch beczki lub innego granata), wywołuje metodę explode
    private void OnParticleCollision(GameObject other)
    {
        Explode();
        hasExploded = true;
    }

    // Metoda odpowiedzialna za eksplozję granatu
    void Explode()
    {
        audioSource.PlayOneShot(explosion);     // Odtwarza dźwięk eksplozji
        particle.Emit(particleQuantity);        // Emituje cząsteczki w ilości równej wartości zminnej particleQuantity
        GetComponent<MeshRenderer>().enabled = false;   // Wyłącza renderer granata, przez co ten po wybuchu nie jest widoczny
        GetComponent<SphereCollider>().enabled = false; // Wyłącza collider granata, by po wybuchu nie kolidował z innymi obiektami
        hasExploded = true;                     // Zapobiega kilkukrotnemu wybuchowi granata
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);   //Tworzy sferę pobierającą wszystkie obiekty wewnątrz niej
        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb!=null)
            {
                if(rb.tag == "Enemy")   //Jeżeli obiekt w obszarze wybuchu to przeciwnik, to na jedną sekundę wyłączany jest NavMeshAgent, wartość zmiennej
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
                if (rb.tag == "Player") //Jeżeli obiekt w obszarze wybuchu to gracz, to zadawane są mu obrażenia.
                {
                    Player player = rb.GetComponent<Player>();
                    player.Hit(transform, false);
                }
                if (rb.tag == "Barrel") //Jeżeli obiekt w obszarze wybuchu to beczka, to wywołana zostaje jej eksplozja
                {
                    Explosive explosive = rb.GetComponent<Explosive>();
                    explosive.Explode();
                }
                rb.AddExplosionForce(force, transform.position, radius); //Dodawana jest siła odrzucająca obiekt od środka wybuchu zależnie od odległości od środka i wartości zmiennej force
            }
        }
        Destroy(gameObject, 2f); //Po dwóch sekundach od wybuchu niszczony jest obiekt. Niszcząc obiekt od razu,
                                 //usunięte zostałyby cząsteczki po wybuchu oraz zachowanie przeciwników nie zostałoby przywrócone
    }
}
