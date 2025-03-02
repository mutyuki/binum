using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Transform[,] grid;

    [SerializeField]
    private Transform tetrisFlame;

    [SerializeField]
    private BlockPeace blockPeace;
    private int height = 30,
        width = 10,
        header = 8; //ボードの大きさ

    private void Awake()
    {
        grid = new Transform[width, height];
    }

    private void Start()
    {
        CreateBoard();
    }

    //フィールドの作成
    void CreateBoard()
    {
        if (tetrisFlame)
        {
            for (int y = 0; y < height - header; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone = Instantiate(
                        tetrisFlame,
                        new Vector3(x, y, 0),
                        Quaternion.identity
                    );
                    clone.transform.parent = transform;
                }
            }
        }
    }

    //はみ出てないかのチェック
    public bool IsWithinPosition(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            if (!BoardOutCheck((int)pos.x, (int)pos.y))
            {
                return false;
            }
            if (BlockCheck((int)pos.x, (int)pos.y, block))
            {
                return false;
            }
        }
        return true;
    }

    //枠内判定
    bool BoardOutCheck(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    //他のブロックがないか判定
    bool BlockCheck(int x, int y, Block block)
    {
        return (grid[x, y] != null && grid[x, y].parent != block.transform);
    }

    //ブロックの座標を記録
    public void SaveBlockInGrid(Block block)
    {
        foreach (Transform item in block.transform)
        {
            Vector2 pos = Rounding.Round(item.position);

            grid[(int)pos.x, (int)pos.y] = item;
        }
    }

    //消す処理
    public List<BlockPeace> ClearAllRows(int y)
    {
        {
            ClearRow(y);
            return ShiftRowsDown(y + 1);
        }
    }

    //全部埋まってるかの確認
    bool IsComplate(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    //一列消す
    void ClearRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
            }
            grid[x, y] = null;
        }
    }

    //上にある奴らを一個落とす
    List<BlockPeace> ShiftRowsDown(int startY)
    {
        List<BlockPeace> RowsPeace = new List<BlockPeace>();
        for (int x = 0; x < width; x++)
        {
            RowsPeace.AddRange(ShiftRowsDownColumn(x, startY));
        }
        return RowsPeace;
    }

    public List<BlockPeace> ShiftRowsDownColumn(int x, int startY)
    {
        List<BlockPeace> movedBlocks = new List<BlockPeace>();
        for (int y = startY; y < height; y++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
                BlockPeace blockPeace = grid[x, y - 1].GetComponent<BlockPeace>();
                if (blockPeace != null)
                {
                    movedBlocks.Add(blockPeace);
                }
            }
        }
        return movedBlocks;
    }

    //ブロックが上についてしまったか
    public bool OverLimit(Block block)
    {
        foreach (Transform item in block.transform)
        {
            if (item.transform.position.y + 1 >= height - header)
            {
                return true;
            }
        }
        return false;
    }

    // 指定された位置にあるブロックを取得するメソッド
    public Transform GetBlockAtPosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < width && position.y >= 0 && position.y < height)
        {
            return grid[position.x, position.y];
        }
        return null;
    }

    public void RemoveBlock(Vector3Int position)
    {
        if (position.x >= 0 && position.x < width && position.y >= 0 && position.y < height)
        {
            if (grid[position.x, position.y] != null)
            {
                grid[position.x, position.y] = null;
            }
        }
    }

    public BlockPeace CreateNewBlock(Vector3 newPos, int newNumber)
    {
        BlockPeace newBlockPeace = Instantiate(blockPeace, newPos, Quaternion.identity);
        newBlockPeace.SetNumber(newNumber);
        Vector2Int gridPos = Rounding.RoundToInt(newPos);
        if (gridPos.x >= 0 && gridPos.x < width && gridPos.y >= 0 && gridPos.y < height)
        {
            grid[gridPos.x, gridPos.y] = newBlockPeace.transform;
        }
        return newBlockPeace;
    }
}
