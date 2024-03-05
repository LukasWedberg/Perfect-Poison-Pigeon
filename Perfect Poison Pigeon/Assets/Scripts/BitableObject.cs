using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class BitableObject : MonoBehaviour
{
    


    BoxCollider hitBox = null;


    [SerializeField]
    public float nutritionalValue = 10f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        hitBox = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnTriggerEnter( Collider other)
    {
        if (hitBox != null)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(other.gameObject.name);

                PigeonController pigeonScript = other.GetComponent<PigeonController>();

                pigeonScript.Bite(transform);
            }


        }
    }



}
