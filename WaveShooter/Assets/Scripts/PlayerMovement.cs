using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 80f;

    void Update()
    {
        // vooruit
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // draaien
        float horizontal = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up * horizontal * turnSpeed * Time.deltaTime);
    }
}

