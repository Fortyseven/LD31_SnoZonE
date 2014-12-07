using UnityEngine;
using System.Collections;

public class EnemyTank : MonoBehaviour
{
    private TankMovement _treads;
    private bool _ready;

    void Start()
    {
        _treads = GetComponent<TankMovement>();
        Utils.Assert( _treads );
        _ready = false;
    }

    void Update()
    {
        if ( !_ready )
            return;

        _treads.RotateLeft( 1.0f );
        _treads.RotateRight( 1.0f );
    }

    public void OnSpawn()
    {
        _ready = true;
        GetComponentInChildren<Animation>().Play("SnowmanWaddle");
    }

    public void OnCollisionEnter(Collision col)
    {
        //if (col.other.tag.Equals("Projectile")) {
        if ( col.gameObject.tag.Equals("Projectile")) {
            Destroy(this.gameObject);
            Destroy(col.gameObject);
        }
    }
}
