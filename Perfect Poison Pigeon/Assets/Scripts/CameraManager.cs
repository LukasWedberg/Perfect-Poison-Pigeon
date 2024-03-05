using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    Transform target = null;

    [SerializeField]
    float heightAbovePlayer = 2;

    [SerializeField]
    float maxCameraDistance = 10;

    Quaternion targetRot;


    // Start is called before the first frame update
    void Start()
    {
        if (target == null) {
            target = GameObject.Find("PPPlayer").transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, target.position.y + heightAbovePlayer, transform.position.z);

        Vector3 originDirection = target.position - transform.position;
        
        float currentCamDist = Vector3.Distance(target.position, transform.position);

        //Debug.Log(currentCamDist);



        if ( !(currentCamDist < heightAbovePlayer + .1f))
        {
            targetRot = Quaternion.LookRotation(originDirection);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, .3f);



        if ( currentCamDist > maxCameraDistance) {
            
            transform.position = target.position + transform.forward * -maxCameraDistance;

        }



    }
}
