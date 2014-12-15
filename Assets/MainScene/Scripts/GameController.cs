using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public const float PLAYING_FIELD_X_MIN = 10.0f;
    public const float PLAYING_FIELD_X_MAX = 90.0f;
    public const float PLAYING_FIELD_Z_MIN = 8.0f;
    public const float PLAYING_FIELD_Z_MAX = 90.0f;

    private const int MAX_SNOWMAN = 5;
    private const int DELAY_BETWEEN_SNOWMAN_SPAWN = 4;
    private const float DELAY_AFTER_GAME_OVER = 2.0f;

    public GameObject SnowmanPrefab;
    public GameObject PlayerPrefab;
    public GameObject UIGameOver;

    public class GameInfo
    {
        private int _lives;
        public int Lives
        {
            get { return _lives; }
            set
            {
                _lives = value;
                OnLivesChange();
            }
        }

        private void OnLivesChange()
        {
            if ( Lives == 0 ) {
                GameOver = true;
                instance.OnGameOver();
            }
        }

        public long Score { get; set; }

        public void Reset()
        {
            Lives = 3;
            Score = 0;
            GameOver = false;
        }

        public bool GameOver { get; set; }
    }

    public GameInfo Info { get; private set; }
    public PlayerControl Player { get; set; }
    private GameObject _player_instance;

    public Text    UIScore;

    private int _snowman_count;
    private  float _snowman_spawn_timeout;
    private  float _delay_since_gameover;

    /******************************************************************/
    public void Start()
    {
        Info = new GameInfo();
        instance = this;
        CreatePlayer();
        Reset();
    }

    /******************************************************************/
    private void CreatePlayer()
    {
        if ( !_player_instance ) {
            GameObject spawn_point = GameObject.Find( "PlayerSpawn" );

            _player_instance = Instantiate( PlayerPrefab, spawn_point.transform.position, spawn_point.transform.rotation ) as GameObject;

            if ( _player_instance != null )
                Player = _player_instance.GetComponent<PlayerControl>();
            else
                throw new UnityException( "Problem instantiating Player" );
        }
    }

    /******************************************************************/
    void Reset()
    {
        Time.timeScale = 1.0f;
        UIGameOver.SetActive( false );
        Info.Reset();
        Player.Reset();
        _snowman_spawn_timeout = Time.time + DELAY_BETWEEN_SNOWMAN_SPAWN;
        _snowman_count = 0;
    }

    /******************************************************************/
    public void Update()
    {
        if ( Info.GameOver ) {
            if ( Time.unscaledTime > _delay_since_gameover ) {
                if ( Input.anyKeyDown ) {
                    Reset();
                }
            }
        }
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
        instance.Info.Score += 100;
    }

    /******************************************************************/
    public void KillAllSnowmen()
    {
        // Wipe out all the other snowmen; start fresh
        GameObject[] snowmen = GameObject.FindGameObjectsWithTag( "Enemy" );
        for ( int i = 0; i < snowmen.Length; i++ ) {
            Destroy( snowmen[ i ] );
        }
        _snowman_count = 0;
    }

    /******************************************************************/
    private void SpawnSnowman()
    {
        var target = new Vector3(
            Random.Range( PLAYING_FIELD_X_MIN, PLAYING_FIELD_X_MAX ),
            0.0f,
            Random.Range( PLAYING_FIELD_Z_MIN, PLAYING_FIELD_Z_MAX )
            );

        Instantiate( SnowmanPrefab, target, Quaternion.identity );

        Debug.Log( "### Spawning new Snowman at " + target );

    }

    /******************************************************************/
    private void OnGameOver()
    {
        UIGameOver.SetActive( true );
        _delay_since_gameover = Time.unscaledTime + DELAY_AFTER_GAME_OVER;
        Time.timeScale = 0;
    }
}
