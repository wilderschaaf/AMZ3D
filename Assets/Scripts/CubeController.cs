using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

    public GameObject player;
    Animator spincontroller;
    PlayerScript p;

	// Use this for initialization
	void Start () {
        spincontroller = GetComponent<Animator>();
        p = player.GetComponent<PlayerScript>();
        float randintro = Random.value;
        spincontroller.Play("DefAnim", 0, randintro);
    }
	
	// Update is called once per frame
	void Update () {
        

        Vector3 noffs = p.getOffset().normalized;
        if(noffs== (transform.position - player.transform.position).normalized)
        {
            
            if(noffs == new Vector3(1, 0, 0) || noffs == new Vector3(-1, 0, 0))
            {
                spincontroller.SetBool("spotonX", true);
                spincontroller.SetBool("spotonY", false);
                spincontroller.SetBool("spotonZ", false);
            }
            else if (noffs == new Vector3(0, 1, 0) || noffs == new Vector3(0, -1, 0))
            {
                spincontroller.SetBool("spotonY", true);
                spincontroller.SetBool("spotonX", false);
                spincontroller.SetBool("spotonZ", false);
            }
            else if (noffs == new Vector3(0, 0, 1) || noffs == new Vector3(0, 0, -1))
            {
                spincontroller.SetBool("spotonZ", true);
                spincontroller.SetBool("spotonX", false);
                spincontroller.SetBool("spotonY", false);
            }

        }
        else
        {
            spincontroller.SetBool("spotonX", false);
            spincontroller.SetBool("spotonY", false);
            spincontroller.SetBool("spotonZ", false);
        }
	}
}
