using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] GameObject thingToFollow;
    void Start()
    {
        transform.position = new Vector3(thingToFollow.transform.position.x, thingToFollow.transform.position.y, transform.position.z);
    }

 
    void Update()
    {
        transform.position = new Vector3(thingToFollow.transform.position.x, thingToFollow.transform.position.y, transform.position.z);
    }
}
