using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    private Rigidbody rb;
    public Camera cam;
    Vector3 movement;
    private bool isMoving;
    private bool exited = false;
    private bool safe = true;

    public Vector3 offset;
    Vector3 oldoffset;
    public float speed;
    public float force;
    private float fov;

    // Use this for initialization
    void Start() {
        isMoving = false;
        rb = GetComponent<Rigidbody>();
        offset = (cam.transform.position - transform.position);
        //print(offset);
        fov = cam.fieldOfView;
    }

    public bool getMoving()
    {
        return isMoving;
    }
    
    // Update is called once per frame
    void FixedUpdate() {

       
        
        if (Input.GetKeyDown("space"))
        {
            rb.AddForce(offset.normalized * force);
            print(safe);
            if (safe)
            {
                oldoffset = offset;
                exited = true;
                //print("exit");
                isMoving = true;
                StartCoroutine(offslerp(isMoving));
            }
        }


    }

    void OnTriggerExit(Collider other)
    {
        
        
    }
    void OnTriggerEnter(Collider other)
    {
        Vector3 newpos = new Vector3();
        if ((int) rb.velocity.x < 0)
        {
            print(other.transform.position.x);
            newpos.Set(other.transform.position.x + 2.5f, rb.position.y, rb.position.z);
        }
        else if ((int)rb.velocity.x > 0)
        {
            newpos.Set(other.transform.position.x - 2.5f, rb.position.y, rb.position.z);
        }
        else if ((int)rb.velocity.y < 0)
        {
            newpos.Set(rb.position.x, other.transform.position.y + 2.5f, rb.position.z);
        }
        else if ((int)rb.velocity.y > 0)
        {
            newpos.Set(rb.position.x, other.transform.position.y - 2.5f, rb.position.z);
        }
        else if ((int)rb.velocity.z < 0)
        {
            newpos.Set(rb.position.x, rb.position.y, other.transform.position.z + 2.5f);
        }
        else if ((int)rb.velocity.z > 0)
        {
            newpos.Set(rb.position.x, rb.position.y, other.transform.position.z - 2.5f);
        }

        
        if (exited) { rb.position = newpos; cam.transform.position = newpos + oldoffset; }
        if (exited)
        {
            //print(oldoffset);
            
            rb.velocity = new Vector3(0, 0, 0);
            isMoving = false;
            StartCoroutine(offslerp(isMoving));

        }
    }


    // animates the offset so 
    public IEnumerator offslerp(bool inout)
    {
       // Vector3 startingPosition;
        Vector3 finalPosition;
        float camview;
        if (inout)
        {
            //startingPosition = oldoffset;
            finalPosition = oldoffset * -5;
            camview = 40;
        }
        else
        {
            //startingPosition = -5*oldoffset;
            finalPosition = oldoffset;
            camview = -40;
        }
        safe = false;
        float elapsedTime = 0.0f;
        while (elapsedTime < .3f)
        {
            //print(offset);
            elapsedTime += Time.deltaTime;
            offset = Vector3.Lerp(offset, finalPosition, (elapsedTime / .3f));
            cam.fieldOfView += camview * (Time.deltaTime / .3f);
            yield return new WaitForEndOfFrame();
        }
        if (inout)
        {
            cam.fieldOfView = fov + 40;
        }
        else
        {
            cam.fieldOfView = fov;
        }
        offset = finalPosition;
        safe = true;
        yield return 0;
    }
}
