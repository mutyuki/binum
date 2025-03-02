using UnityEngine;

public class Spawner : MonoBehaviour
{
    //次のブロックを受け取って生成してそれを返す
    public Block SpawnBlock(Block block)
    {
        block.transform.position = transform.position;
        if (block.CompareTag("I") || block.CompareTag("O"))
        {
            Vector3 adjustedPosition = block.transform.position;
            adjustedPosition.x += 0.5f;
            adjustedPosition.y += 0.5f;
            block.transform.position = adjustedPosition;
        }

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
