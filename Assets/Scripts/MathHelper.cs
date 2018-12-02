
using UnityEngine;

public class MathHelper {
    public static Vector2 RadianToVector2(float radian) {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
  
    public static Vector2 DegreeToVector2(float degree) {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static float Vector2ToRadian(Vector2 direction) {
        return Mathf.Atan2(direction.y, direction.x);
    }

    public static float Vector2ToDegrees(Vector2 direction) {
        float angle = Vector2ToRadian(direction)* Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    public static float AngleDifference(float a, float b) {
        return 180 - Mathf.Abs((Mathf.Abs(a - b) % 360.0f) - 180.0f);
    }

    public static string CountdownSeconds(float seconds) {
        if (seconds < 0) {
            seconds = 0;
        }
        int minutes = (int)(seconds / 60);
        int s = (int)(seconds % 60);
        return minutes.ToString("D2") + ":" + s.ToString("D2");
    }

    public static float TweenInQuad(float start, float end, float value) {
        return (end - start) * value * value + start;
    }
}