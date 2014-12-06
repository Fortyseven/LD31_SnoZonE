using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour
{
    public float BASE_SPEED = 45.0f;
    public float FOWARD_SPEED = 0.05f;

    public float stability = 0.3f;
    public float speed = 2.0f;

    // via http://answers.unity3d.com/questions/10425/how-to-stabilize-angular-motion-alignment-of-hover.html
    void FixedUpdate()
    {
        Vector3 predictedUp = Quaternion.AngleAxis(
            rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            rigidbody.angularVelocity
        ) * transform.up;

        Vector3 torqueVector = Vector3.Cross( predictedUp, Vector3.up );
        rigidbody.AddTorque( torqueVector * speed * speed );
    }

    public void RotateLeft( float amount )
    {
        transform.Rotate( Vector3.up, amount * BASE_SPEED * Time.deltaTime );
        transform.Translate( new Vector3( 0, 0, amount * BASE_SPEED * FOWARD_SPEED * Time.deltaTime ), Space.Self );
    }

    public void RotateRight( float amount )
    {
        transform.Rotate( Vector3.up, -amount * BASE_SPEED * Time.deltaTime );
        transform.Translate( new Vector3( 0, 0, amount * BASE_SPEED * FOWARD_SPEED * Time.deltaTime ), Space.Self );
    }
}
