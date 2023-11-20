using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int max_health = 100;
    public int current_health;
    [SerializeField] HealthBar healthBar;
    [SerializeField] DeathMenu deathMenu;

    [SerializeField] int max_experience = 50;
    [SerializeField] int max_increase = 50;
    [SerializeField] int current_experience = 0;
    [SerializeField] ExperienceBar experienceBar;
    [SerializeField] UpgradeMenu upgradeMenu;
    // [SerializeField] int Strength = 1;

    [SerializeField] GameObject startingSpell;

    public List<GameObject> spawnedSpells;

    InventoryManager inventory;
    public int spellIndex;


    private void Awake()
    {

        healthBar.SetMaxHealth(max_health);
        current_health = max_health;
        healthBar.SetHealth(current_health);

        experienceBar.SetMaxExperience(max_experience);
        current_experience = 0;
        experienceBar.SetExperience(current_experience);


        //Spawn the starting weapon

        inventory = GetComponent<InventoryManager>();

        SpawnSpell(startingSpell);

    }

    public void TakeDamage(int damage){

        // char_stats_Hp -= damage;
        // //Debug.Log("Player takes a hit.\n");
        // //Debug.Log(Hp);
        // if(char_stats_Hp < 1){
        //     Debug.Log("Player is dead.");
        // }

        current_health -= damage;
        healthBar.SetHealth(current_health);
        if (current_health <= 0)
        {
            current_health = 0;
            Debug.Log("Player is dead.");
            deathMenu.OpenDeathMenu();
        }
    }

    public void SpawnSpell(GameObject spell)
    {
        //checking if the slots are full, and returning if it is
        if(spellIndex >= inventory.spellSlots.Count -1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //Spawn the starting spell
        GameObject spawnedSpell = Instantiate(spell, transform.position, Quaternion.identity);
        spawnedSpell.transform.SetParent(transform);
        inventory.AddSpell(spellIndex, spawnedSpell.GetComponent<SpellController>());

        spellIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseManager.GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeDamage(10);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GainExperience(15);
            }
        }
    }
    void HealDamage(int health)
    {
        current_health += health;
        if (current_health > max_health)
        {
            current_health = max_health;
        }
        healthBar.SetHealth(current_health);
    }

    void GainExperience(int experience)
    {
        current_experience += experience;
        if (current_experience >= max_experience)
        {
            current_experience -= max_experience;
            max_experience += max_increase;
            experienceBar.SetMaxExperience(max_experience);
            upgradeMenu.OpenUpgradeMenu();
        }
        experienceBar.SetExperience(current_experience);
    }

    
}
