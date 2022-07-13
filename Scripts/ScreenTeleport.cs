using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTeleport
{
    private readonly GameObject _observedObject;

    public ScreenTeleport(GameObject observedObject)
    {
        _observedObject = observedObject;
    }

    public void CheckScreenBorders()
    {
        var camera = Camera.main;
        var higherPosition = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        var lowerPosition = camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var pos = _observedObject.transform.position;
            


        if (_observedObject.transform.position.x > higherPosition.x )
        {
            _observedObject.transform.position = new Vector3(lowerPosition.x, pos.y, 0);
        }
        else if (_observedObject.transform.position.x < lowerPosition.x)
        {
            _observedObject.transform.position = new Vector3(higherPosition.x, pos.y, 0);
        }
        else if (_observedObject.transform.position.y > higherPosition.y)
        {
            _observedObject.transform.position = new Vector3(pos.x, lowerPosition.y, 0);
        }
        else if (_observedObject.transform.position.y < lowerPosition.y)
        {
            _observedObject.transform.position = new Vector3(pos.x, higherPosition.y, 0);
        }
    }
}
