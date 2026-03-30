using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializedField] public float forwardSpeed = 10f;
  [SerializedField] public float sideSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontal * sideSpeed * Time.deltaTime);
    }
   
}

internal class SerializedFieldAttribute : Attribute
{

}