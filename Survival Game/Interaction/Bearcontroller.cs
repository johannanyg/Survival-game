using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Bearcontroller : AnimalBase {
    
    // Healthbar for enemy
    [SerializeField] GameObject HealthBarUI;


    // Creating a place with what we can link a specific food to specific object. 
    [SerializeField] private GameObject Food;
    private bool Respawn = false;
    private bool deadCheck;
    private Transform childTransform;
    private BoxCollider boxCollider;
    private AudioSource audioSource;

    /// <summary>
    /// Constructor for an instance of Bearcontroller.
    /// </summary>
    /// <param name="hp">Current amount of healt points on this enemy.</param>
    /// <param name="maxHp">Maximum amount of health points on this enemy.</param>
    /// <param name="damage">Amount of damage this enemy object can hit.</param>
    /// <param name="attack">Audio clip to be player upon attacking.</param>
    /// <param name="source">Source for the attack audio clip.</param>
    /// <param name="respawnLocation">Respawn location of the enemy object.</param>
    /// <param name="lookRadius">From how far the enemy object can aggro the player.</param>
    /// <param name="chasing">Chasing status of this enemy object.</param>
    /// <param name="hitInProcess">Check to prevent hit function from looping.</param>
    /// <param name="playerTarget">Attack target GameObject.</param>
    /// <param name="target">Attack target Transform.</param>
    /// <param name="agent">This enemy object's NavMeshAgent.</param>
    public Bearcontroller(float hp, float maxHp, float damage, AudioClip attack, AudioSource source, 
            Vector3 respawnLocation, float lookRadius, bool chasing, bool hitInProcess, GameObject playerTarget, Transform target, NavMeshAgent agent) : 
        base(hp, maxHp, damage, attack, source, respawnLocation, lookRadius, chasing, hitInProcess, playerTarget, target, agent)
    {
        
    }

    /// <summary>
    /// Initialize inherited values appropriately.
    /// </summary>
    void Start () {
        MaxHp = 3500;
        Hp = MaxHp;
        Damages = 35;
	    Target = GameObject.Find("Player").transform;
        PlayerTarget = GameObject.Find("Player");
        Agent = GetComponent<NavMeshAgent>();
        LookRadius = 10.0f;
        RespawnLocation = gameObject.transform.position;
        HitInProcess = false;
        UpdateHealthBar();
        deadCheck = false;
        HealthBarUI.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        //transform.Find("BearCanvas").gameObject.SetActive(false);
        //childTransform = transform.Find("BearCanvas");
        //childTransform.gameObject.SetActive(false);
    }
	
	/// <summary>
    /// Update health bar and check for nearby player for attacks.
    /// </summary>
	void Update () {


        // Activating the healtbarview
        if (Hp < MaxHp)
        {
            HealthBarUI.SetActive(true);
        }
        float distance = Vector3.Distance(Target.position, transform.position);

        // Deactivate GameObject if hp gets to 0 or below.
        if (this.Hp <= 0) {

            // With this the food item is only respawnig once and not thousand times when put to update
            if (Respawn == false)
            {

                for (int i = 0; i < 4; i++)
                {
                    Instantiate(Food, new Vector3(Random.Range(transform.position.x + 0.5f * -2, transform.position.x + 0.5f * 2),
                                                  transform.position.y,
                                                  Random.Range(transform.position.z + 0.5f * -2, transform.position.z + 0.5f * 2)),
                                                  Quaternion.identity);

                }

                Respawn = true;
            }
           
            if (!deadCheck) {
                StartCoroutine(RespawnTimer());
            }
        }

        // Move towards player if in radious, otherwise set position to current position.
        if (distance <= LookRadius) {
            Agent.SetDestination(Target.position);
            if (!Chasing) {
                Debug.Log("started chasing");
            }
            Chasing = true;
        } 
        if (distance > LookRadius && Chasing) {
            Agent.SetDestination(Agent.transform.position);
            Debug.Log("stopped chasing");
            Chasing = false;
        }
        
        if (distance <= 1.4 && !HitInProcess) {
            StartCoroutine(HitCooldown());
        }
        if (deadCheck) {
            HealthBarUI.SetActive(false);
        }
    }

    /// <summary>
    /// Relocates the enemy for a respawn period when it is dead.
    /// </summary>
    IEnumerator RespawnTimer() {
        deadCheck = true;
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        boxCollider.enabled = false;
        audioSource.enabled = false;
        HealthBarUI.SetActive(false);
        LookRadius = 0.0f;
        transform.Find("graphics").gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        HealthBarUI.SetActive(false);
        Agent.Warp(RespawnLocation); 
        yield return new WaitForSeconds(59f);
        LookRadius = 10.0f;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.Find("graphics").gameObject.SetActive(true);
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        boxCollider.enabled = true;
        audioSource.enabled = true;
        HealthBarUI.SetActive(false);
        Hp = MaxHp;
      
        // deactivating the healtbarvalue (canvas not visible)
        UpdateHealthBar();
        Respawn = false;
        deadCheck = false;
    }
}
