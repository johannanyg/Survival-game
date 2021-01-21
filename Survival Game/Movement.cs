using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
/// <summary>
    /// Tinos made class about animation and player movment.
    /// </summary>

public class Movement : MonoBehaviour
{

    
    private Vector3 jump;
    [SerializeField] private float force = 3.0f;

    private bool grounded;
    private Rigidbody rb;
    private Animator animation;


    /// <summary>
    ///  Setting up values to rb , animation and jump. 
    /// </summary>
    /// <param name="rb"> Marking rb wiht a Rigetbody component </param>
    ///  <param name="animation"> Same but with animatior  </param>
    ///   <param name="jump"> if jump is happening then we go forward a little bit  </param>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animation = GetComponent<Animator>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    /// <summary>
    /// Animation for moving. And then also screen moving.
    /// </summary>
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(jump * force, ForceMode.Impulse);
            animation.Play("Jump");
        }
        else if (Input.GetAxis("Vertical") > 0 && grounded)
        {
            animation.Play("Walking");
        } else if(Input.GetAxis("Vertical") == 0 && grounded)
        {
            animation.Play("Idle" );
        } else if(Input.GetAxis("Vertical") < 0 && grounded)
        {
            animation.Play("WalkingBackward");
        }
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }

    /// <summary>
    /// if the player is not on the ground grounded = false if player touches the ground then it is true . 
    /// </summary>
    void OnCollisionStay()
    {
        grounded = true;
    }

    /// <summary>
    /// -
    /// </summary>
    void OnCollisionExit()
    {
        grounded = false;
    } 
}
