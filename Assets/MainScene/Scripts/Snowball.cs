using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour
{
    public AudioClip AudioClipFire;
    public AudioClip AudioClipImpact;
    /* Note: these snowballs should be in an object pool, but we're
     * just instantiating/destroying because I'm in a hurry. */

    private const float LIFETIME = 5.0f;

    void Start()
    {
        audio.PlayOneShot( AudioClipFire );
        Destroy( this.gameObject, LIFETIME );
    }

    void Update()
    {
        //if ( !rigidbody.detectCollisions )
        //    return;
        //float foo = Mathf.Abs( rigidbody.velocity.x + rigidbody.velocity.y + rigidbody.velocity.z );
        //if ( foo < 0.01f ) {
        //    Debug.Log( "Snowball " + GetInstanceID() + " dead velocity: " + foo );
        //    rigidbody.detectCollisions = false;
        //    return;
        //}
        //Debug.Log( "Snowball " + GetInstanceID() + " LIVE velocity: " + foo );
    }

    public void OnCollisionEnter( Collision col )
    {
        AudioSource col_audio =  col.gameObject.GetComponent<AudioSource>();

        if ( !col_audio ) {
            col_audio = this.audio;
        }

        col_audio.PlayOneShot( AudioClipImpact );
        //Debug.Log( "OH GOD WHAT HAVE I HIT: " + col.gameObject.name );
    }
}
