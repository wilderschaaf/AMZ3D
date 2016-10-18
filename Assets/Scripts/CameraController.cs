using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    //private Vector3 offset;
    Quaternion targetRotation;
    private Vector3 oldoffset;
    PlayerScript p;
    //private bool isMoving;

    private bool rotated = false;
    private bool yup = false, ydown = false;

    private float time;

    // Use this for initialization
    void Start()
    {
        p = player.GetComponent<PlayerScript>();
        //isMoving = false;
        targetRotation = Quaternion.Euler(new Vector3(0,0,0));

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //print(offset);
        p = player.GetComponent<PlayerScript>();
       // if (p)
       // {
            //print("wukder");
        //    isMoving = p.getMoving();
       // }
        

        transform.position = player.transform.position + p.offset;
       
        


        if (Input.GetAxis("Horizontal") > 0 && !rotated && !(ydown || yup))
        {
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
            StartCoroutine(rotateTo(.3f, false, false, false));

            rotated = true;
            time = Time.time;


        }
        else if (Input.GetAxis("Horizontal") < 0 && !rotated && !(ydown || yup))
        {

            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
            StartCoroutine(rotateTo(.3f,true, false, false));

            rotated = true;
            time = Time.time;

        }
        else if (Input.GetAxis("Vertical") > 0 && !rotated && !yup)
        {
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            if (!ydown)
            {
                oldoffset = p.offset;
                yup = true;
            }
            else
            {
                ydown = false;
            }
    
            
                

            StartCoroutine(rotateTo(.3f, false, true, true));
            rotated = true;
            time = Time.time;
            

        }
        else if (Input.GetAxis("Vertical") < 0 && !rotated && !ydown)
        {
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            if (!yup)
            {
                oldoffset = p.offset;
                ydown = true;
            }
            else
            {
                yup = false;
            }
            
                

            StartCoroutine(rotateTo(.3f, false, false, true));
            rotated = true;
            time = Time.time;
           

        }
        if (Time.time - time > .5)
        {
            rotated = false;
        }

        //if (!isMoving)
        //{
        //    offset = transform.position - player.transform.position;
        //}
        
        

    }
    public IEnumerator rotateTo(float time, bool left, bool gUp, bool vert)
    {

        float elapsedTime = 0.0f;
        Quaternion startingRotation = transform.rotation;
        Vector3 startingPosition = p.offset;
        Vector3 finalPosition;

        if (left)
        {
            finalPosition = new Vector3(-p.offset.z, p.offset.y, p.offset.x);
        }
        else
        {
            finalPosition = new Vector3(p.offset.z, p.offset.y, -p.offset.x);
        }
        if (vert)
        {
            if (gUp && yup)
            {
                finalPosition = new Vector3(0, p.offset.magnitude, 0);
            }
            else if ((gUp && !yup) || (!gUp && !ydown))
            {
                finalPosition = oldoffset;
            }
            else
            {
                finalPosition = new Vector3(0, -p.offset.magnitude, 0);
            }
        }
        //print(startingPosition);
        //print(finalPosition);
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            // Rotation
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, (elapsedTime / time));

            // Position
            p.offset = Vector3.Lerp(startingPosition, finalPosition, (elapsedTime/time));
            
            yield return new WaitForEndOfFrame();
        }

        yield return 0;
    }



}
