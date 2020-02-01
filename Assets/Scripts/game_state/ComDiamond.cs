using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class ComDiamond
{
    public Com this[InputDirection direction] => coms[direction];

    InputDirectionBox<Com> _coms;
    InputDirectionBox<Com> coms
    {
        get
        {
            if (_coms == null)
            {
                _coms = initializeComs();
            }
            return _coms;
        }
    }

    protected abstract InputDirectionBox<Com> initializeComs ();
}
