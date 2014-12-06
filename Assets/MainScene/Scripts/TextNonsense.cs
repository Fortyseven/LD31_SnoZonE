using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class TextNonsense : MonoBehaviour
{
    private Text _lblText;
    private StringBuilder _gibberish;
    void Start()
    {
        _lblText = GetComponent<Text>();
        _gibberish = new StringBuilder();
    }


    void Update()
    {

        //TODO: Pregenerate these at Start instead of creating new every time
        _gibberish.Length = 0;
        for ( int i = 0; i < 50; i++ ) {
            if ( Random.Range( 0, 2 ) == 0 )
                _gibberish.Append( " " );
            else
                _gibberish.Append( Random.Range( 0, 99 ) );
        }
        _lblText.text = _gibberish.ToString();
    }
}
