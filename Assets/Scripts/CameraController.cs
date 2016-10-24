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
    float mx, my, mx2, my2;
    Quaternion v;
    private float yrot, xrot;
    bool horRight = false, horLeft = false, vertUp = false, vertDown = false;
    float st = .4f;

    private bool rotated = false;
    private bool yup = false, ydown = false;

    private float time;

    // Use this for initialization
    void Start()
    {
        yrot = 0;
        xrot = 0;
        p = player.GetComponent<PlayerScript>();
        targetRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        print(transform.rotation);
        StartCoroutine(look());
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = player.transform.position + p.getOffset();

        if (!p.lockturn && p.safe)
        {

            if (horRight && !rotated && !(ydown || yup))
            {
                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 60, transform.rotation.eulerAngles.z);
                StartCoroutine(rotateTo(st, false, false, false, 0 , 90));
                rotated = true;
                time = Time.time;
                //yrot += 90;

            }
            else if (horLeft && !rotated && !(ydown || yup))
            {

                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 60, transform.rotation.eulerAngles.z);
                StartCoroutine(rotateTo(st, true, false, false, 0, -90));
                rotated = true;
                time = Time.time;
                //yrot -= 90;

            }
            else if (vertUp && !rotated && !yup)
            {
                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x - 60, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                //xrot += 90;
                if (!ydown)
                {
                    oldoffset = p.getOffset();
                    yup = true;
                }
                else
                {
                    ydown = false;
                }




                StartCoroutine(rotateTo(st, false, true, true, 90, 0));
                rotated = true;
                time = Time.time;

            }
            else if (vertDown && !rotated && !ydown)
            {
                targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + 60, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                //xrot -= 90;
                if (!yup)
                {
                    oldoffset = p.getOffset();
                    ydown = true;
                }
                else
                {
                    yup = false;
                }



                StartCoroutine(rotateTo(st, false, false, true, -90, 0));
                rotated = true;
                time = Time.time;

            }

        }
        else
        {
            horRight = horLeft = vertUp = vertDown = false;
        }
        if (Time.time - time > st)
        {
            rotated = false;
        }
    }

    public bool getRot()
    {
        return rotated;
    }
    public float[] xyRots()
    {
        float[] outa = new float[2];
        outa[0] = xrot;
        outa[1] = yrot;
        return outa;
    }

    public IEnumerator look()
    {

        while (true)
        {
            mx2 = (50 * (Input.mousePosition.x - Screen.width / 2) / (Screen.width / 2));

            print(mx2);
            if (mx2 >= 30 && horRight == false)
            {
                horRight = true;
            }
            else if (mx2 <= -30 && horLeft == false)
            {
                horLeft = true;
            }
            my2 = 40 * (Input.mousePosition.y - Screen.height / 2) / (Screen.height / 2);
            if (my2 >= 30 && vertUp == false)
            {
                vertUp = true;
            }
            else if (my2 <= -30 && vertDown == false)
            {
                vertDown = true;
            }


            v.eulerAngles = new Vector3(-my2 - xrot, mx2 + yrot, 0);
            if (xrot != 0)
            {
                if ((xrot > 0 && my2 > 0) || (xrot < 0 && my2 < 0))
                {
                    my2 = 0;
                }
                v.eulerAngles = new Vector3(-my2 - xrot, yrot, -mx2);
            }

            transform.rotation = v;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator rotateTo(float time, bool left, bool gUp, bool vert, float xdest, float ydest)
    {

        float elapsedTime = 0.0f;
        Quaternion startingRotation = transform.rotation;
        Vector3 startingPosition = p.getOffset();
        Vector3 finalPosition;
        if (left)
        {
            finalPosition = new Vector3(-p.getOffset().z, p.getOffset().y, p.getOffset().x);
        }
        else
        {
            finalPosition = new Vector3(p.getOffset().z, p.getOffset().y, -p.getOffset().x);
        }
        if (vert)
        {
            if (gUp && yup)
            {
                finalPosition = new Vector3(0, p.getOffset().magnitude, 0);
            }
            else if ((gUp && !yup) || (!gUp && !ydown))
            {
                finalPosition = oldoffset;
            }
            else
            {
                finalPosition = new Vector3(0, -p.getOffset().magnitude, 0);
            }
        }
        float oy = yrot;
        float ox = xrot;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            // Rotation
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, (elapsedTime / time));
            yrot += (Time.deltaTime / time) * ydest;
            xrot += (Time.deltaTime / time) * xdest;

            // Position
            p.setOffset(Vector3.Lerp(startingPosition, finalPosition, (elapsedTime / time)));



            yield return new WaitForEndOfFrame();
        }
        yrot = oy + ydest;
        xrot = ox + xdest;
        horRight = horLeft = vertUp = vertDown = false;
        yield return 0;
    }



}
