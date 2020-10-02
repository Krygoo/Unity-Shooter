using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Weapon : MonoBehaviour
{
    public abstract void Shoot(); //Funkcja odpowiadająca za strzał
    public abstract string GetName(); // Zwraca nazwe broni
    public abstract int GetDamage(); // Zwraca obrazenia broni
    public abstract void SetActive(bool active); //gdy bron jest aktywna pojawia sie jej model
    public abstract bool Getactive(); //Zwaraca aktualny stan
    public abstract AudioClip GetShootSound(); //Zwraca dzwiek strzalu broni

    
}
