using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour {
    public Bounds Bounds {
        get {
            var bounds = Tilemap.localBounds;
            var nb=  new Bounds(Tilemap.transform.TransformPoint(bounds.center), 2 * Vector3.Scale(bounds.extents, Tilemap.transform.lossyScale));
            return nb;
        }
    }

    [Header("Prefabs")]
    public GameObject PlayerPrefab;
    [Header("References")]
    public Tilemap Tilemap;
    public CameraFollow Camera;
    public static Level Instance;
    private Player _player;
    private SpawnPoint _spawnPoint;

    public Player CurrentPlayer {
        get { return _player; }
    }

    private void Awake() {
        Instance = this;
        _spawnPoint = GameObject.FindWithTag("Respawn")?.GetComponent<SpawnPoint>();
        if (!_spawnPoint) {
            Debug.LogError("No spawn point, please add one");
        }
    }

    private void Start() {
        Tilemap.CompressBounds();
    }

    private void Update() {
        if (!_player) {
            _player = Instantiate(PlayerPrefab, _spawnPoint.transform.position + new Vector3(0, 0.7f), Quaternion.identity).GetComponent<Player>();
            Camera.Follow = _player.transform;
            _player.gameObject.SetActive(false);
            _spawnPoint.Spawn(() => {
                _player.gameObject.SetActive(true);
            });
        }
    }
}