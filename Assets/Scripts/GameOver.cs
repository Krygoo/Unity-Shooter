using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GameOver : MonoBehaviour
{

    [SerializeField] AudioClip gameOver; // dzwiek konca gry
    AudioSource audioSource; //komponent odpowiadajacy za dzwieki
    SceneLoader sceneLoader; // referencja do sceneLoadera
    bool escPressed = false; 
    
    // Start jest wywoływany przed pierwszym Update, inicjuje zmienne, przydziela do nich komponenty oraz wywołuje metodę odtwarzającą dźwięk końca gry.
    void Start()
    {
        //referencje 
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(gameOver);
        sceneLoader = FindObjectOfType<SceneLoader>();

    }

    // Update jest wywoływany co każdą klatkę. Sprawdza, czy został wciśnięty klawisz escape by wyjść do głównego menu
    private void Update()
    {
        //jesli przycisk  esc wcisniety przechodzi do ekranu startowego
        if (CrossPlatformInputManager.GetButtonDown("Cancel") && !escPressed)
        { 
            sceneLoader.ExitToMainMenu();
            escPressed = true;
        }
    }


}
