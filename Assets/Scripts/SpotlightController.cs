using UnityEngine;
using System.Collections;

public class SpotlightController : MonoBehaviour {

    public GameObject player;
    public Camera cam;
	// Use this for initialization
	void Start () {
        transform.position = player.transform.position;
        transform.rotation = cam.transform.rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = player.transform.position;
        transform.rotation = cam.transform.rotation;
    }
}
