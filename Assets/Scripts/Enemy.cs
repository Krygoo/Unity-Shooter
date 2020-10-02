using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// health  - punkty zdrowia zycia w danym momencie
    /// MaxHealth - maksymalny poziom zdrowia
    /// expierience - doswiadczenie ktore otrzyma gracz po zlikwidowaniu wroga
    /// pickup - obiekt upuszczany po smierci
    /// particleObject - efekt czasteczkowy ilustrujacy smierc potwora
    /// iSDead - informuje czy poziom zycia jest rowny 0
    /// deathSFX -  dzwiek smierci potwora
    /// audioSource - komponent odtwarzajacy dzwiek
    /// player - uchwyt gracza potrzebny do nawigacji agenta
    /// manager - obiekt zarzadzajacy falami wrogow
    /// particle - system emisji efektow 
    /// </summary>
    public float health = 10;
    [SerializeField] int MaxHealth = 100;
    [SerializeField] float speed = 5;
    [SerializeField] int expierience = 200;
    [SerializeField] GameObject[] pickup;
    [SerializeField] GameObject deathParticleObject;
    [SerializeField] GameObject hitParticleObject;
    [SerializeField] bool isDead = false;
    [SerializeField] AudioClip deathSFX;

    AudioSource audioSource;
    Player player;
    EnemyManager manager;
    ParticleSystem deathParticle;
    ParticleSystem hitParticle;
    OptionsStorage options;

    // Awake jest wywoływany przed pierwszym Update. Przydziela zmiennym obiekty i komponenty
    void Awake()
    {
        options = FindObjectOfType<OptionsStorage>();
        player = FindObjectOfType<Player>();
        manager = FindObjectOfType<EnemyManager>();
        deathParticle = deathParticleObject.GetComponent<ParticleSystem>();
        hitParticle = hitParticleObject.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        health = health + options.GetDifficulty() * 20;
        speed = speed + (float)options.GetDifficulty();
    }

    //Przy kolizji z cząsteczką (pocisk gracza) wywołuje metodę Hit
    private void OnParticleCollision(GameObject other)
    {
        float dmg = player.GetCurrentWeaponDamage() * player.GetDamageModifier();
        if (options.GetBlood())
            hitParticle.Emit(25);
        Hit(dmg);
    }

    //Zadaje obrażenia przeciwnikowi
    public void Hit(float damage)
    {

        health -= damage;
        print("Enemy hit for  "+  player.GetCurrentWeaponDamage() + "Health:" + health);
        if(health<= 0&&!isDead)
        {
            Die();
        }
            
    }

    //Śmierć przeciwnika. Dodaje graczowi doświadczenie, odtwarza dźwięk śmierci, wyłącza renderer i kolizje przeciwnika, losuje, czy zostanie 
    //stworzony przedmiot do podniesienia i jeśli tak, to jaki i go tworzy, emituje cząsteczki, niszczy obiekt przeciwnika po dwóch sekundach i wywołuje funkcje dekrementującą licznik przeciwników
    private void Die()
    {
        isDead = true;
        player.AddExp(expierience);
        audioSource.PlayOneShot(deathSFX);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        if (UnityEngine.Random.Range(0, 3) == 0)
            Instantiate(pickup[UnityEngine.Random.Range(0,3)], new Vector3(transform.position.x, 0.5f, transform.position.z), transform.rotation);
        deathParticle.Emit(100);
        Destroy(gameObject, 2f);
        manager.decreaseEnemiesLeft();
    }

    //Zwraca informację, czy przeciwnik jest martwy
    public bool IsDead()
    {
        return isDead;
    }

    public float getSpeed()
    {
        return speed;
    }
}
