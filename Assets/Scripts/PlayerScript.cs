using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private Rigidbody rb;
    public Camera cam;
    Vector3 movement;


    Vector3 offset;
    public float speed;
    public float force;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        offset = (cam.transform.position - transform.position);
    }

    
    // Update is called once per frame
    void FixedUpdate() {


        offset = (cam.transform.position - transform.position);
        if (Input.GetKeyDown("space"))
        {
            rb.AddForce(offset.normalized * force);
        }


    }

}
