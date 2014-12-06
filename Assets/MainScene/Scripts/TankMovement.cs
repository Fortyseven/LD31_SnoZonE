using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour
{
    private const float BASE_SPEED = 0.5f;
    private const float FOWARD_SPEED = 0.05f;

    public void RotateLeft( float amount )
    {
        transform.Rotate( Vector3.up, amount * BASE_SPEED );
        transform.Translate( new Vector3( 0, 0, amount * BASE_SPEED * FOWARD_SPEED ), Space.Self );
    }

    public void RotateRight( float amount )
    {
        transform.Rotate( Vector3.up, -amount * BASE_SPEED );
        transform.Translate( new Vector3( 0, 0, amount * BASE_SPEED * FOWARD_SPEED ), Space.Self );
    }
}
