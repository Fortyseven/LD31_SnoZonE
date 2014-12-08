using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class TextNonsense : MonoBehaviour
{
    private const int MAX_GIBBERS = 50;

    private Text _lblText;
    private StringBuilder[] _gibberish;

    /******************************************************************/
    void Start()
    {
        _lblText = GetComponent<Text>();

        _gibberish = new StringBuilder[ MAX_GIBBERS ];

        for ( int i = 0; i < MAX_GIBBERS; i++ ) {
            StringBuilder g = new StringBuilder();
            for ( int j = 0; j < 50; j++ ) {
                if ( Random.Range( 0, 2 ) == 0 )
                    g.Append( " " );
                else
                    g.Append( Random.Range( 0, 99 ) );
            }
            _gibberish[ i ] = g;
        }
    }

    /******************************************************************/
    void Update()
    {
        _lblText.text = _gibberish[ Random.Range( 0, MAX_GIBBERS ) ].ToString();
    }
}
