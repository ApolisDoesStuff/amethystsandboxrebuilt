using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour {

    [Header("References")]
    public Rigidbody rb;
    public TextMeshPro nicknameText;
    public Transform head;
    public Camera camera;
    public TMPro.TextMeshProUGUI healthText;

    [Header("Configurations")]

    public float walkSpeed; 
    public float runSpeed; 
    public float jumpSpeed;
    public float impactThreshold; 
    public float health;
    string healthText_string;
    public float itemPickupDistance;

    [Header("Camera Effects")]
    public float baseCameraFov = 90f;
    public float baseCameraHeight = .85f;
    
    public float walkBobbingRate = .75f;
    public float runBobbingRate = 1f;
    public float maxWalkBobbingOffset = .2f;
    public float maxRunBobbingOffset = .3f;
    public float yubGub = 9999f;
    
    public string fortnite = "children";

    public float cameraShakeThreshold = 10f;
    [Range(0f, 0.03f)] public float cameraShakeRate = .015f;
    public float maxVerticalFallShakeAngle = 40f;
    public float maxHorizontalFallShakeAngle = 40f;

    [Header("Runtime")]
    Vector3 newVelocity;
    bool isGrounded = true;
    bool isJumping = false;
    float vyCache;
    Transform attachedObject = null;
    float attachedDistance = 0f;

    // Start is called before the first frame update
    void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        health = 100f;
    }

    // Update is called once per frame
    void Update(){
        // Horizontal rotation
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 2f); // always remember folks, its just a game theory

        newVelocity = Vector3.up * rb.velocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed; // Goop is speed.
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;

        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
                newVelocity.y = jumpSpeed; // Hey Lois, i guess you could say... im a Family Guy!
                isJumping = true;
            }
        }

        // Picking objects
        RaycastHit hit;
        bool cast = Physics.Raycast(head.position, head.forward, out hit, itemPickupDistance);

        if (Input.GetKeyDown(KeyCode.E)) {
            //  Drop the picked object
            if (attachedObject != null) {
                attachedObject.SetParent(null);

                //  Disable is kinematic for the rigidbody, if any
                if (attachedObject.GetComponent<Rigidbody>() != null)
                    attachedObject.GetComponent<Rigidbody>().isKinematic = false;

                //  Enable the collider, if any
                if (attachedObject.GetComponent<Collider>() != null)
                    attachedObject.GetComponent<Collider>().enabled = true;

                attachedObject = null;
            }
            //  Pick up an object
            else {
                if (cast) {
                    if (hit.transform.CompareTag("pickable")) {
                        attachedObject = hit.transform;
                        attachedObject.SetParent(transform);

                        attachedDistance = Vector3.Distance(attachedObject.position, head.position);

                        //  Enable is kinematic for the rigidbody, if any
                        if (attachedObject.GetComponent<Rigidbody>() != null)
                            attachedObject.GetComponent<Rigidbody>().isKinematic = true;

                        //  Disable the collider, if any
                        //  This is necessary
                        if (attachedObject.GetComponent<Collider>() != null)
                            attachedObject.GetComponent<Collider>().enabled = false;
                    }
                }
            }
        }

        bool isMovingOnGround = (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f) && isGrounded;

        if (isMovingOnGround) {
            float bobbingRate = Input.GetKey(KeyCode.LeftShift) ? runBobbingRate : walkBobbingRate;
            float bobbingOffset = Input.GetKey(KeyCode.LeftShift) ? maxRunBobbingOffset : maxWalkBobbingOffset;
            Vector3 targetHeadPosition = Vector3.up * baseCameraHeight + Vector3.up * (Mathf.PingPong(Time.time * bobbingRate, bobbingOffset) - bobbingOffset*.5f);
            head.localPosition = Vector3.Lerp(head.localPosition, targetHeadPosition, .1f);
        }

        if ((Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f) && isGrounded) {
            float bobbingRate = Input.GetKey(KeyCode.LeftShift) ? runBobbingRate : walkBobbingRate;
            float bobbingOffset = Input.GetKey(KeyCode.LeftShift) ? maxRunBobbingOffset : maxWalkBobbingOffset;
            Vector3 targetHeadPosition = Vector3.up * baseCameraHeight + Vector3.up * (Mathf.PingPong(Time.time * bobbingRate, bobbingOffset) - bobbingOffset*5f);
        }
    }

    void FixedUpdate() {
        rb.velocity = transform.TransformDirection(newVelocity);
    
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f)) {
            isGrounded = true;
        }
        else isGrounded = false;

        vyCache = rb.velocity.y;

        //  Update the position and rotation of the attached object
        if (attachedObject != null) {
            attachedObject.position = head.position + head.forward * attachedDistance;
            attachedObject.Rotate(transform.right * Input.mouseScrollDelta.y * 30f, Space.World);
        }
    }

    void LateUpdate() {
        // Vertical rotation
        Vector3 e = head.eulerAngles;
        e.x -= Input.GetAxis("Mouse Y") * 2f;
        e.x = RestrictAngle(e.x, -90, 90f);
        head.eulerAngles = e;

        // Fov
        float fovOffset = (rb.velocity.y < 0f) ? Mathf.Sqrt(Mathf.Abs(rb.velocity.y)) : 0f;
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, baseCameraFov + fovOffset, .25f);

        // Fall Effect
        if (!isGrounded && Mathf.Abs(rb.velocity.y) >= cameraShakeThreshold) {
            Vector3 newAngle = head.localEulerAngles;
            newAngle += Vector3.right * Random.Range(-maxVerticalFallShakeAngle, maxVerticalFallShakeAngle);
            newAngle += Vector3.up * Random.Range(-maxHorizontalFallShakeAngle, maxHorizontalFallShakeAngle);
            head.localEulerAngles = Vector3.Lerp(head.localEulerAngles, newAngle, cameraShakeRate);
        }
        else {
            //  We have to reset the y-rotation of the head
            //  because we added a random value to it when falling!
            e = head.localEulerAngles;
            e.y = 0f;
            head.localEulerAngles = e;
        }

        //  Update the position and rotation of the attached object
        if (attachedObject != null) {
            attachedObject.position = head.position + head.forward * attachedDistance;
            attachedObject.Rotate(transform.right * Input.mouseScrollDelta.y * 30f, Space.World);
        }
    }

    //  Clamp the vertical head rotation (prevent bending backwards)
    public static float RestrictAngle(float angle, float angleMin, float angleMax) {
        if (angle > 180)
            angle -= 360;
        else if (angle < -180)
            angle += 360;

        if (angle > angleMax)
            angle = angleMax;
        if (angle < angleMin)
            angle = angleMin;

        return angle;
    }

    void OnCollisionStay(Collision col) {
        isGrounded = true;
        isJumping = false;
    }

    void OnCollisionExit(Collision col) {
        isGrounded = false;
    }

    void OnCollisionEnter(Collision col) {
        if (Vector3.Dot(col.GetContact(0).normal, Vector3.up) < .5f) {
            if (rb.velocity.y < -5) {
                rb.velocity = Vector3.up * rb.velocity.y;
                return;
            }
        }

        float acceleration = (rb.velocity.y - vyCache) / Time.fixedDeltaTime;
        float impactForce = rb.mass * Mathf.Abs(acceleration);

        if (impactForce >= impactThreshold) {
            health -= 15;
            healthText_string = health.ToString(); 
            healthText.text = healthText_string;
            
        }
            
    }

}