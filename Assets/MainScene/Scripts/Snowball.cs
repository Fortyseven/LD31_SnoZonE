using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour
{
    /* Note: these snowballs should be in an object pool, but we're
     * just instantiating/destroying because I'm in a hurry. */

    private const float LIFETIME = 5.0f;

    void Start()
    {
        Destroy( this.gameObject, LIFETIME );
    }
}
