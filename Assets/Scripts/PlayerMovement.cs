using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //private float BallThrowPower = 100f;
    public float MovementSpeed = 70f;
    private float PlayerInteractDistance = 3f;
    public float PlayerThrowForce = 20f;
    public float JumpVertForce = 800f;
    public static float MouseSens = 100f;
    public float GravityScale = -9800f;

    public int NumberofInverntorySlots = 2;

    Vector3 Gravity;
    public GameObject Player;
    public GameObject PickupObject;


    public Sprite HotBarSlotSprite;

    private List<GameObject> HolsterObjectPockets = new List<GameObject>();
    private List<GameObject> HotBarSlotsImages = new List<GameObject>();
    private List<Vector3> OrgObjScales = new List<Vector3>();
    float DistanceFromHoldPoint;

    public GameObject LookPoint;
    public GameObject HoldPoint;
    private Rigidbody PlayerRB;

    private Transform PlayerTrans;
    float xRotation = 0f;
    public Transform GroundCheck;
    private float Checkarea = 0.49f;
    public LayerMask GroundLayer;
    bool IsOnGround;
    public bool isHoldingObject;
    bool LiftedObjectGroundedAgain;
    private List<GameObject> LiftedObjects = new List<GameObject>();
    public GameObject TennisBallPrefab;

    // Start is called before the first frame update
    void Start()
    {

        LookPoint = GameObject.Find("PlayerLookPoint");
        HoldPoint = GameObject.Find("PlayerHoldPoint");


        PlayerTrans = Player.GetComponent<Transform>();
        PlayerRB = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        for (int i =  0; i < NumberofInverntorySlots; i++)
        {
            int SlotxPos;
            SlotxPos = 624 + (i * 55);
            HolsterObjectPockets.Add(null);
            HotBarSlotsImages.Add(new GameObject());
            OrgObjScales.Add(new Vector3());
            HotBarSlotsImages[i].name = "HotBarSlot:" + i;
            HotBarSlotsImages[i].AddComponent<Image>();
            HotBarSlotsImages[i].transform.SetParent(GameObject.Find("HotBar").transform);
            HotBarSlotsImages[i].GetComponent<Image>().sprite = HotBarSlotSprite;
            HotBarSlotsImages[i].transform.localScale = new Vector3(1f, 1f, 1f);
            HotBarSlotsImages[i].GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);
            HotBarSlotsImages[i].GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
            HotBarSlotsImages[i].GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            HotBarSlotsImages[i].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(SlotxPos, 35, 0);

        }


    }

    // Update is called once per frame
    void Update()
    {
        IsOnGround = Physics.CheckSphere(GroundCheck.position, Checkarea, GroundLayer);

        if (IsOnGround == true && Gravity.y < 0f)
        {
            Gravity.y = -100f;
        }

        //Mouse Looking
        float mouseX = Input.GetAxisRaw("Mouse X") * MouseSens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * MouseSens * Time.deltaTime;
        //Rotate Player Body around Y axis  left/right
        PlayerTrans.Rotate(Vector3.up * mouseX);
        //Rotate Camera around X axis   up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        LookPoint.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //Jump
        if (IsOnGround == true && Input.GetKeyDown("space"))
        {
             FindObjectOfType<AudioManagerScript>().Play("jump");
            Gravity.y = Mathf.Sqrt(JumpVertForce * -2 * GravityScale);
        }

        Gravity.y += GravityScale * Time.deltaTime;
        PlayerRB.AddForce(Gravity * Time.deltaTime);






        //Interact with objects
        if (Input.GetKeyDown("e") && isHoldingObject == false)
        {
            InteractWithObject();
        }
        else if (Input.GetKeyDown("e") && isHoldingObject == true && PickupObject != null)
        {
            PickupObject.transform.position = LookPoint.transform.position + LookPoint.transform.forward * 1.5f;
            isHoldingObject = false;
        }

        //Object Joint to HoldPoint
        if (PickupObject != null)
        {
            DistanceFromHoldPoint = Vector3.Distance(PickupObject.transform.position, HoldPoint.transform.position);
        }

        if (DistanceFromHoldPoint >= 2f)
        {
            isHoldingObject = false;
        }
        else
        {
            Collider[] hitcols = Physics.OverlapSphere(HoldPoint.transform.position, 0.1f);
            foreach (Collider col in hitcols)
            {
                
                if (col.gameObject.layer == 9)
                {
                    if (DistanceFromHoldPoint >= 0.2f)
                    {
                        isHoldingObject = false;
                    }
                }
            }
        }
        
        if(LiftedObjects.Count > 0)
        {
            for(int i = 0 ; i < LiftedObjects.Count ; i++)
            {
                Debug.Log(i.ToString() + "'st object's name is : "  + LiftedObjects[i].name);
                Collider[] hitcols = Physics.OverlapBox(LiftedObjects[i].transform.position , LiftedObjects[i].transform.localScale / 2 , Quaternion.Euler(LiftedObjects[i].transform.localEulerAngles));
                foreach(Collider col in hitcols)
                {
                    if(col.gameObject.layer == 9)
                    {
                        LiftedObjects[i].layer = 0;
                        LiftedObjects.Remove(LiftedObjects[i]);
                    }
                
                }
            }
          
        }
        if (isHoldingObject == true && PickupObject != null)
        {
            // HoldPoint.GetComponent<FixedJoint>().connectedBody = PickupObject.GetComponent<Rigidbody>();
           // Debug.Log(this.gameObject.name + " " + "   " + "The Current distance between the object and holdpoint is : " + DistanceFromHoldPoint);
            if (Input.GetMouseButton(0))
            {
                HoldPoint.GetComponent<FixedJoint>().connectedBody = null;
                PickupObject.transform.position = LookPoint.transform.position + LookPoint.transform.forward * 1.5f;
                PickupObject.GetComponent<Rigidbody>().velocity = (LookPoint.transform.forward * PlayerThrowForce);
                if (DistanceFromHoldPoint >= 0.2)
                {
                    isHoldingObject = false;
                }

            }
        }
        else if (isHoldingObject == false && PickupObject != null)
        {
            HoldPoint.GetComponent<FixedJoint>().connectedBody = null;
            PickupObject.GetComponent<Rigidbody>().useGravity = true;
            Physics.IgnoreCollision(PickupObject.GetComponent<Collider>(), Player.GetComponent<Collider>(), false);
            PickupObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            LiftedObjects.Add(PickupObject);
            PickupObject = null;
        }

        if (Input.GetKeyDown("1"))
        {
            HolsterObjectInPocket(0);
        }
        if (Input.GetKeyDown("2"))
        {
            HolsterObjectInPocket(1);
        }
        if (Input.GetKeyDown("3"))
        {
            HolsterObjectInPocket(2);
        }
        if (Input.GetKeyDown("4"))
        {
            HolsterObjectInPocket(3);
        }
        if (Input.GetKeyDown("5"))
        {
            HolsterObjectInPocket(4);
        }
        if (Input.GetKeyDown("6"))
        {
            HolsterObjectInPocket(5);
        }
        if (Input.GetKeyDown("7"))
        {
            HolsterObjectInPocket(6);
        }
        if (Input.GetKeyDown("8"))
        {
            HolsterObjectInPocket(7);
        }
        if (Input.GetKeyDown("9"))
        {
            HolsterObjectInPocket(8);
        }
        if (Input.GetKeyDown("0"))
        {
            HolsterObjectInPocket(9);
        }




        //Shoot a Ball
        /*if(Input.GetMouseButtonDown(0) && isHoldingObject != true)
        {

          GameObject BallInstance = Instantiate(TennisBallPrefab , HoldPoint.transform.position , HoldPoint.transform.rotation);
          BallInstance.AddComponent<Rigidbody>();
          BallInstance.GetComponent<Rigidbody>().velocity = HoldPoint.transform.forward * BallThrowPower;


        }*/


    }



    void FixedUpdate()
    {
        //Movement

        float XInput = Input.GetAxisRaw("Horizontal");
        float ZInput = Input.GetAxisRaw("Vertical");

        Vector3 XMove = transform.right * XInput;
        Vector3 ZMove = transform.forward * ZInput;

        PlayerRB.AddForce(XMove * MovementSpeed + ZMove * MovementSpeed);

    }








    void HolsterObjectInPocket(int SelectedPocket)
    {

        GameObject tempobject = null;
        GameObject HolsterPoint = HotBarSlotsImages[SelectedPocket];
        tempobject = HolsterObjectPockets[SelectedPocket];
        HolsterObjectPockets[SelectedPocket] = PickupObject;
        PickupObject = tempobject;


        if (PickupObject != null)
        {
            PickupObject.GetComponent<Collider>().enabled = true;
            PickupObject.GetComponent<Rigidbody>().isKinematic = false;
            PickupObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            PickupObject.transform.SetParent(GameObject.Find("DynamicEnviorment").transform);
            PickupObject.transform.position = HoldPoint.transform.position;
            PickupObject.transform.localScale = OrgObjScales[SelectedPocket];
            Physics.IgnoreCollision(PickupObject.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);
            HoldPoint.GetComponent<FixedJoint>().connectedBody = PickupObject.GetComponent<Rigidbody>();
            PickupObject.layer = 8;
        }




        if (HolsterObjectPockets[SelectedPocket] != null)
        {
            HolsterObjectPockets[SelectedPocket].GetComponent<Collider>().enabled = false;
            HolsterObjectPockets[SelectedPocket].GetComponent<Rigidbody>().isKinematic = true;
            HolsterObjectPockets[SelectedPocket].GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            OrgObjScales[SelectedPocket] = HolsterObjectPockets[SelectedPocket].transform.localScale;
            HolsterObjectPockets[SelectedPocket].transform.position = HolsterPoint.transform.position;
            HolsterObjectPockets[SelectedPocket].transform.SetParent(HolsterPoint.transform);
            Vector3 ShrinkFac = new Vector3();
            for (int i = 0; i < 100; i++)
            {
                ShrinkFac = HolsterObjectPockets[SelectedPocket].transform.localScale / i;
                
                if (ShrinkFac.x < 20f && ShrinkFac.y < 20f && ShrinkFac.z < 20f)
                {
                    break;
                }
            }

            HolsterObjectPockets[SelectedPocket].transform.localScale = ShrinkFac;
            FindObjectOfType<AudioManagerScript>().Play("HolsterObjectSound");
            isHoldingObject = false;
            HolsterObjectPockets[SelectedPocket].layer = 5;
        }

        if (PickupObject != null)
        {
            isHoldingObject = true;
        }


    }
    void InteractWithObject()
    {
        RaycastHit HitInfo;
        Debug.DrawRay(LookPoint.transform.position, LookPoint.transform.TransformDirection(Vector3.forward) * PlayerInteractDistance, Color.yellow, 10f * Time.deltaTime);
        if (Physics.Raycast(LookPoint.transform.position, LookPoint.transform.TransformDirection(Vector3.forward), out HitInfo, PlayerInteractDistance))
        {
            
            if (HitInfo.collider.tag == "Pickupable")
            {
                PickupObject = GameObject.Find(HitInfo.collider.name);
                isHoldingObject = true;
                PickupObject.GetComponent<Rigidbody>().useGravity = false;
                PickupObject.GetComponent<Rigidbody>().detectCollisions = true;
                PickupObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                PickupObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                Physics.IgnoreCollision(PickupObject.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);
                PickupObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
                PickupObject.layer = 8;
                PickupObject.transform.position = Vector3.MoveTowards(PickupObject.transform.position, HoldPoint.transform.position, 10f);
                HoldPoint.GetComponent<FixedJoint>().connectedBody = PickupObject.GetComponent<Rigidbody>();
            }


            /*
                        if (HitInfo.collider.tag == "Interactable")
                        {

                        }*/
        }
    }
}

