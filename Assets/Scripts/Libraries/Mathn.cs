using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script was taken entirely from my CT4101 Assessment, Modes of Motion
//As such, I do not wish for it to be marked outside of other scripts' use of it.
public class Mathn {
    public static float Fract(float a) {
        return a - Mathf.Floor(a);
    }

    public static float PingPong(float a, float b) {
        if (b == 0.0) {
            return 0.0f;
        } else {
            return Mathf.Abs(Fract((a - b) / (b * 2.0f)) * b * 2.0f - b);
        }
    }

    public static float Lerp(float startValue, float endValue, float t) {
        return (startValue + (endValue - startValue) * t);
    }

    public static Vector2 Vec2Lerp(Vector2 start, Vector2 end, float t) {
        return start + t * (end - start);
    }
}
