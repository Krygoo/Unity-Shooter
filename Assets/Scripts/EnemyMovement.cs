using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody rb;                             // referencja do komponentu RigidBody Potwora
    Transform playerTransform;               // referencja do komponentu transform gracza
    Player playerObject;      //  referencja do obiektu gracza
    Enemy enemyObject;        // referencja do skryptu Enemy w obiekcie gracza
    UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.

    void Awake()
    {
        //ustawienie refereencji
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerObject = playerTransform.GetComponent<Player>();
        enemyObject = GetComponent<Enemy>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>(); // agent nawigacji Unity
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        nav.speed = enemyObject.getSpeed();
    }

    void Update()
    {
        //sprawdza czy wlaczona jest pauza lub ekran ulepszen. Jesli tak to zatrzymuje wrogow
        if (playerObject.GetCurrentState() == 1)
        {
            StartNav();
            EnemyMove();
        }
        else
            StopNav();

        
    }

    private void EnemyMove() // Porusza model gracza
    {
        if (enemyObject.health > 0 && playerObject.GetPlayersHealth() > 0)
        {
            nav.SetDestination(playerTransform.position); // ustawienie kierunku, w ktorym ma poruszac sie gracz 
        }
        else
        {
            nav.enabled = false;
        }
    }

    public void StopNav() // zatrzymuje agenta nawigacji 
    {
        if(!enemyObject.IsDead())
            nav.isStopped = true;
    }
    public void StartNav() // wlacza agenta nawigacji
    {
        if (!enemyObject.IsDead())
            nav.isStopped = false;
    }

    // ponizsze funkcje  zmieniaja wlasciwosc w komponencie rigid body. Zmiana jest potrzebna aby moc zaaplikowac sile odrzutu przy wybuchach
    public void MakeKinematic()
    {
        if (!enemyObject.IsDead())
            rb.isKinematic = true;
    }
    public void MakeNonKinematic()
    {
        if (!enemyObject.IsDead())
            rb.isKinematic = false;
    }
}