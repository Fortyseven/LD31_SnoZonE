using UnityEngine;
using System.Collections;

public class EnemyTank : MonoBehaviour
{
    private NavMeshAgent _agent;
    private TankMovement _treads;

    enum State
    {
        SPAWNING,           // spawn animation playing; don't move
        IDLE,               // ready to switch to other state
        HUNTING_PLAYER,     // 
        WANDERING           // moving to a random spot
    }
    private State _current_state;
    private Vector3 _target;

    private const float PLAYING_FIELD_X_MIN = 10.0f;
    private const float PLAYING_FIELD_X_MAX = 90.0f;
    private const float PLAYING_FIELD_Z_MIN = 8.0f;
    private const float PLAYING_FIELD_Z_MAX = 90.0f;

    /******************************************************************/
    void Start()
    {
        _treads = GetComponent<TankMovement>();
        Utils.Assert( _treads );

        _agent = GetComponent<NavMeshAgent>();
        Utils.Assert( _agent );

        _current_state = State.SPAWNING;
    }

    /******************************************************************/
    void Update()
    {
        if ( _current_state == State.SPAWNING )
            return;

        switch ( _current_state ) {
            case State.IDLE:
                ChangeState();
                break;
            case State.HUNTING_PLAYER:
            case State.WANDERING:
                Debug.DrawLine( transform.position, _target );
                if ( IsStopped() ) {
                    Debug.Log( "### I have stopped." );
                    _current_state = State.IDLE;
                }
                break;
        }

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
            StateHuntPlayerStart();
        }
    }

    /******************************************************************/
    public void StateWanderStart()
    {
        _target = new Vector3(
                        Random.RandomRange( PLAYING_FIELD_X_MIN, PLAYING_FIELD_X_MAX ),
                        0.0f,
                        Random.RandomRange( PLAYING_FIELD_Z_MIN, PLAYING_FIELD_Z_MAX )
                        );

        NavMeshHit hit;
        NavMesh.SamplePosition( _target, out hit, 100.0f, 1 );
        _target = hit.position;

        _agent.SetDestination( _target );
        Debug.Log( "### I have begun WANDERING to: " + _target );
        _current_state = State.WANDERING;
    }
    /******************************************************************/
    public void StateHuntPlayerStart()
    {
        _current_state = State.HUNTING_PLAYER;
        Debug.Log( "### I have begun HUNTING player" );
    }

    /******************************************************************/
    public void OnSpawn()
    {
        GetComponentInChildren<Animation>().Play( "SnowmanWaddle" );
        _current_state = State.IDLE;
    }

    /******************************************************************/
    public void OnCollisionEnter( Collision col )
    {
        if ( col.gameObject.tag == "Projectile" ) {
            Destroy( this.gameObject );
            Destroy( col.gameObject );
        }
    }
}
