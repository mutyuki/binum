using UnityEngine;
using System.Collections;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab; // ブロックのプレハブ
    public Vector3 spawnPosition;  // ブロックを生成する位置
    private GameObject currentBlock; // 現在のブロック

    void Start()
    {
        // 最初のブロックを生成
        SpawnNewBlock();
    }

    // 新しいブロックを生成する関数
    public void SpawnNewBlock()
    {
        if (currentBlock == null)
        {
            currentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

            // ブロックにランダムな数字を設定（2、4、8、16のいずれか）
            int[] possibleNumbers = { 2, 4, 8, 16 };
            int randomNumber = possibleNumbers[Random.Range(0, possibleNumbers.Length)];
            currentBlock.GetComponent<BlockController>().SetNumber(randomNumber);
        }
    }

    // ブロックが停止したときに呼び出される
    public void BlockStopped()
    {
        if (currentBlock != null)
        {
            currentBlock = null;
            StartCoroutine(DelayedSpawn());
        }
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        SpawnNewBlock();
    }
}
