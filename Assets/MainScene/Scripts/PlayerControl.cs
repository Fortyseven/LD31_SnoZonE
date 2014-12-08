using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public GameObject FirePointObject;
    public GameObject SnowBallObject;

    private const float CANNON_FORCE = 5000.0f;
    private TankMovement _treads;

    private Vector3 _start_position;
    //private Quaternion _start_rotation; //NOTE: Is this necessary?

    /******************************************************************/
    void OnCollisionEnter( Collision other )
    {
        Debug.Log( "Hit " + other.collider.name );
    }

    /******************************************************************/
    void Start()
    {
        _start_position = transform.position;
        //_start_rotation = transform.rotation;

        Physics.IgnoreLayerCollision( LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "Player" ) );
        _treads = GetComponent<TankMovement>();
    }

    /******************************************************************/
    public void Reset()
    {
        transform.position = _start_position;
        //transform.rotation = _start_rotation;
    }

    /******************************************************************/
    void Update()
    {
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


        _treads.RotateLeft( Mathf.Clamp( left_tread, -1.0f, 1.0f ) );
        _treads.RotateRight( Mathf.Clamp( right_tread, -1.0f, 1.0f ) );

    }

    /******************************************************************/
    private void FireCannon()
    {
        GameObject ball = Instantiate( SnowBallObject, transform.position, Quaternion.identity ) as GameObject;

        // TODO: Would like this to fire a bit harderif we're moving forward
        ball.GetComponent<Rigidbody>().AddForce( transform.forward * ( CANNON_FORCE + rigidbody.velocity.magnitude ) );
    }
}
