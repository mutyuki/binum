using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // 初期の回転を保持する
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // 初期の回転を維持する
        transform.rotation = initialRotation;
    }
}
