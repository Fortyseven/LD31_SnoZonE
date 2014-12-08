using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public const float PLAYING_FIELD_X_MIN = 10.0f;
    public const float PLAYING_FIELD_X_MAX = 90.0f;
    public const float PLAYING_FIELD_Z_MIN = 8.0f;
    public const float PLAYING_FIELD_Z_MAX = 90.0f;

    private int MAX_SNOWMAN = 5;
    private int DELAY_BETWEEN_SNOWMAN_SPAWN = 4;

    public GameObject SnowmanPrefab;
    public GameObject PlayerPrefab;

    public class State
    {
    }

    public class GameInfo
    {
        public int Lives { get; set; }
        public long Score { get; set; }

        public void Reset()
        {
            Lives = 3;
            Score = 0;
        }
    }

    public GameInfo Info { get; private set; }
    public PlayerControl Player { get; set; }
    private GameObject _player_instance;

    public Text    UIScore;

    private int _snowman_count = 0;
    private  float _snowman_spawn_timeout;


    /******************************************************************/
    void Start()
    {
        Info = new GameInfo();
        GameController.instance = this;
        CreatePlayer();
        Reset();
    }

    /******************************************************************/
    private void CreatePlayer()
    {
        if ( !_player_instance ) {
            GameObject spawn_point = GameObject.Find( "PlayerSpawn" );
            _player_instance = Instantiate( PlayerPrefab, spawn_point.transform.position, spawn_point.transform.rotation ) as GameObject;
            Player = _player_instance.GetComponent<PlayerControl>();
            Debug.Log( "Player spawned at " + spawn_point.transform.position );
        }
    }

    /******************************************************************/
    void Reset()
    {
        Info.Reset();
        _snowman_spawn_timeout = Time.time + DELAY_BETWEEN_SNOWMAN_SPAWN;
    }

    /******************************************************************/
    void Update()
    {
        UIScore.text = Info.Score.ToString();

        if ( _snowman_count < MAX_SNOWMAN ) {
            if ( Time.time > _snowman_spawn_timeout ) {
                _snowman_spawn_timeout = Time.time + DELAY_BETWEEN_SNOWMAN_SPAWN;
                SpawnSnowman();
                _snowman_count++;
            }
        }
    }

    /******************************************************************/
    public void OnSnowmanKilled()
    {
        _snowman_count--;
        GameController.instance.Info.Score += 100;
    }

    /******************************************************************/
    private void SpawnSnowman()
    {
        Vector3 target;
        target = new Vector3(
                       Random.Range( PLAYING_FIELD_X_MIN, PLAYING_FIELD_X_MAX ),
                       0.0f,
                       Random.Range( PLAYING_FIELD_Z_MIN, PLAYING_FIELD_Z_MAX )
                       );

        Instantiate( SnowmanPrefab, target, Quaternion.identity );

        Debug.Log( "Spawning new Snowman at " + target );

    }
}
