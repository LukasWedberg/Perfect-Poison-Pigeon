using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonController : MonoBehaviour
{
    CharacterController controller;

    Camera cam;

    Vector3 previousMoveDirection = new Vector3(.1f, .1f, .1f);

    [SerializeField]
    float moveSpeed = 2;

    [SerializeField]
    float averageSpeedMultiplier = .9f;

    [SerializeField]
    float maxMoveSpeed = 2;

    [SerializeField]
    float gravity = Physics.gravity.y;

    [SerializeField]
    float jumpHeight = -2f;

    float verticalVelocity = 0;

    [SerializeField]
    float maxEnergy = 100;

    [SerializeField]
    float currentEnergy = 100;

    [SerializeField]
    float jumpEnergyCost = 20;

    public enum PigeonState
    { 
        Biting,
        Roaming
    }

    [SerializeField]
    public PigeonState currentPigeonState = PigeonState.Roaming;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentPigeonState)
        {
            case PigeonState.Roaming:
                Vector3 forwardDirection = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;

                Vector3 rightDirection = Vector3.Scale(cam.transform.right, new Vector3(1, 0, 1)).normalized;

                Vector3 newMoveDirection = (forwardDirection * Input.GetAxisRaw("Vertical") + rightDirection * Input.GetAxisRaw("Horizontal")).normalized * moveSpeed;


                Vector3 averageMoveDirection = Vector3.Scale(newMoveDirection + previousMoveDirection, new Vector3(averageSpeedMultiplier, averageSpeedMultiplier, averageSpeedMultiplier));

                if (averageMoveDirection.magnitude > maxMoveSpeed)
                {
                    //Debug.Log("MAXC SPEED!");

                    averageMoveDirection = averageMoveDirection.normalized * maxMoveSpeed;

                }

                if (controller.isGrounded)
                {

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("Alleyoop!");
                        verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity);
                    }
                    else
                    {
                        verticalVelocity = -0.1f;
                    }
                }
                else
                {
                    //Not on the ground
                    //Debug.Log("Apparently We're not grounded!");

                    verticalVelocity += gravity * Time.deltaTime;


                    //Now we check for extra jumps. If the player has energy to spare, then we're set!

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (currentEnergy > jumpEnergyCost)
                        {

                            currentEnergy -= jumpEnergyCost;

                            Debug.Log("Alleyoop!");
                            verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity);
                        }
                        else
                        {
                            Debug.Log("We're out of jump juice!");
                        }
                    }
                }

                //Debug.Log(verticalVelocity);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, Mathf.Atan2(averageMoveDirection.x, averageMoveDirection.z) * 180 / Mathf.PI, 0), .1f);

                controller.Move(averageMoveDirection * Time.deltaTime + new Vector3(0, verticalVelocity, 0) * Time.deltaTime);

                previousMoveDirection = averageMoveDirection;


                break;

            case PigeonState.Biting:

                currentEnergy = Mathf.Min(currentEnergy + transform.parent.GetComponent<BitableObject>().nutritionalValue * Time.deltaTime, maxEnergy);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity);

                    UnBite();
                }

                break;

        }
        

    }


    public void Bite(Transform thingToBite)
    {
        if (!controller.isGrounded && verticalVelocity < .1f)
        {
            currentPigeonState = PigeonState.Biting;

            transform.parent = thingToBite;
        }

        

    }

    public void UnBite()
    {
        currentPigeonState = PigeonState.Roaming;

        transform.parent = null;
    }

}
