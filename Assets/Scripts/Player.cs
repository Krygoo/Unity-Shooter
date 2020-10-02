using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    /// <summary>
    /// health - punkty życia gracza
    /// level - poziom gracza
    /// jumHeight - wyosokść skoku gracza
    /// canJump - informacja czy gracz znajduje się na ziemi
    /// grenades - liczba posiadanych granatów
    /// maxGrenades - maksymalna liczba granatów
    /// exp - liczba punktów doświadczenia
    /// weapons - tablica posiadanych broni
    /// isMoving - zmienna potrzebna do animacji, która informuje czy gracz jest w ruchu
    /// ar15Ammo,shotgunAmmo - zmienne informujące o liczbie posiadanej amunicji do broni
    /// floorMask - referencja do parametru obiektu podłoża
    /// caRayLength - długość promieni emitowanych przez kamerę w celu obrotu postaci w miejsce wskazywane przez kursor
    /// currentMaxHealthLvl, currentDamageModlvl, currentSped lvl - zmienne informujące o obecnym poziomie ulepszen gracza
    /// damageModifier - modyfkator obrazeń zadawanych przez gracza
    /// playersSpeed - predkosc poruszania sie gracza
    /// maxHealth - maksymalny poziom zdrowia
    /// NoAmmo, picku, deathSFX - dzwieki
    /// State  gameStatus- informuje o statusie rozgrywki
    /// Gui - referencja do interfejsu użytkownika
    /// upgradeMenu - referencja do obiektu odpowiadającego za ulepszenia
    /// playerRigidBody - referencja do komponentu rigidBody
    /// playerBoxColider - referncja do komponentu odpwoedzialnego za detekcję kolizji
    /// currentWeapon - obecnie używana broń
    /// statistics - referncja do obiektu przechowujacego informacje na temat statystyk gracza
    /// upgradeManager - referencja do obiektu odpowiedzialnego za ulepszanie postaci
    /// sceneLoader - referecja do obiektu odpowiadającego za zmianę poziomów
    /// </summary>
    [SerializeField] int health = 80;
    [SerializeField] int level;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] bool canJump;
    [SerializeField] int grenades;
    [SerializeField] int exp;
    [SerializeField] Weapon[] weapons;
    [SerializeField] bool isMoving = false;
    [SerializeField] bool isDead = false;


    [SerializeField] int aR15Ammo;
    [SerializeField] int shotgunAmmo;
    [SerializeField] int floorMask;
    [SerializeField] float camRayLength = 100f;

    //Upgrade Fields

    [SerializeField] int currentMaxHealthLvl = 1;
    [SerializeField] int currentDamageModlvl = 1;
    [SerializeField] int currentSpeedlvl = 1;
    [SerializeField] float damageModifier = 1;
    [SerializeField] float playerSpeed = 8f;
    [SerializeField] int maxHealth = 100;

    [SerializeField] AudioClip noAmmo;
    [SerializeField] AudioClip pickup;
    [SerializeField] AudioClip deathSFX;

    [SerializeField] Animator animator;

    enum State { Upgrade, Playing, Dead }
    State gameStatus;

    // References
    GameObject gUI;
    GameObject upgradeMenu;
    Rigidbody playerRigidBody;
    BoxCollider playerBoxColider;
    Weapon currentweapon;
    UpgradeManager upgradeManager;
    StatsManager statistics;
    AudioSource audioSource;
    SceneLoader sceneLoader;

    // Start jest wywoływany przed pierwszym Update, inicjuje zmienne, przyporządkowuje im komponenty i obiekty
    void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        gameStatus = State.Playing;
        playerRigidBody = GetComponent<Rigidbody>();
        playerBoxColider = GetComponent<BoxCollider>();
        currentweapon = weapons[0];
        var meshRenderer = currentweapon.GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        gUI = GameObject.FindGameObjectWithTag("GUI");
        upgradeMenu = GameObject.FindGameObjectWithTag("Menu");
        upgradeMenu.SetActive(false);
        upgradeManager = FindObjectOfType<UpgradeManager>();
        statistics = FindObjectOfType<StatsManager>();
        LoadStats();
        audioSource = GetComponent<AudioSource>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    // Update jest wywoływany co każdą klatkę. Jeżeli gra nie jest w stanie pauzy, wywoływane są metody Move, Turn, SwitchWeapons, Shoot i jeśli podczas się porusza,
    // wysyłany jest komunikat, by rozpocząć obracanie modelu. Natomiast jeśli gra jest w stanie pauzy wywoływana jest metoda obsługi pauzy.
    void Update()
    {
        if (gameStatus == State.Playing)
        {
            Move();
            Turn();
            SwitchWeapons();
            Shoot();
           /* if (isMoving)
            {
                SendMessage("RotateModel",isMoving);
            }*/
             
            
        }
        HandlePause();

    }

    // Metoda obsługi pauzy. Zmienia stan gry i zastępuje interfejs gry interfejsem ulepszeń i na odwrót i wywołują metody OnStartUpgradeMenu
    // lub OnExitUpgrade menu zależnie od tego, czy wchodzi, czy wychodzi się z menu
    private void HandlePause()
    {
        if (CrossPlatformInputManager.GetButtonDown("Cancel") && gameStatus == State.Playing)
        {
            gameStatus = State.Upgrade;
            gUI.SetActive(false);
            upgradeMenu.SetActive(true);
            upgradeManager.OnStartUpgradeMenu();
        }
        else if (CrossPlatformInputManager.GetButtonDown("Cancel") && gameStatus == State.Upgrade)
        {
            gameStatus = State.Playing;
            gUI.SetActive(true);
            upgradeMenu.SetActive(false);
            upgradeManager.OnExitUpgradeMenu();
        }
    }

    // Metoda odpowiedzialna za strzelanie. Odtwarza dźwięk zależnie od wybranej broni i wywołuje metodę Shoot odpowiedniej broni.
    // Ponadto zmniejsza ilość amunicji w przypadku strzelby i karabinu maszynowego
    private void Shoot()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            if (currentweapon.GetName() == "Pistol")
            {
                audioSource.PlayOneShot(currentweapon.GetShootSound(), 0.5F);
                currentweapon.Shoot();
            }

            else if (currentweapon.GetName() == "Shotgun" && shotgunAmmo > 0)
            {
                audioSource.PlayOneShot(currentweapon.GetShootSound(), 0.5F);
                currentweapon.Shoot();
                shotgunAmmo--;
            }
            else if (currentweapon.GetName() == "AR15" && aR15Ammo > 0)
            {
                audioSource.PlayOneShot(currentweapon.GetShootSound(), 0.5F);
                currentweapon.Shoot();
                aR15Ammo--;
            }
            else
            {
                audioSource.PlayOneShot(noAmmo);
            }
        }
    }

    // Metoda odpowiedzialna za zmianę broni za pomocą klawiszy 1,2,3. Przy zmianie broni zmienia się wartośc zmiennej currentweapon,
    // wyłączane są renderery wszystkich broni i aktytowany zostaje tylko renderer broni aktywnej
    private void SwitchWeapons()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (Weapon gun in weapons)
            {
                var Renderer = gun.GetComponent<MeshRenderer>();
                Renderer.enabled = false;
            }
            currentweapon = weapons[0];
            var meshRenderer = currentweapon.GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (Weapon gun in weapons)
            {
                var Renderer = gun.GetComponent<MeshRenderer>();
                Renderer.enabled = false;
            }
            currentweapon = weapons[1];
            var meshRenderer = currentweapon.GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (Weapon gun in weapons)
            {
                var Renderer = gun.GetComponent<MeshRenderer>();
                Renderer.enabled = false;
            }
            currentweapon = weapons[2];
            var meshRenderer = currentweapon.GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
        }

    }

    // Metoda odpowiedzialna za poruszanie postacią
    private void Move()
    {
      
        float moveVertical = CrossPlatformInputManager.GetAxis("Vertical");     //Odczyt wejścia odpowiedzialnego za poruszanie pionowe
        float moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal"); //Odczyt wejścia odpowiedzialnego za poruszanie poziome
        if(moveHorizontal != 0 || moveVertical != 0)    //Zmiana wartości zmiennej isMoving na true jeżeli jest wciśnięty dowolny klawisz odpowiedzialny za poruszanie graczem lub na false w przeciwnym wypadku
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("speed", playerSpeed);

        Vector3 newPosition = new Vector3(moveHorizontal, 0.0f, moveVertical); //Tworzenie wektora określającego kierunek ruchu
        newPosition = Vector3.ClampMagnitude(newPosition, 1);   //Ograniczenie długości wektora, by ruch na ukos nie był szybszy od ruchu w jednej osi

        transform.Translate(newPosition * playerSpeed * Time.deltaTime, Space.World); //Ruch obiektu zależny od wektora ruchu, wartości prędkości gracza oraz czasu od poprzedniej klatki

        //Jeżeli gracz może wykonać skok oraz wciśnięty jest odpowiedni przysisk, na gracza działa siła w kierunku do góry i wyłączana jest możliwość skoku
        if (CrossPlatformInputManager.GetButtonDown("Jump") && canJump)
        {
            playerRigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            canJump = false;
        }
    }

    //Metoda odpowiedzialna za obrót gracza. Wypuszczany jest promień z kamery w kierunku kursora myszy. Przy zderzeniu promienia z podłożem tworzony jest wektor na podstawie
    //położenia gracza oraz zderzenia promienia. Kierunek tego wektora oznacza kierunek wzroku gracza. 
    private void Turn()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidBody.MoveRotation(newRotation);
        }
    }

    //Przy kolizji z podłożem zmienna decydująca o możliwości skoku jest ustawiana jako true, a przy kolizji z przeciwnikiem jest wywoływana metoda Hit
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            canJump = true;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Hit(collision.collider.gameObject.transform, true);
        }

    }

    //Metoda wywoływana przy kolizji z triggerem, podnosi bonusy poprzez niszczenie ich i dodawaniu graczowi odpowiednich statystyk.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "First Aid")
        {
            Destroy(other.gameObject);
            AddHealth();
        }
        if (other.gameObject.tag == "Ammo")
        {

            Destroy(other.gameObject);
            AddAmmo(UnityEngine.Random.Range(5, 10), UnityEngine.Random.Range(5, 10));
            print("Ammo Called");
        }
        if (other.gameObject.tag == "Grenade")
        {
            Destroy(other.gameObject);
            AddGrenade();
            print("Grenade Called");
        }

    }

    //Wczytuje parametry gracza z StatsManagera
    private void LoadStats()
    {
        float[] stats = statistics.LoadStats();
        exp = (int)stats[0];
        maxHealth = (int)stats[1];
        damageModifier = stats[2];
        playerSpeed = stats[3];
        currentMaxHealthLvl = (int)stats[4];
        currentDamageModlvl = (int)stats[5];
        currentSpeedlvl = (int)stats[6];
        shotgunAmmo = (int)stats[7];
        aR15Ammo = (int)stats[8];
        grenades = (int)stats[9];
    }

    //Zapis parametrów do StatsManagera
    public void SaveStats()
    {
        statistics.SaveCurrentStats(exp, maxHealth, damageModifier, playerSpeed,
            currentMaxHealthLvl, currentDamageModlvl, currentSpeedlvl, shotgunAmmo, aR15Ammo, grenades);
    }

    //Zadanie obrażen graczowi i ewentualne odepchnięcie go od obiektu od którego obrażenia otrzymał
    public void Hit(Transform objTransform, bool applyForce)
    {
        health -= 20;
        if (health <= 0)
        {
            health = 0;
            if(!isDead)Die();
            isDead = true;
        }
        if(applyForce)
        playerRigidBody.AddForce(objTransform.forward * 7, ForceMode.Impulse);

    }

    //Śmierć gracza. Zmiana stanu gry na Dead, odtworzenie dźwięku śmierci, wywołanie metody EndGame po trzech sekundach i zniszczenie obiektu gracza po czterech sekundach
    private void Die()
    { 
        gameStatus = State.Dead;
        audioSource.PlayOneShot(deathSFX);
        Invoke("EndGame", 3f);
        Destroy(this.gameObject,4f);
        
    }

    //Wywoływania przy zderzeniu z cząsteczką pochodzenia innego niż broń gracza. Wywołuje metodę Hit.
    public void OnParticleCollision(GameObject other)
    {
        if (other.tag != "Weapon")
            Hit(other.transform, true);

    }

    // ======== SETTERS & GETTERS =================
    public void AddExp(int amount)
    {
        exp += amount;
    }
    public int GetCurrentWeaponDamage()
    {
        return currentweapon.GetDamage();
    }
    public int GetPlayersHealth()
    {
        return health;
    }

    public int GetPlayerExp()
    {
        return exp;
    }

    private void AddGrenade()
    {
            audioSource.PlayOneShot(pickup);
            grenades += 1;
    }

    private void AddAmmo(int ar15, int shotgun)
    {
        audioSource.PlayOneShot(pickup);
        aR15Ammo += ar15;
        shotgunAmmo += shotgun;
    }

    private void AddHealth()
    {
        if (health < maxHealth)
        {
            audioSource.PlayOneShot(pickup);
            health += 30;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public int GetCurrentState()
    {
        if (gameStatus == State.Playing) return 1;
        else if (gameStatus == State.Upgrade) return 2;
        else if (gameStatus == State.Dead) return 3;
        return 0;

    }

    public void DecGrenades()
    {
        grenades -= 1;
    }
    private void EndGame()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public int GetGrenades()
    {
        return grenades;
    }

    public int GetAR15Ammo()
    {
        return aR15Ammo;
    }
    public int GetShotgunAmmo()
    {
        return shotgunAmmo;
    }
    public float GetDamageModifier()
    {
        return damageModifier;

    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetSpeed()
    {
        return playerSpeed;
    }
    public void SetDamageModifier(float modifier)
    {
        this.damageModifier = modifier;
    }
    public void SetMaxHealth(int health)
    {
        this.maxHealth = health;
    }
    public void SetSpeed(float speed)
    {
        this.playerSpeed = speed;
    }
    public void SetExp(int exp)
    {
        this.exp = exp;
    }

    public void SetHpLVL(int lvl)
    {
        currentMaxHealthLvl = lvl;
    }

    public void SetSpeedLVL(int lvl)
    {
        currentSpeedlvl = lvl;
    }
    public void SetDamageLVL(int lvl)
    {
        currentDamageModlvl = lvl;
    }

    public int GetHpLvl()
    {
        return currentMaxHealthLvl;
    }

    public int GetSpeedLvl()
    {
        return currentSpeedlvl;

    }
    public int GetDmgLvl()
    {
        return currentDamageModlvl;
     }

}

