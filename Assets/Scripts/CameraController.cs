using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    //private Vector3 offset;
    Quaternion targetRotation;
    private Vector3 oldoffset;
    PlayerScript p;
    Rigidbody crb;
    private Vector3 torqueVect;
    //private bool isMoving;
    float mx, my, mx2, my2;
    Quaternion v;
    float yrot, xrot;

    public bool rotated = false;
    private bool yup = false, ydown = false;

    private float time;

    // Use this for initialization
    void Start()
    {
        yrot = 0;
        xrot = 0;
        torqueVect = new Vector3(0, 0, 0);
        p = player.GetComponent<PlayerScript>();
        crb = GetComponent<Rigidbody>();
        targetRotation = Quaternion.Euler(new Vector3(0,0,0));
        
        print(transform.rotation);
        StartCoroutine(look());
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = player.transform.position + p.offset;
        //mx = Input.GetAxis("Mouse X");
        //my = Input.GetAxis("Mouse Y");

        

        //torqueVect = new Vector3(-Mathf.Cos(transform.rotation.eulerAngles.y * (Mathf.PI / 180)) * my, mx, Mathf.Sin(transform.rotation.eulerAngles.y * (Mathf.PI / 180)) * my);
        
        //StartCoroutine(look(v));
        

        if (!p.lockturn && p.safe)
        {
            
            if (Input.GetAxis("Horizontal") > 0 && !rotated && !(ydown || yup))
            {
                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
                StartCoroutine(rotateTo(.3f, false, false, false));
                rotated = true;
                time = Time.time;
                yrot += 90;
            }
            else if (Input.GetAxis("Horizontal") < 0 && !rotated && !(ydown || yup))
            {

                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
                StartCoroutine(rotateTo(.3f, true, false, false));
                rotated = true;
                time = Time.time;
                yrot -= 90;
            }
            else if (Input.GetAxis("Vertical") > 0 && !rotated && !yup)
            {
                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                xrot += 90;
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
                xrot -= 90;
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
            
        }
        if (Time.time - time > .4)
        {
            rotated = false;
        }
    }

    public IEnumerator look()
    {
        
        while (true)
        {
            mx2 = 40 * (Input.mousePosition.x - Screen.width/2)/(Screen.width/2);
            mx2 = mx2 < 40 ? mx2 : 40;
            my2 = 40 * (Input.mousePosition.y - Screen.height/2) / (Screen.height/2);
            my2 = my2 < 40 ? my2 : 40;
            print(transform.rotation.eulerAngles.y);

            v.eulerAngles = new Vector3(-my2 - xrot, mx2 + yrot, 0);
            if (xrot != 0)
            {
                if ((xrot > 0 && my2>0) || (xrot < 0 && my2 < 0))
                {
                    my2 = 0;
                }
                v.eulerAngles = new Vector3(-my2 - xrot, yrot, -mx2);
            }

            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, v ,Time.time/100);
            yield return new WaitForEndOfFrame();
        }
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
