using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReticleController : MonoBehaviour
{
    public GameObject ReticleObject;

    public Image _imgReticle;

    void Start()
    {
        _imgReticle = ReticleObject.GetComponent<Image>();
        Utils.Assert( _imgReticle );
    }

    void Update()
    {

    }
}
