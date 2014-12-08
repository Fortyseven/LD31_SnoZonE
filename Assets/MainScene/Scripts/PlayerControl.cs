using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public GameObject FirePointObject;
    public GameObject SnowBallObject;

    private const float CANNON_FORCE = 5000.0f;
    private TankMovement treads;

    void OnCollisionEnter( Collision other )
    {
        Debug.Log( "Hit " + other.collider.name );
    }

    void Start()
    {
        Debug.Log( "LAYERPLAYER:" + LayerMask.NameToLayer( "Player" ) );
        Physics.IgnoreLayerCollision( LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "Player" ) );
        treads = GetComponent<TankMovement>();
    }

    // Update is called once per frame
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


        treads.RotateLeft( Mathf.Clamp( left_tread, -1.0f, 1.0f ) );
        treads.RotateRight( Mathf.Clamp( right_tread, -1.0f, 1.0f ) );

    }

    private void FireCannon()
    {
        GameObject ball = Instantiate( SnowBallObject, transform.position, Quaternion.identity ) as GameObject;

        // TODO: Would like this to fire a bit harderif we're moving forward
        ball.GetComponent<Rigidbody>().AddForce( transform.forward * ( CANNON_FORCE + rigidbody.velocity.magnitude ) );
    }
}
