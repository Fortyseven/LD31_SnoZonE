using UnityEngine;
using System.Collections;

public class SnowmanAnimationHandler : MonoBehaviour
{
    public void OnSpawnAnimFinish()
    {
        transform.parent.SendMessage( "OnSpawn" );
    }

    public void OnDeathAnimFinish()
    {
        transform.parent.SendMessage( "OnDeathAnimFinish" );
    }
}
