using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReticleController : MonoBehaviour
{
    public GameObject ReticleObject;

    private  Image _imgReticle;
    public Text  _txtRange;

    void Start()
    {
        _imgReticle = ReticleObject.GetComponent<Image>();
        Utils.Assert( _imgReticle );
    }

    void Update()
    {
        Vector3 dir = transform.TransformDirection( Vector3.forward );
        RaycastHit hit_info;

        if ( Physics.Raycast( transform.position, dir, out hit_info, 50.0f ) ) {
            if ( hit_info.collider.tag == "Enemy" ) {
                _txtRange.text = hit_info.distance.ToString( "###.#" ) + " M";
                _imgReticle.color = Color.red;
                return;
            }
        }
        _txtRange.text = "-- M";
        _imgReticle.color = Color.green;
    }
}
