using UnityEngine;
using System.Collections;

public class Cammove : MonoBehaviour
{




    public GameObject Cammera;
    public float climbSpeed = 4;
    

    void Start()
    {
      //  Cammera = new GameObject();
    }

    void Update()
    {
     

        Vector3 zoom1 = new Vector3(0, 1, 0);
        Vector3 zoom2 = new Vector3(1, 0, 0);
        Vector3 zoom3 = new Vector3(0, 0, 1);

        if (Input.GetKey("z")) { transform.position += zoom1 * climbSpeed * Time.deltaTime; }
        if (Input.GetKey("s")) { transform.position -= zoom1 * climbSpeed * Time.deltaTime; }

        if (Input.GetKey("q")) { transform.position -= zoom2 * climbSpeed * Time.deltaTime; }
        if (Input.GetKey("d")) { transform.position += zoom2 * climbSpeed * Time.deltaTime; }

        //if (Input.GetKey("a")) { transform.position += zoom3 * climbSpeed * Time.deltaTime; }
        //if (Input.GetKey("e")) { transform.position -= zoom3 * climbSpeed * Time.deltaTime; }

        if (Input.GetKey("a")) { Cammera.GetComponent<Camera>().orthographicSize+=climbSpeed * Time.deltaTime; }
        if (Input.GetKey("e")) { Cammera.GetComponent<Camera>().orthographicSize -= climbSpeed * Time.deltaTime; }


    }
}