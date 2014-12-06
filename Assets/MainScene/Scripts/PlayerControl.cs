using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    private TankMovement treads;

    void OnCollisionEnter( Collision other )
    {
        Debug.Log( "Hit " + other.collider.name );
    }

    void Start()
    {
        treads = GetComponent<TankMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        treads.RotateLeft( Input.GetAxis( "Left Tread" ) );
        treads.RotateRight( Input.GetAxis( "Right Tread" ) );
    }
}
