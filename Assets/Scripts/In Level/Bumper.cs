using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the cylindrical 'bumper' objects throughout the levels.
public class Bumper : MonoBehaviour
{
    //The values of the explosion are serialized to allow customisation for different bumpers.
    [SerializeField] private float force;
    [SerializeField] private float radius;
    [SerializeField] private float upForce;
    [SerializeField] private int score;

    private void OnCollisionEnter(Collision other) {
        StartCoroutine(Bounce(other));
    }

    //When the ball collides with the bumper, it launches the ball away using explosive force.
    //It also uses a lerp to change its scale for a visual indicator.
    private IEnumerator Bounce(Collision other) {
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.AddExplosionForce(force, transform.position, radius, upForce);
            GetComponent<MeshCollider>().isTrigger = true; // to prevent the bumper from clipping into the ball while expanding.
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + score);
            GetComponent<AudioSource>().Play();
            float time = 0.2f;
            while (time < 1) {
                float perc = Easing.Bounce.Out(time);
                perc = Easing.PP(perc);
                float lerpFloat = Mathn.Lerp(0.5f, 1.5f, perc);
                transform.localScale = new Vector3(lerpFloat, 0.5f, lerpFloat);
                time += Time.deltaTime;
                if (time >= 0.3f) {
                    GetComponent<MeshCollider>().isTrigger = false;
                }
                yield return null;
            }
        }
        yield return null;
    }
}
