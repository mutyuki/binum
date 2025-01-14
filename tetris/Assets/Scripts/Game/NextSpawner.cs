using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSpawner : MonoBehaviour
{
    [SerializeField]
    Block[] Blocks; // ブロックの配列

    [SerializeField]
    Transform[] nextBlockPositions; // 5つのブロックを表示する位置
    private Queue<Block> nextBlocksQueue = new Queue<Block>(); // 次のブロックを管理するキュー
    private List<Block> availableBlocks;
    Block nextBlock;

    void Awake()
    {
        ResetAvailableBlocks();
        // 最初の5個のブロックを生成してキューに追加
        for (int i = 0; i < nextBlockPositions.Length; i++)
        {
            Block newBlock = NextBlock();
            nextBlocksQueue.Enqueue(newBlock);
            DisplayNextBlocks();
        }
    }

    private void ResetAvailableBlocks()
    {
        availableBlocks = new List<Block>(Blocks);
    }

    public Block GetRandomBlock()
    {
        if (availableBlocks.Count == 0)
        {
            ResetAvailableBlocks();
        }

        int index = Random.Range(0, availableBlocks.Count);
        Block selectedBlock = availableBlocks[index];
        availableBlocks.RemoveAt(index);

        return selectedBlock;
    }

    public Block NextBlock()
    {
        Block block = Instantiate(GetRandomBlock(), transform.position, Quaternion.identity);
        block.MakeRandomPeace();
        return block;
    }

    // キュー内のブロックを表示
    public void DisplayNextBlocks()
    {
        Block[] blocksArray = nextBlocksQueue.ToArray();
        for (int i = 0; i < nextBlockPositions.Length; i++)
        {
            if (i < blocksArray.Length)
            {
                blocksArray[i].transform.position = nextBlockPositions[i].position;
            }
            else
            {
                // キューに含まれないブロックを非表示にする
                if (i < nextBlockPositions.Length && nextBlockPositions[i].childCount > 0)
                {
                    nextBlockPositions[i].GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    // 実際に使用するブロックを生成してキューを更新
    public Block GetAndShiftNextBlock()
    {
        // キューの一番前のブロックを取り出す
        nextBlock = nextBlocksQueue.Dequeue();

        // 新しいブロックをキューに追加
        Block newBlock = NextBlock();
        nextBlocksQueue.Enqueue(newBlock);

        // キュー内のブロックを更新して表示
        DisplayNextBlocks();

        return nextBlock;
    }
}
