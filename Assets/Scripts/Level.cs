using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Level : MonoBehaviour {
    private const float LevelEndTime = 2;
   
    public static Level Instance;
    public static List<VictimType> KilledVictims = new List<VictimType>();
    public static int VictimsLeft;
    public static float TimeLeft;
    public static string NextScene;
    
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
    public Text TimeText;
    public Image FinishVictims;
    public Image FinishTime;
    public GameObject TutorialObject;
    [Header("Settings")] 
    public float LevelTime;
    public bool FirstLevel;
    public string NextLevel;
    public AudioSource VictimGetSound;
    
    
    
    private Player _player;
    private SpawnPoint _spawnPoint;
    private HashSet<Victim> _victims = new HashSet<Victim>();
    private bool _over;


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
        TutorialObject.SetActive(FirstLevel);
    }

    private void OnDestroy() {
        Time.timeScale = 1;
        Time.fixedDeltaTime = (1.0f / 60) * Time.timeScale;
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Update() {
        if (TutorialObject.activeInHierarchy) {
            Time.timeScale = 0;
            if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) {
                Time.timeScale = 1;
                TutorialObject.SetActive(false);
            } else {
                return;
            }

        }
        if (_over) {
            return;
        }
        if (!_player) {
            _player = Instantiate(PlayerPrefab, _spawnPoint.transform.position + new Vector3(0, 0.7f), Quaternion.identity).GetComponent<Player>();
            Camera.Follow = _player.transform;
            _player.gameObject.SetActive(false);
            _spawnPoint.Spawn(() => {
                _player.gameObject.SetActive(true);
            });
        }

        LevelTime -= Time.deltaTime;
        if (LevelTime <= 0 || _victims.Count == 0) {
            // level over
            _over = true;
            StartCoroutine(nameof(EndLevel));
            return;
        }
        TimeText.text = MathHelper.CountdownSeconds(LevelTime) + " - " + _victims.Count + " left";
    }

    public void ReachedGoal(Victim victim) {
        VictimGetSound.Play();
        _victims.Remove(victim);
        KilledVictims.Add(victim.Type);
    }

    public void AddVictim(Victim victim) {
        _victims.Add(victim);
    }

    private IEnumerator EndLevel() {
        VictimsLeft = _victims.Count;
        TimeLeft = LevelTime;
        NextScene = NextLevel;
        Image endImg = VictimsLeft == 0 ? FinishVictims : FinishTime;
        endImg.gameObject.SetActive(true);
        float timer = 0;
        do {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = MathHelper.TweenInQuad(1, 0.1f, Mathf.Min(1, timer * 2/ LevelEndTime));
            Time.fixedDeltaTime = (1.0f / 60) * Time.timeScale;
            float imgScale = MathHelper.TweenInQuad(20, 1, Mathf.Min(1, timer * 2 / LevelEndTime));
            endImg.transform.localScale = new Vector3(imgScale, imgScale, imgScale);
            yield return null;
        } while (timer < LevelEndTime);
        SceneManager.LoadScene("Scenes/Score");
    }

}
