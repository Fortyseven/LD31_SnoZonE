using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float BASE_SPEED = 45.0f;
    public float FOWARD_SPEED = 0.05f;

    public float stability = 0.3f;
    public float speed = 2.0f;

    // via http://answers.unity3d.com/questions/10425/how-to-stabilize-angular-motion-alignment-of-hover.html
    public void FixedUpdate()
    {
        Vector3 predicted_up = Quaternion.AngleAxis(
            rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            rigidbody.angularVelocity
        ) * transform.up;

        Vector3 torque_vector = Vector3.Cross( predicted_up, Vector3.up );
        rigidbody.AddTorque( torque_vector * speed * speed );
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
