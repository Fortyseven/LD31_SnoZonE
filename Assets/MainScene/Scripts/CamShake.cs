using UnityEngine;

public class CamShake : MonoBehaviour
{
    private float _f;
    private  bool _running;

    public void Start()
    {
        _running = false;
        _f = 0;
    }

    public void Update()
    {
        if ( !_running )
            return;

        Vector3 foo = Vector3.zero;
        foo.x += 7.0f * Mathf.PerlinNoise( _f, 0 );
        foo.y += 7.0f * Mathf.PerlinNoise( 0, _f );
        transform.localPosition = foo;
        float rot = 45.0f * Mathf.PerlinNoise( 0, _f );
        transform.localRotation = Quaternion.Euler( 0, rot, 0 );

        _f += Time.deltaTime * 50;
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
