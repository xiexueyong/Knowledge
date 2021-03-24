using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleClass {

    private double _idleTime;
    public double IdleTime
    {
        get
        {
            return _idleTime;
        }
    }
    private bool _idle;
    public virtual bool Idle
    {
        get { return _idle; }
        set
        {
            _idle = value;
            if (_idle)
                _idleTime = Time.fixedTime;
        }
    }

    public virtual void CheckIdle()
    {
        Idle = false;
    }

}
