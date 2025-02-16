using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the portals found in level 2.
//It controls their interaction with the ball (changing its position to the destination on trigger).
public class Portal : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private Vector3 tpPos;
    public static bool active = true;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ball") && active) {
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 70);
            StartCoroutine(Teleport(other.gameObject)); 
        }
    }

    private IEnumerator Teleport(GameObject other) {
        GetComponent<AudioSource>().Play();
        tpPos = destination.position;
        other.transform.position = tpPos;
        active = false;
        yield return new WaitForSeconds(0.1f);
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.AddExplosionForce(10, tpPos, 2); // helps the ball launch away a little
        yield return new WaitForSeconds(0.3f);
        active = true;
    }
}
