using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Shotgun : Weapon
{

    /// <summary>
    /// shootSound - dzwiek strzalu
    /// isUnlucked - czy bron jest obecnie uzywana
    /// name - zawiera nazwe broni
    /// parcivle - referencja do prefabrykatu efektu strzalu
    /// emission - referncja do modułu komponentu efektów cząsteczkowych
    /// </summary>
    [SerializeField] AudioClip shootSound; 
    bool isUnlocked = true;
    int damage = 5;
    string name = "Shotgun";

    //referecncje
    GameObject particle; //refrencja do systemu particli
    ParticleSystem emission; // modul emisji systemu

    public override void SetActive(bool active) // wlacza widocznosc broni
    {
        isUnlocked = active;
    }

    public override bool Getactive() // zwraca widocznosc broni
    {
        return isUnlocked;
    }

    private void Start()
    {
        //referecje
        particle = transform.Find("Particle System").gameObject;
        emission = particle.GetComponent<ParticleSystem>();
    }


    public override void Shoot() // strzela
    {
        emission.Emit(9); // emitowane jest 9 efektow na raz

    }
    public override string GetName() // zwraca nazwe broni
    {
        return name;

    }
    public override int GetDamage() // zwraca obrazenia
    {
        return damage;
    }

    public override AudioClip GetShootSound() // zwraca dzwiek broni
    {
        return shootSound;
    }


}

