using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class AnimalBase : MonoBehaviour {

    // maxHp =  is the maximum healthnumbers the enemy can have
    // hp = is the chagnes of healthnumbers
    // EnemeySlider = is how the player can se how mutch hes hits have affected the enemy
    // damage = how mutch damage the animal gets from the players hits.s

    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private Slider EnemySlider;
    [SerializeField] private float damage;
    [SerializeField] private AudioClip attack;
    [SerializeField] private float lookRadius = 10f;
    private Vector3 respawnLocation;
    private AudioSource source;
    [SerializeField] private bool chasing;
    private bool hitInProcess = false;
    private GameObject playerTarget;
    private Transform target;
    private NavMeshAgent agent;

    /// <summary>
    /// Constructor for an instance of AnimalBase.
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
    public AnimalBase(float damage, float hp, float maxHp, AudioClip attack, AudioSource source, Vector3 respawnLocation, 
            float lookRadius, bool chasing, bool hitInProcess, GameObject playerTarget, Transform target, NavMeshAgent agent)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        this.damage = damage;
        this.attack = attack;
        this.source = source;
        this.respawnLocation = respawnLocation;
        this.lookRadius = lookRadius;
        this.chasing = chasing;
        this.hitInProcess = hitInProcess;
        this.playerTarget = playerTarget;
        this.target = target;
        this.agent = agent;
    }

    /// <summary>
    /// Get audio source on Awake()
    /// </summary>
    private void Awake()
    {
        source = GetComponent<AudioSource>();

    }

    /// <summary>
    /// Play the attack sound
    /// </summary>
    public void AttackSounds()
    {
        source.PlayOneShot(attack, 1F);
    }

    // Properties 
    public float Damages
    {
        get { return damage; }
        set { this.damage = value; }
    }

    public float Hp
    {
        get { return hp; }
        set { this.hp = value; }
    }

    public float MaxHp
    {
        get { return maxHp; }
        set { this.maxHp = value; }
    }

    public Vector3 RespawnLocation
    {
        get { return respawnLocation; }
        set { this.respawnLocation = value; }
    }

    public float LookRadius
    {
        get { return lookRadius; }
        set { this.lookRadius = value; }
    }

    public bool Chasing 
    {
        get { return chasing; }
        set { this.chasing = value; }
    }

    public bool HitInProcess
    {
        get { return hitInProcess; }
        set { this.hitInProcess = value; }
    }
    
    public GameObject PlayerTarget
    {
        get { return playerTarget; }
        set { this.playerTarget = value; }
    }
    
    public Transform Target
    {
        get { return target; }
        set { this.target = value; }
    }

    public NavMeshAgent Agent
    {
        get { return agent; }
        set { this.agent = value; }
    }


    /// <summary>
    /// Play attack sounds, attack and regulate attack frequency.
    /// </summary>
	public IEnumerator HitCooldown() {
        AttackSounds();
        HitInProcess = true;
        Player Target_player = PlayerTarget.transform.GetComponent<Player>();
        yield return new WaitForSeconds(2);
	    float distance = Vector3.Distance(Target.position, transform.position);
        if (distance <= LookRadius) {
            Target_player.Damage(Damages);
        }
        HitInProcess = false;
    }
    
    /// <summary>
    ///  Counting this for the slidervalue what equals the life the enemey have left.
    /// </summary>
    /// <returns>hp</returns>
    public virtual float GetInfo()
    {
        return hp / this.maxHp;
    }
    
    /// <summary>
    /// damage method for the animal, so player can attack it
    /// </summary>
    public void Damage (float dmg)
    {
        Hp -= dmg;
        UpdateHealthBar();
    }

    /// <summary>
    ///  Updating new values to the health bar
    /// </summary>
    public void UpdateHealthBar()
    {
        EnemySlider.value = GetInfo();
    }
    
    /// <summary>
    ///  Draw a sphere Gizmo in the editor to indicate enemy look radius.
    /// </summary>
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
   

}
