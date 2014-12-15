using UnityEngine;
using UnityEngine.UI;

public class ReticleController : MonoBehaviour
{
    //public GameObject ReticleObject;

    public Image _imgReticle;
    public Text  _txtRange;

    public Image[] ShieldBars;

    public void Update()
    {
        GameObject player = GameController.instance.Player.gameObject;
        Vector3 dir = player.transform.TransformDirection( Vector3.forward );
        RaycastHit hit_info;

        if ( Physics.Raycast( player.transform.position, dir, out hit_info, 50.0f ) ) {
            if ( hit_info.collider.tag == "Enemy" ) {
                _txtRange.text = hit_info.distance.ToString( "###.#" ) + " M";
                _imgReticle.color = Color.red;
                return;
            }
        }
        _txtRange.text = "-- M";
        _imgReticle.color = Color.green;

        // Shields

        ShieldBars[ 0 ].enabled = ( GameController.instance.Info.Lives > 0 );
        ShieldBars[ 1 ].enabled = ( GameController.instance.Info.Lives > 1 );
        ShieldBars[ 2 ].enabled = ( GameController.instance.Info.Lives > 2 );
    }
}
