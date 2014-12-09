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
