using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//ensures the class is serialized and visible
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    private void Update()
    {
        while (Input.GetKey("space") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //tells unity to instantiate this object as a game object
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();//plays audio
        }
    }

    private void FixedUpdate()
    {
        //Grabs the input from the player - default axis preset in the input manager
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //Apply the input to the player game object - non realistic physics behaviour
        //Address rigidbody component
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().velocity = movement * speed;//velocity uses a vector 3 value, direction we are travelling + how fast as vector and its magnitude

        //Set player position back to edge of game area
        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        //tilt the ship using the velocity on the x axis
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }
}
