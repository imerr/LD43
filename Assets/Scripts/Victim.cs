using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum VictimType {
    Human
}

public class Victim : MonoBehaviour {
    public float FearDistance = 8;
    public float FleeSpeed = 3;
    public float WalkSpeed = 1;
    public float WalkTime = 3;
    public int Score = 10;
    public VictimType Type;
    [Range(0, 1)] public float WalkChance = 0.01f;
    private Rigidbody2D _body;
    private Collider2D _collider;
    private float _walkTime;
    private float _fleeBias;
    private bool _fleeing;
    private bool _exiting;
    private Vector3 _exitPoint;
    private SpriteRenderer _renderer;
    private SpriteAnimator _animator;
    private bool _sacrificing;
    private bool _sacrificeJumping;
    private Vector3 _jumpPoint;

    private void Awake() {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<SpriteAnimator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        if (Level.Instance) {
            Level.Instance.AddVictim(this);
        }
    }

    void FixedUpdate() {
        if (_sacrificing) {
            if (_sacrificeJumping) {
                _body.velocity += new Vector2(0, -9) * Time.fixedDeltaTime;
            } else {
                var dir = _jumpPoint - transform.position;
                if (dir.magnitude < 0.1) {
                    _sacrificeJumping = true;
                    _body.velocity = new Vector2(2, 5);
                    _animator.State = EntityState.Idle;
                    return;
                }

                _animator.State = EntityState.Running;
                _body.velocity = Vector3.ClampMagnitude(dir.normalized * FleeSpeed, dir.magnitude / Time.deltaTime);
                return;
            }
            return;
        }
        if (_exiting) {
            var dir = _exitPoint - transform.position;
            if (dir.magnitude < 0.1) {
                Destroy(gameObject);
                return;
            }
            transform.position = transform.position + Vector3.ClampMagnitude(dir.normalized * FleeSpeed * Time.deltaTime, dir.magnitude);
        } else {
            var player = Level.Instance.CurrentPlayer;
            if (Random.Range(1, 30) == 1) {
                _fleeBias = Random.Range(-10, 10);
            }
    
            if (player && player.gameObject.activeInHierarchy) {
                var dif = transform.position - player.transform.position;
                if (dif.magnitude < FearDistance) {
                    _fleeing = true;
                    _walkTime = 0;
                    var fleeDegree = MathHelper.Vector2ToDegrees(dif) + _fleeBias;
                    var fleeDir = MathHelper.DegreeToVector2(fleeDegree);
                    var hit = Physics2D.Raycast(transform.position, fleeDir,
                        FleeSpeed * Time.fixedDeltaTime * 10 +
                        Mathf.Max(_collider.bounds.extents.x, _collider.bounds.extents.y), LayerHelper.MoveMask);
                    if (hit.collider != null) {
                        float hitAngle = MathHelper.Vector2ToDegrees(hit.normal);
                        float leftDifference = MathHelper.AngleDifference(fleeDegree, hitAngle - 90);
                        float rightDifference = MathHelper.AngleDifference(fleeDegree, hitAngle + 90);
                        if (leftDifference < rightDifference) {
                            fleeDegree = hitAngle - 90;
                        } else {
                            fleeDegree = hitAngle + 90;
                        }
                        fleeDir = MathHelper.DegreeToVector2(fleeDegree);
                    }
    
                    _animator.State = EntityState.Running;
                    _body.velocity = fleeDir * FleeSpeed;
                } else {
                    _fleeing = false;
                    _walkTime -= Time.deltaTime;
                    if (_walkTime < 0) {
                        _body.velocity *= 0.95f;
                        if (_body.velocity.magnitude < 0.05) {
                            _animator.State = EntityState.Idle;
                        }
                    }
    
                    if (WalkChance > Random.Range(0.0f, 1.0f)) {
                        _walkTime = WalkTime;
                        _body.velocity = Random.onUnitSphere * WalkSpeed;
                        _animator.State = EntityState.Walking;
                    }
                }
            }
    
            if (_body.velocity.x > 0.05) {
                _animator.FlipX = false;
            } else if (_body.velocity.x < -0.05) {
                _animator.FlipX = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerHelper.LayerExit) {
            Level.Instance.ReachedGoal(this);
            _body.bodyType = RigidbodyType2D.Static;
            _exiting = true;
            _exitPoint = other.gameObject.transform.position + other.gameObject.transform.rotation * Vector3.right * 3;
        }
    }

    public static Victim InstantiateType(VictimType type, Vector3 position) {
        GameObject prefab;
        switch (type) {
            case VictimType.Human:
                prefab = Resources.Load<GameObject>("Victims/Human");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        var v =  Instantiate(prefab, position, Quaternion.identity).GetComponent<Victim>();
        v.transform.position = position - new Vector3(0, v.transform.InverseTransformPoint(v._renderer.bounds.min).y);
        return v;
    }

    public void Sacrifice(Vector3 jumpPoint) {
        _sacrificing = true;
        _jumpPoint = jumpPoint - new Vector3(0, transform.InverseTransformPoint(_renderer.bounds.min).y);
    }
}