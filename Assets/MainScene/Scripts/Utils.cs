using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Utils
{
    public static void Assert( bool expr )
    {
        if ( !expr ) {
            throw new UnityEngine.UnityException("ASSERTION FAILED");            
        }
    }
}
