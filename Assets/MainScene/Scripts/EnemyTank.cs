using UnityEngine;
using System.Collections;

public class EnemyTank : MonoBehaviour
{
    /* Note: these enemies should be in an object pool, but we're
     * just instantiating/destroying because I'm in a hurry. */

    private const float CHASE_UPDATE_DELAY = 0.5f;
    private const float STALE_STATE_HACK_TIMEOUT = 15.0f;
    private const float MIN_SPEED = 1.0f;
    private const float MAX_SPEED = 6.0f;

    public AudioClip[] AudioSpawn;
    public AudioClip[] AudioDeath;

    private NavMeshAgent _agent;
    private TankMovement _treads;
    public GameObject MyMesh;
    enum State
    {
        SPAWNING,           // spawn animation playing; don't move
        IDLE,               // ready to switch to other state
        CHASING_PLAYER,     // 
        WANDERING           // moving to a random spot
    }
    private State _current_state;
    private Vector3 _target;
    private float _chase_update;
    private float _state_watchdog_timer_hack;

    /******************************************************************/
    void Start()
    {
        _treads = GetComponent<TankMovement>();
        Utils.Assert( _treads );

        _agent = GetComponent<NavMeshAgent>();
        Utils.Assert( _agent );

        _current_state = State.SPAWNING;

        _agent.speed = Random.Range( MIN_SPEED, MAX_SPEED );
        audio.PlayOneShot( AudioSpawn[ Random.Range( 0, AudioSpawn.Length - 1 ) ] );
        Debug.Log( "### Spawned snowman with speed " + _agent.speed );
    }

    /******************************************************************/
    void Update()
    {
        if ( _current_state == State.SPAWNING )
            return;

        if ( IsStopped() ) {
            Debug.Log( "### I have stopped. Going IDLE to decide next move." );
            _current_state = State.IDLE;
            ChangeState();
            return;
        }

        // TODO: If player is inside radius, switch to hunting mode

        switch ( _current_state ) {
            case State.IDLE:
                ChangeState();
                break;
            case State.CHASING_PLAYER:
                if ( Time.time > _chase_update ) {
                    _chase_update = Time.time + CHASE_UPDATE_DELAY;
                    _agent.SetDestination( GameController.instance.Player.transform.position );
                    // There's a chance we'll lose interest and break off
                    if ( Random.Range( 0, 10 ) == 4 ) {
                        Debug.Log( "### Bored of CHASING, now WANDERING..." );
                        StateWanderStart();
                    }
                }
                break;
            case State.WANDERING:


                break;
        }

        // HACKHACK
        if ( Time.time > _state_watchdog_timer_hack ) {
            NavmeshHackAssBoot();
            return;
        }
        Debug.DrawLine( transform.position, _target );

        //_treads.RotateLeft( 1.0f );
        //_treads.RotateRight( 1.0f );
    }

    /******************************************************************/
    private bool IsStopped()
    {
        if ( !_agent.pathPending ) {
            if ( _agent.remainingDistance <= _agent.stoppingDistance ) {
                if ( !_agent.hasPath || _agent.velocity.sqrMagnitude == 0f ) {
                    return true;
                }
            }
        }
        return false;
    }

    /******************************************************************/
    public void ChangeState()
    {
        if ( Random.Range( 0, 2 ) == 0 ) {
            StateWanderStart();
        }
        else {
            StateChasePlayerStart();
        }
        StateChasePlayerStart();

        _state_watchdog_timer_hack = Time.time + STALE_STATE_HACK_TIMEOUT;
    }

    /******************************************************************/
    public void NavmeshHackAssBoot()
    {
        Debug.Log( "### HACK: Booting in the ass" );
        Vector3 newpos = new Vector3(
                        Random.Range( GameController.PLAYING_FIELD_X_MIN, GameController.PLAYING_FIELD_X_MAX ),
                        0.0f,
                        Random.Range( GameController.PLAYING_FIELD_Z_MIN, GameController.PLAYING_FIELD_Z_MAX )
                        );

        _agent.Warp( newpos );
        ChangeState();
    }

    /******************************************************************/
    public void StateWanderStart()
    {
        _target = new Vector3(
                        Random.Range( GameController.PLAYING_FIELD_X_MIN, GameController.PLAYING_FIELD_X_MAX ),
                        0.0f,
                        Random.Range( GameController.PLAYING_FIELD_Z_MIN, GameController.PLAYING_FIELD_Z_MAX )
                        );

        NavMeshHit hit;
        NavMesh.SamplePosition( _target, out hit, 100.0f, 1 );
        _target = hit.position;

        _agent.SetDestination( _target );
        Debug.Log( "### I have begun WANDERING to: " + _target );
        _current_state = State.WANDERING;
    }
    /******************************************************************/
    public void StateChasePlayerStart()
    {
        _current_state = State.CHASING_PLAYER;
        Debug.Log( "### I have begun CHASING player" );
        _agent.SetDestination( GameController.instance.Player.transform.position );
        _chase_update = Time.time + CHASE_UPDATE_DELAY;
    }

    /******************************************************************/
    public void OnSpawn()
    {
        GetComponentInChildren<Animation>().Play( "SnowmanWaddle" );
        _current_state = State.IDLE;
    }

    /******************************************************************/
    public void OnDeathAnimFinish()
    {
        Destroy( this.gameObject, 1.0f );
    }

    /******************************************************************/
    public void OnCollisionEnter( Collision col )
    {
        // No cheap kills
        if ( _current_state == State.SPAWNING )
            return;

        if ( col.gameObject.tag == "Projectile" ) {
            Destroy( col.gameObject );
            KillMe();
        }

        if ( col.gameObject.tag == "Player" ) {
            GameController.instance.Player.HitBy( this.gameObject );
            KillMe( false ); // no score for death by snowman, jerk
        }
    }
    /******************************************************************/
    public void KillMe( bool tell_player = true )
    {
        _current_state = State.SPAWNING;
        _agent.Stop();
        rigidbody.detectCollisions = false;

        if ( tell_player ) {
            GameController.instance.OnSnowmanKilled();
            GetComponentInChildren<ParticleSystem>().Play();
            MyMesh.animation.Play( "SnowmanDeath" );
            audio.PlayOneShot( AudioDeath[ Random.Range( 0, AudioDeath.Length - 1 ) ] );
        }
        else {
            // striaght to death
            OnDeathAnimFinish();
        }
    }

}
