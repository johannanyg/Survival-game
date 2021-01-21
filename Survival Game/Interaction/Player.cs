using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip jump;
    private AudioSource source;

    [SerializeField] private float maxHealth;           // The maximum value of the health bar.
    [SerializeField] private float damagedHealth;       // Changing health value (damage lvl ) 
    [SerializeField] private Slider healthBar;          // Slider what is representing how mutch health is left from the maxHealth 
                                                        // - if health bar fully green maxHealth is fully maximaized 
                                                        // if it has red it is damaged 

    // GameObject to show score input window. - Jesse
    private GameObject scoreInputWindow;

    // Same  thing to hunger system    
    [SerializeField] private float maxFilldUp;
    [SerializeField] private float hungry;
    [SerializeField] private Slider hungerBar;
    [SerializeField] private bool dead;

    
    // used to the time speeder method that increses the speed of getting hungry 
    private int counter = 0;
    private float speederMultiplier = 1;




    // Timer what counts how long the player have surived in the game.
    [SerializeField] private float timeCount = 0f;
    private float secound;

    // put the timer in the UI.
    [SerializeField] private Text timeText;
    [SerializeField] private float Eating = 0.25f;

    /// <summary>
    ///  Initialising Auiosource source
    /// </summary>
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        
    }

    /// <summary>
    /// Putting values to the parameters
    /// updating the healthBarvalue
    /// </summary>
    /// <param name="maxHealth">Putting value to maxhealth.</param>
    /// <param name="maxFilldUp">Putting value to maxFilldup.</param>
    ///  <param name="damageHealth"> In begining damageHealth value is same as maxHealth value </param>
    /// <param name="hungry"> In begining hungry value is same as maxFilldUp value </param>
    void Start()
    {
        // we set a value for the maximum health and maxFillup  
        maxHealth = 100f;
        maxFilldUp = 200f;

        // Marking that the damageHealth/hungry value is in beging same as maxHealth/maxfilledup
        damagedHealth = maxHealth;
        hungry = maxFilldUp;

        // we set what ever value we have in method HealtBarValue () or HungerValue() to the slider (UI)
        healthBar.value = HealthValue();
        hungerBar.value = HungerValue();

        // For activating the window later. - Jesse
        scoreInputWindow = GameObject.Find("ScoreInputWindow");
        scoreInputWindow.SetActive(false);
    }

    /// <summary>
    /// Method for damage . If player get damage it reduces from the health.
    /// (This can be seen in the inventory class)
    /// </summary>
    /// <param name="damage">Damage dealt.</param>
    public void Damage(float damage)
    {
        damagedHealth -= damage;

    }

    /// <summary>
    /// Returns the value how mutch percentage health has gone down.
    /// </summary>
    /// <returns>Current health / max health.</returns>
    public float HealthValue()
    {
        return damagedHealth / maxHealth;
    }


    /// <summary>
    /// Returns the value how mutch percentage hunger that has gone down.
    /// </summary>
    /// <returns>Hungry divided by maximum amount of hunger bar.</returns>
    public float HungerValue()
    {
        return hungry / maxFilldUp;
    }

    /// <summary>
    /// Updates methods Dies. (checks if the player health have gone all the way down)
    /// </summary>
    void Update()
    {
        Dies();
        


        if (Input.GetKeyDown("r"))
        {
            source.PlayOneShot(hit, 1F);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            source.PlayOneShot(jump, 1F);
        }
        

        // Counting secound the player survives.
        timeCount += Time.deltaTime;
        // secound = timeCount % 60;
        timeText.text = timeCount.ToString("00");



    }

    /// <summary>
    /// Updateing the starw method in fixed period of time.
    /// </summary>
    public void FixedUpdate()
    {
        Starw();
    }

    /// <summary>
    /// If damageHealth is 0 or below the player dies and game pauses and score table appears. 
    ///  If player dosent die  hp is just being updated
    ///  Activate score input and pause the game. - Jesse
    /// </summary>
    public void Dies()
    {
        if (damagedHealth <= 0)
        {
            healthBar.value = HealthValue();

            // Debug.Log("Lets change");

            if (!dead)
            {
                scoreInputWindow.SetActive(true);
                dead = true;
                Time.timeScale = 0;
             }
        }
        else
        {       
            StartCoroutine(RegenerationTime());
            // Debug.Log("Health is damaged");
            healthBar.value = HealthValue();
        }
    }


    /// <summary>
    /// If hungry is 0 or below the player dies and game pauses and score table appears. 
    ///  If player dosent die  hp is just being updated
    ///  Activate score input and pause the game. - Jesse
    /// </summary>
    public void Starw()
    {

        if (hungry <= 0)
        {
            // SceneManager.LoadScene("MainMenu");
            // Debug.Log("Lets change");
            // Activate score input and pause the game. - Jesse
            if (!dead) {
                scoreInputWindow.SetActive(true);
                dead = true;
                Time.timeScale = 0;
            }

        }
        else
        {
            StartCoroutine(HungerTime());
            hungerBar.value = HungerValue();
        }
    }


    /// <summary>
    /// The hunger bar is going faster after some intervall of time
    /// </summary>
    public void Speeder()
    {
        counter++;
        float number = 0.0001f;
        float counterMax = speederMultiplier * 2 ;

        //  Debug.Log("start value hungry :" + hungry + ", number:" + number);
        if (counter >= counterMax)
        {
            float decrease = (speederMultiplier * number);


            hungry -= decrease;


            hungerBar.value = HungerValue();
            speederMultiplier++;

         Debug.Log("Decrease value now is :" + decrease);
        }
         // Debug.Log("counter : " + counter + ", counterMax: " + counterMax +", speed multi: "+speederMultiplier+", hungerBar : :" + hungerBar.value);

    }


    
    /// <summary>
    /// WaitForSecound method what increases the  health.
    /// </summary>
    IEnumerator RegenerationTime()
    {
        damagedHealth += 0.05f;
        yield return new WaitForSecondsRealtime(1);
        if (damagedHealth > maxHealth)
        {
            damagedHealth = maxHealth;
        }

    }

    /// <summary>
    /// WaitForSecound method what decreases the hunger.
    /// </summary>
    IEnumerator HungerTime()
    {
        hungry -= 0.2f;
        Speeder();
        yield return new WaitForSecondsRealtime(1);

    }



    /// <summary>
    /// Make the player to collect the item when player gose through it ( Copied from Roll a ball (lesson 6)) 
    /// Implemented that the item we collect increases the hungrey value what was used for counting the percentage in the slider.
    /// </summary>
    /// <param name="other">Other object being collided with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick up") && hungry < maxFilldUp)
        {
            other.gameObject.SetActive(false);

            
            hungry += Eating;
            if (hungry > maxFilldUp)
            {
                hungry = maxFilldUp;
            }
        }
    }
}
