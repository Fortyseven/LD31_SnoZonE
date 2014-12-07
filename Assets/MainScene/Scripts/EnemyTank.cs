using UnityEngine;
using System.Collections;

public class EnemyTank : MonoBehaviour
{
    private TankMovement _treads;

    void Start()
    {
        _treads = GetComponent<TankMovement>();
        Utils.Assert(_treads);
    }

    void Update()
    {
        _treads.RotateLeft(1.0f);
        _treads.RotateRight( 1.0f );
    }
}
