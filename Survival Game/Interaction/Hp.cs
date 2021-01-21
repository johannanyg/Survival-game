using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for plants to take there health functions from
/// </summary>
public class HealthBase : MonoBehaviour {

    
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;


    /// <summary>
    /// Basic health things for the plants
    /// </summary>
    /// <param name="hp"> healt value </param>
    /// <param name="maxHp">max health value to hp.</param>
    public HealthBase(float hp, float maxHp)
    {
        this.hp = hp;
        this.maxHp = maxHp;
    }

    /// <summary>
    /// Properties Set a value to Hp and return it 
    /// </summary>
    /// <returns>HP value</returns>
    public float Hp
    {
        get { return hp; }
        set { this.hp = value; }
    }

    /// <summary>
    /// Properties Set a value to maxHp and return it 
    /// </summary>
    /// <returns>Max HP value</returns>
    public float MaxHp
    {
        get { return maxHp; }
        set { this.maxHp = value; }
    }
    /// <summary>
    /// Returns the value how mutch percentage hp that has gone down.
    /// </summary>
    /// <returns>Current HP divided by max HP</returns>
    public virtual float GetInfo()
    {
        return this.hp / this.maxHp;
    }


}
