using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabol : MonoBehaviour
{
    public Vector3 Parabola(Vector3 start , Vector3 end,float height, float time)
    {
        Func<float,float> f = x => -4 * height * x * x + 4 * height* x;
        Vector3 dir = Vector3.Lerp(start, end, time);
        return new Vector3(dir.x, Mathf.Lerp(start.y, end.y, time) + f(time), dir.z);
    }
}
