using System.Runtime.CompilerServices;
using UnityEngine;

public class FixedCamera : MonoBehaviour {
    public Rect Rect;
    private Camera _camera;

    private void Awake() {
        _camera = GetComponent<Camera>();
    }

    private void Update() {
        float targetRatio = Rect.width / Rect.height;
        float ratio = Screen.width / (float)Screen.height;
        if (ratio > targetRatio) {
            // height driven
            _camera.orthographicSize = Rect.width / 2 * targetRatio;
        } else {
            // width driven
            _camera.orthographicSize = Rect.height / 2 / ratio;
        }
        transform.position = new Vector3(Rect.xMin, Rect.yMin, transform.position.z);
    }
}