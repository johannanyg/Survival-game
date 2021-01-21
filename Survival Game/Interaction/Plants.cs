using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plants : HealthBase {


    
    private Vector3 respawnLocation;
    // Bool to stop respawnTimer from going crazy. -Jesse
    private bool waitActive = false;

    // Healthbar for enemy
    [SerializeField] private Slider EnemySlider;
    [SerializeField] GameObject HealthBarUI;


    // Creating a place with what we can link a specific food to specific object. 
    [SerializeField] private GameObject Food;


    private bool respawn = false;

    /// <summary>
    /// Constructur that brings values from Parent class
    /// </summary>
    /// <param name="hp">value plants health</param>
    /// <param name="maxHp">max value of hp</param>
    public Plants(float hp, float maxHp) : base(hp, maxHp)
    {

    }

    /// <summary>
    /// Putting values to MaxHp and setting it to be the maxvalue of hp
    /// Save the current GameObjects position as Vector3 for respawning purposes. - Jesse
    /// </summary>  
    void Start()
    {
        
        MaxHp = 2000f;
        Hp = MaxHp;

        // 
        respawnLocation = gameObject.transform.position;

        // Puts the value to the hpBar
        EnemySlider.value = GetInfo();
    }

    /// <summary>
    /// Method for damage . If plant get damage it reduces from the health.
    /// (This can be seen in the inventory class)
    /// </summary>
    /// <param name="damage">Damage dealt.</param>
    public void Damage(float damage)
    {
        Hp -= damage;
        
        EnemySlider.value = GetInfo();

    }

    

    /// <summary>
    ///  Update health bar and activatet the healthbar (let the player see it )
    /// </summary>
    void Update()
    {

        // Activating the healtbarview
        if (Hp < MaxHp)
        {
            HealthBarUI.SetActive(true);
        }

        // A function what regenerates the health if the damageHealth have got below maxHealth
        if (Hp < MaxHp)
        {
            // Debug.Log("Health is damaged");
            GetInfo();
            // We use the die method if the health gose below 0.
            Dies();
        }


    }

    
    /// <summary>
    ///  method for Dieing if the Plant "dies" it will drop an item and the then calls for the respwaning method respawntimer();
    /// </summary>
    public void Dies()
    {

        if (Hp <= 0)
        {
            // With this the food item is only respawnig once and not thousand times when put to update
            if (respawn == false)
            {
                Instantiate(Food, new Vector3(Random.Range(transform.position.x + 0.5f * -2, transform.position.x + 0.5f * 2),
                                                  transform.position.y+0.5f,
                                                   Random.Range(transform.position.z + 0f * -2, transform.position.z + 0.5f * 2)),
                                                  Quaternion.identity);
                respawn = true;
            }

            // Start timer upon death. - Jesse
            if (!waitActive)
            {
                StartCoroutine(respawnTimer());
            }

            // Destroy(GameObject.Find("Oak_Tree"));

        }

    }

    /// <summary>
    ///  Mathod for plant if it "dies it respawn in a diffrent place fore few secounds and then respwans back to the same position it died in.
    /// </summary>
    IEnumerator respawnTimer()
    {
        waitActive = true;
        gameObject.transform.position = (new Vector3(9999999999, 999999999, 99999999));
        yield return new WaitForSeconds(60);
        gameObject.transform.position = respawnLocation;
        Hp = MaxHp;
        HealthBarUI.SetActive(false);
        EnemySlider.value = GetInfo();
        waitActive = false;
        respawn = false;
    }
}
