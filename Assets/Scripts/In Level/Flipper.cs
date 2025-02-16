using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the red 'flippers' that the player controls in all levels.
//These objects have hinge joints on them, so adding explosive force creates the 'flip' effect desired.
public class Flipper : MonoBehaviour
{

    public void ExternalFlip() {
        StartCoroutine(Flip());
    }

    private IEnumerator Flip() {
        GetComponent<MeshCollider>().material.bounciness = 0.5f;
        Vector3 position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
        GetComponent<Rigidbody>().AddExplosionForce(9000, position, 0);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.2f);
        GetComponent<MeshCollider>().material.bounciness = 0.1f;
    }
}
