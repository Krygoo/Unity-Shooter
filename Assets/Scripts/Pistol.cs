using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 class Pistol : Weapon
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
    string name = "Pistol";
    GameObject particle;
    ParticleSystem emission;

    public override void SetActive(bool active)
    {
        isUnlocked = active;
    }

    public override bool Getactive()
    {
        return isUnlocked;
    }

    private void Start()
    {
        particle = transform.Find("Particle System").gameObject;
        emission = particle.GetComponent<ParticleSystem>();
    }


    public override void Shoot()
    {
        emission.Emit(1); 
  
    }
        public override string GetName()
    {
        return name; 

    }
        public override int GetDamage()
    {
        return damage;
    }
    public override AudioClip GetShootSound()
    {
        return shootSound;
    }


}
