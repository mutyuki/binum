using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSpawner : MonoBehaviour
{
    public Block HoldBlock(Block block)
    {
        block.transform.position = transform.position;
        block.transform.rotation = Quaternion.identity;
        if (block)
        {
            return block;
        }
        else
        {
            return null;
        }
    }
}
