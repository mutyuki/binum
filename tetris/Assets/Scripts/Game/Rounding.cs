using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public static class Rounding
{
    //数値の四捨五入
    public static Vector2 Round(Vector2 i)
    {
        return new Vector2(Mathf.Round(i.x), Mathf.Round(i.y));
    }

    public static Vector3 Round(Vector3 i)
    {
        return new Vector3(Mathf.Round(i.x), Mathf.Round(i.y), Mathf.Round(i.z));
    }

    public static Vector2Int RoundToInt(Vector3 i)
    {
        return new Vector2Int(Mathf.RoundToInt(i.x), Mathf.RoundToInt(i.y));
    }
}
