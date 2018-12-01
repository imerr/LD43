using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody2D _body;
    public Vector2 BaseSpeed = new Vector2(5f, 2.5f);
    private SpriteRenderer _sprite;

    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        var speed = new Vector2(0,0);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            speed.x = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            speed.x = 1;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            speed.y = 1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            speed.y = -1;
        }

        if (speed.x > 0) {
            _sprite.flipX = false;
        } else if (speed.x < 0) {
            _sprite.flipX = true;
        }

        if (speed.sqrMagnitude > 0) {
            _body.velocity = Vector2.Scale(Vector2.ClampMagnitude(speed, 1), BaseSpeed);
        } else {
            _body.velocity *= 0.9f;
        }
    }
}
