using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour {
    public Transform Follow;
    private Camera _camera;

    private void Awake() {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate() {
        var bounds = Level.Instance.Bounds;
        var halfSize = GetHalfSize();
        if (halfSize.y > bounds.extents.y) {
            _camera.orthographicSize = bounds.extents.y;
            Debug.Log("Changing camera size to _camera.orthographicSize since Y " + halfSize.y + " > " + bounds.extents.y);
            halfSize = GetHalfSize();
        }

        if (halfSize.x > bounds.extents.x) {
            _camera.orthographicSize = bounds.extents.x / (Screen.width / (float) Screen.height);
            Debug.Log("Changing camera size to _camera.orthographicSize since X " + halfSize.x + " > " + bounds.extents.x);
            halfSize = GetHalfSize();
        }

        if (!Follow) {
            return;
        }
        transform.position = new Vector3(
            Mathf.Clamp(Follow.position.x, bounds.min.x + halfSize.x, bounds.max.x - halfSize.x),
            Mathf.Clamp(Follow.position.y, bounds.min.y + halfSize.y, bounds.max.y - halfSize.y),
            transform.position.z
        );
    }

    private Vector2 GetHalfSize() {
        return new Vector2(_camera.orthographicSize * (Screen.width / (float) Screen.height), _camera.orthographicSize);
    }
}