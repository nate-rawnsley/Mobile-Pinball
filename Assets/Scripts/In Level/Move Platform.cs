using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the moving platforms found in level 3.
//It controls their movement (using lerps) as well as their interactions with the ball.
public class MovePlatform : MonoBehaviour
{
    [SerializeField] private bool moved;
    [SerializeField] private float speed = 1;
    private bool moving;
    private Vector3 startPos;

    private void Awake() {
        startPos = transform.position;
    }

    private void Update() {
        if (!moving) {
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move() {
        moving = true;
        float time = 0f;
        while (time < 1) {
            float perc = Easing.Sine.InOut(time);
            float lerpFloat;
            if (moved) {
                lerpFloat = Mathn.Lerp(-14f, 16f, perc);
            } else {
                lerpFloat = Mathn.Lerp(16f, -14f, perc);
            }
            transform.position = new Vector3(lerpFloat, startPos.y, startPos.z);
            time += Time.deltaTime * speed;
            yield return null;
        }
        moved = !moved;
        moving = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ball")) {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 5);
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 pos = collision.GetContact(0).point;
            rb.AddExplosionForce(100f, pos, 50);
        }
    }
}
