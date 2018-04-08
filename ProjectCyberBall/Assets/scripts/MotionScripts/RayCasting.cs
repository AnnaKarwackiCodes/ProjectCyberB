using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCasting : MonoBehaviour {

    LineRenderer line;
    RaycastHit hit;
    public Text text;
    private playerScript user;
    private MotionControllers myControls;

    public GameObject hexHL;
    private bool createHexHL;
    private GameObject myHexHL;

    private GameObject preSel;

    public GameObject enemyHL;
    private bool createEnemyHL;
    private GameObject myEnemyHL;

    //public Text txt;
    // Use this for initialization
    void Start() {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;
        user = transform.parent.transform.parent.GetComponent<playerScript>();
        myControls = user.gameObject.GetComponent<MotionControllers>();
        preSel = null;
        createHexHL = true;
    }

    // Update is called once per frame
    void Update() {

    }

    //should have a function that changes the range that the raycast is checking
    public void SelectingObj(float distance)
    {
        //Debug.Log("selecting obj");
        //drawing the line to show what it's interacting with
        line.enabled = true;
        Ray ray = new Ray(transform.position, transform.forward);

        line.SetPosition(0, ray.origin);
        line.SetPosition(1, ray.GetPoint(distance));

        if (Physics.Raycast(ray, out hit, distance))
        {
            /*
            if (hit.collider.gameObject.tag == wantedTag && Input.GetAxis("Right_Trigger") == 1.0f)
            {
                if (wantedTag == "Hex" && hit.collider.gameObject.GetComponent<Hex>().occupant != null) { return; }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>().SelectedObj = hit.collider.gameObject;
                    line.enabled = false;
                    //Input.ResetInputAxes();
                }
                
            }
            */
            switch (hit.collider.gameObject.tag)
            {
                case "Hex":
                    if (user.Action != "Move Boi")
                    {
                        myControls.HexInteraction(hit.collider.gameObject);
                        RemoveHighlight();

                    }
                    user.SelectedObj = hit.collider.gameObject;
                    break;
                case "Boi":
                    myControls.BoiInteraction(hit.collider.gameObject);
                    user.SelectedMinion = hit.collider.gameObject.GetComponent<mobBase>();
                    break;
                case "Enemy":
                    //player attack options
                    break;
                case "Info":
                    user.UseBall();
                    break;
                default:
                    Debug.LogError("What you are currently selecting is not an object with a recognizeable tag");
                    break;
            }
        }
        else
        {
            //if a menu is open and doesnt need to be, close it
            myControls.RemoveUI();
        }

    }

    public void SelectOption(float distance) {

        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, distance))
        {

            if (hit.collider.gameObject.tag == "button" && Input.GetAxis("Right_Trigger") == 1.0f) {

                Debug.Log("Menu option selected: " + hit.collider.gameObject.GetComponent<vrMenu>().buttonId);

            }

        }

    }

    public void BoiFind(float distance, string look)
    {
        Debug.Log(look);
        line.enabled = true;
        Ray ray = new Ray(transform.position, transform.forward);

        line.SetPosition(0, ray.origin);
        line.SetPosition(1, ray.GetPoint(distance));

        if (Physics.Raycast(ray, out hit, distance))
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Hex":
                    if (look == "Move")
                    {
                        if (preSel != null && preSel.gameObject.tag == "Hex" && hit.collider.gameObject.transform.position != preSel.transform.position)
                        {
                            Debug.Log("different");
                            if (!createHexHL)
                            {
                                Destroy(myHexHL);
                                createHexHL = true;
                            }
                        }
                        user.SelectedObj = hit.collider.gameObject;
                        if (createHexHL && hit.collider.gameObject.GetComponent<Hex>().occupant == null)
                        {
                            Debug.Log("here i am");
                            myHexHL = Instantiate(hexHL, (hit.collider.gameObject.transform.position + new Vector3(0, 1, 0)), new Quaternion(0, 0, 0, 0));
                            createHexHL = false;
                            preSel = hit.collider.gameObject;
                        }
                    }
                    break;
                case "Enemy":
                    if (look == "Attack")
                    {
                        Debug.Log("inside attacking enemy");
                        if (preSel != null && hit.collider.gameObject.transform.position != preSel.transform.position)
                        {
                            Debug.Log("boi");
                            if (!createEnemyHL)
                            {
                                Destroy(myEnemyHL);
                                createEnemyHL = true;
                            }
                        }
                        user.SelectedObj = hit.collider.gameObject;
                        if (createEnemyHL)
                        {
                            Debug.Log("here i am boi");
                            myEnemyHL = Instantiate(enemyHL, hit.collider.gameObject.transform.position, new Quaternion(0, 0, 0, 0));
                            createEnemyHL = false;
                            preSel = hit.collider.gameObject;
                        }
                    }
                    break;
            }
        }
        else
        {
            if (!createHexHL)
            {
                Destroy(myHexHL);
                createHexHL = true;
                //preSel = null;
            }
        }
    }

    public void SelectingMinion(float distance)
    {
        //drawing the line to show what it's interacting with
        line.enabled = true;
        Ray ray = new Ray(transform.position, transform.forward);

        line.SetPosition(0, ray.origin);
        line.SetPosition(1, ray.GetPoint(distance));

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.gameObject.tag == "Boi" && hit.collider.GetComponent<mobBase>().CanMove && Input.GetAxis("Right_Trigger") == 1.0f)
            {
                text.text = hit.collider.gameObject.name;
                GameObject.FindGameObjectWithTag("Player").GetComponent<playerScript>().SelectedMinion = hit.collider.gameObject.GetComponent<mobBase>();
                line.enabled = false;
                //Input.ResetInputAxes();

            }
            else
            {
                text.text = "no boi";
            }
        }
        else
        {
            text.text = "no boi";
        }
    }

    public bool Line
    {
        set { line.enabled = value; }
    }

    public void RemoveHighlight()
    {
        if (!createHexHL)
        {
            Destroy(myHexHL);
            createHexHL = true;
        }
        if (!createEnemyHL)
        {
            Destroy(myEnemyHL);
            createEnemyHL = true;
        }
    }
}
