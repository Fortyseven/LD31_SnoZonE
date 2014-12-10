using UnityEngine;
using System.Collections;

public class CamShake : MonoBehaviour
{
    void Start()
    {
        _running = false;
    }

    private float f = 0;
    private  bool _running;

    void Update()
    {
        if ( !_running )
            return;

        Vector3 foo = Vector3.zero;
        foo.x += 7.0f * Mathf.PerlinNoise( f, 0 );
        foo.y += 7.0f * Mathf.PerlinNoise( 0, f );
        transform.localPosition = foo;
        float rot = 45.0f * Mathf.PerlinNoise( 0, f );
        transform.localRotation = Quaternion.Euler( 0, rot, 0 );

        f += Time.deltaTime * 50;
    }

    public void ShakeOn()
    {
        _running = true;
    }

    public void ShakeOff()
    {
        _running = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
