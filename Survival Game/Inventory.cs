using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DISCLAIMER: This class is called inventory, but it only handles player attacking 
// related functionality for now.

public class Inventory : MonoBehaviour
{

    private GameObject target, plantTarget, SnakeTarget, WolfTarget;
    private GameObject inventory, player;
    private float playerDamage;
    private float range;
    /* -- Tino's old code --
    RaycastHit ray;
    int layerMask = 9;
    string temp;
    private bool canPick = true;
    private bool canDrop = true;
    private bool waitActive = false;
    int slotCount = 0;
    GameObject pickedItem;
    public List<GameObject> itemlist;
    */
    
    /// <summary>
    /// Initialize player damage and player GameObject upon entering the game.
    /// </summary>
    void Start()
    {
        range = 1.1f;
        playerDamage = 250f;
        player = GameObject.Find("Player");
    }

    /// <summary>
    /// Check if "r" is pressed on each frame, and call the player attack method if so.
    /// </summary>
    void Update()
    {
        /*
        //luo säteen, katsoo onko edessä "Item" tagillä varustettu objecti, laittaa sen pois päältä ja siirtää sen gameobject "Inventory"
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * ray.distance, Color.blue);
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward), out ray, 1, layerMask) && Input.GetKey("e"))
        {
            if (ray.collider.gameObject.tag == "Item" && canPick && !waitActive)
            {
                
                //Debug.Log(ray.collider.gameObject.name);
                pickedItem = ray.collider.gameObject;
                
                pickedItem.SetActive(false);
                pickedItem.transform.parent = inventory.transform;
                canPick = false;
                StartCoroutine(Wait());
                itemlist.Add(pickedItem);
                slotCount++;
                //Destroy(pickedItem);
            } 
        } else if (Input.GetKeyDown("1") && itemlist[0] != null)
        {
            DropSlot(0);
            slotCount--;
        }

        else if (Input.GetKeyDown("2") && itemlist[1] != null)
        {
            DropSlot(1);
            slotCount--;
        }

        else if (Input.GetKeyDown("3") && itemlist[2] != null)
        {
            DropSlot(2);
            slotCount--;
        }

        else if (Input.GetKeyDown("4") && itemlist[3] != null)
        {
            DropSlot(3);
            slotCount--;
        }
        
        /*if (ray.collider.gameObject.tag == "Bear") {
            if (Input.GetKeyDown("r")) {
                //ray.collider.gameObject.Damage();
            }

        }*/

        if (Input.GetKeyDown("r")) {
            GetRayTarget();
        }
    }

    /// <summary>
    /// Shoot 3 rays and perform a damaging method on the first one to hit a proper target.
    /// </summary>
    public void GetRayTarget() {
        // Initialize a List of Vector3s to offset ray position.
        List<Vector3> directionList = new List<Vector3> {
            new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, -1, 0)
        };

        RaycastHit rayHit;

        // Add one of the Vector3s to the original raycast Vector3 position to offset it in each round.
        // Call a damaging method on the object hit if found, and break immediately after the first hit.
        for (int i = 0; i <= directionList.Count; i++) {
            if (Physics.Raycast(player.transform.position + new Vector3(0, 0.5f, 0), player.transform.forward + directionList[i], out rayHit, range)) {
                if (rayHit.transform.gameObject.CompareTag("Bear")) {
                    Bearcontroller bearTarget = rayHit.transform.GetComponent<Bearcontroller>();
                    Debug.Log(bearTarget.transform.name);
                    bearTarget.Damage(playerDamage);
                    break;
                } else if (rayHit.transform.gameObject.CompareTag("PLANT")) {
                    Plants plantTarget = rayHit.transform.GetComponent<Plants>();
                    Debug.Log(plantTarget.transform.name);
                    plantTarget.Damage(playerDamage);
                    break;
                } else if (rayHit.transform.gameObject.CompareTag("SNAKE")) {
                    Snake SnakeTarget = rayHit.transform.GetComponent<Snake>();
                    Debug.Log(SnakeTarget.transform.name);
                    SnakeTarget.Damage(playerDamage);
                    break;
                } else if (rayHit.transform.gameObject.CompareTag("WOLF")) {
                    Wolf WolfTarget = rayHit.transform.GetComponent<Wolf>();
                    Debug.Log(WolfTarget.transform.name);
                    WolfTarget.Damage(playerDamage);
                    break;
                }
            }
        }
    }
    
    /* -- Tino's old code --
    public void DropSlot(int slot)
    {
        GameObject empty;
        empty = Instantiate(itemlist[slot] as GameObject, player.transform.position + player.transform.forward, Quaternion.identity);
        pickedItem.transform.position = player.transform.position;
        empty.SetActive(true);
        itemlist.RemoveAt(slot);
    }

    //odottaa 2 sekunttia että voi nostaa uudestaan
    IEnumerator Wait()
    {
        waitActive = true;
        yield return new WaitForSeconds(2.0f);
        canPick = true;
        waitActive = false;
    }
    */

    /// <summary>
    /// Draw gizmos indicating player's attack range
    /// </summary>
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Debug.DrawRay(transform.position + (new Vector3(0, 0.5f, 0)), transform.forward + (new Vector3(0, 0, 0)) * 1.4f, Color.red);
        Debug.DrawRay(transform.position + (new Vector3(0, 0, 0)), transform.forward + (new Vector3(0, 0, 0)) * 1.4f, Color.red);
        Debug.DrawRay(transform.position + (new Vector3(0, 1, 0)), transform.forward + (new Vector3(0, 0, 0)) * 1.4f, Color.red);
    }
}
