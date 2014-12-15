using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject FirePointObject;
    public GameObject SnowBallObject;

    public bool Hurting { get; set; }

    private const float CANNON_FORCE = 5000.0f;
    private const float HURT_TIME_LENGTH = 2.0f;
    private float _hurt_timeout;
    private TankMovement _treads;

    private GlitchEffect _glitch;

    private Vector3 _start_position;

    private CamShake _cam_shake;
    //private Quaternion _start_rotation; //NOTE: Is this necessary?

    AudioSource[] _audio_sources;

    /******************************************************************/
    //void OnCollisionEnter( Collision other )
    //{
    //    //Debug.Log( "Hit " + other.collider.name );
    //}

    /******************************************************************/
    public void Awake()
    {
        _start_position = GameObject.Find( "PlayerSpawn" ).transform.position;

        //_start_rotation = transform.rotation;

        Physics.IgnoreLayerCollision( LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "Player" ) );
        _treads = GetComponent<TankMovement>();
        _glitch = GetComponentInChildren<GlitchEffect>();
        _glitch.enabled = false;

        _audio_sources = GetComponents<AudioSource>();

        _cam_shake = GetComponentInChildren<CamShake>();
        //_audio_sources[ 0 ].pan = -1;
        //_audio_sources[ 1 ].pan = 1;

    }

    /******************************************************************/
    public void Reset()
    {
        transform.position = _start_position;
        Hurting = false;
        _glitch.enabled = false;
        _cam_shake.ShakeOff();
        //transform.rotation = _start_rotation;
    }

    /******************************************************************/
    public void Update()
    {
        if ( GameController.instance.Info.GameOver ) {
            return;
        }

        if ( Hurting ) {
            if ( Time.time >= _hurt_timeout ) {
                _glitch.enabled = false;
                _cam_shake.ShakeOff();
            }
            else {
                return;
            }
        }

        if ( Input.GetButtonDown( "Fire" ) ) {
            FireCannon();
        }

        //treads.RotateLeft( Input.GetAxis( "Left Tread" ) );
        //treads.RotateRight( Input.GetAxis( "Right Tread" ) );

        float left_tread = 0;
        float right_tread = 0;

        //left_tread += Input.GetAxis( "Left Tread" );
        //right_tread += Input.GetAxis( "Right Tread" );

        if ( Input.GetButton( "Forward" ) ) {
            left_tread += 1.0f;
            right_tread += 1.0f;
        }
        if ( Input.GetButton( "Reverse" ) ) {
            left_tread -= 1.0f;
            right_tread -= 1.0f;
        }

        if ( Input.GetButton( "TurnLeft" ) ) {
            right_tread += 1.0f;
            left_tread -= 1.0f;
        }
        if ( Input.GetButton( "TurnRight" ) ) {
            left_tread += 1.0f;
            right_tread -= 1.0f;
        }

        if ( left_tread != 0 ) {
            if ( !_audio_sources[ 0 ].isPlaying )
                _audio_sources[ 0 ].Play();
        }
        else {
            _audio_sources[ 0 ].Stop();
        }

        if ( right_tread != 0 ) {
            if ( !_audio_sources[ 1 ].isPlaying )
                _audio_sources[ 1 ].Play();
        }
        else {
            _audio_sources[ 1 ].Stop();
        }

        _treads.RotateLeft( Mathf.Clamp( left_tread, -1.0f, 1.0f ) );
        _treads.RotateRight( Mathf.Clamp( right_tread, -1.0f, 1.0f ) );
    }

    /******************************************************************/
    private void FireCannon()
    {
        GameObject ball = Instantiate( SnowBallObject, transform.position, Quaternion.identity ) as GameObject;

        // TODO: Would like this to fire a bit harder if we're moving forward
        if ( ball != null ) {
            ball.GetComponent<Rigidbody>().AddForce( transform.forward * ( CANNON_FORCE + rigidbody.velocity.magnitude ) );
        }
        else {
            throw new UnityException( "No ball. Going home." );
        }
    }

    /******************************************************************/
    public void HitBy( GameObject game_object )
    {
        GameController.instance.Info.Lives--;
        Hurting = true;
        _glitch.enabled = true;
        _audio_sources[ 2 ].Play();
        _hurt_timeout = Time.time + HURT_TIME_LENGTH;
        _cam_shake.ShakeOn();
        GameController.instance.KillAllSnowmen();
    }
}
