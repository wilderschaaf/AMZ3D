using UnityEngine;
using System.Collections;

public class SpotlightController : MonoBehaviour {

    public GameObject player;
    public Camera cam;
    private CameraController c;
    private Quaternion rotQuat;
    private Vector3 eulangs;
	// Use this for initialization
	void Start () {
        c = cam.GetComponent<CameraController>();
        eulangs = new Vector3(0,0,0);

        rotQuat.eulerAngles = eulangs;
        transform.position = player.transform.position;
        transform.rotation = cam.transform.rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = player.transform.position;
        eulangs.y = c.xyRots()[1];
        eulangs.x = -c.xyRots()[0];
        rotQuat.eulerAngles = eulangs;
        transform.rotation = rotQuat;
    }
}
