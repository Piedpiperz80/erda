using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Abilities
    public float Agility;
    public float Constitution;
    public float Stamina;
    public float Strength;
    public float Charisma;
    public float Creativity;
    public float Intelligence;
    public float Perception;
    public float Willpower;

    // Current stamina
    public float CurrentStamina;

    //Current health
    public float CurrentHealth;

    // Skills
    public float Lockpicking; // Primary: Agility, Secondary: Intelligence, Perception
    public float Speed; // Primary: Agility & Strength
    public float Sprint; // Primary: Strength Secondary Agility

    // Skill Caps
    public float LockpickingCap;
    public float SpeedCap;
    public float SprintCap;

    protected virtual void Awake()
    {     
        // Initialize abilities
        Agility = Random.Range(25, 101);
        Constitution = Random.Range(25, 101);
        Stamina = Random.Range(25, 101);
        Strength = Random.Range(25, 101);
        Charisma = Random.Range(1, 101);
        Creativity = Random.Range(1, 101);
        Intelligence = Random.Range(1, 101);
        Perception = Random.Range(1, 101);
        Willpower = Random.Range(1, 101);

        // Initialize skill caps based on abilities
        LockpickingCap = 0.5f * Agility + 0.25f * (Intelligence + Perception);
        SpeedCap = 0.5f * (Agility + Strength);
        SprintCap = 0.75f * Strength + 0.25f * Agility;

        // Initialize skills
        Lockpicking = Random.Range(1, 101);
        Speed = Random.Range(SpeedCap, 101);
        Sprint = Random.Range(SprintCap, 101);

        CurrentStamina = Stamina; // Set current stamina to max stamina
        CurrentHealth = Constitution; // Set current health to max constitution
    }

    protected virtual void Update()
    {
        

        // Ensure abilities do not exceed 100
        Agility = Mathf.Min(Agility, 100);
        Constitution = Mathf.Min(Constitution, 100);
        Strength = Mathf.Min(Strength, 100);
        Charisma = Mathf.Min(Charisma, 100);
        Creativity = Mathf.Min(Creativity, 100);
        Intelligence = Mathf.Min(Intelligence, 100);
        Perception = Mathf.Min(Perception, 100);
        Willpower = Mathf.Min(Willpower, 100);

        // Ensure skills do not exceed their caps
        Lockpicking = Mathf.Min(Lockpicking, LockpickingCap);
        Speed = Mathf.Min(Speed, SpeedCap);
        Sprint = Mathf.Min(Sprint, SprintCap);
    }
}
