using UnityEngine;

class Utils
{
    public static void Assert( bool expr )
    {
        if ( !expr ) {
            throw new UnityException("ASSERTION FAILED");            
        }
    }
}