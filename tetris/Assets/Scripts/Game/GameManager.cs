using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Block activeBlock;
    NextSpawner nextSpawner;

    Block holdBlock;
    Block saveBlock;

    [SerializeField]
    private float dropInterval = 2f;
    float nextdropTimer = 2f;
    Board board;
    HoldSpawner holdSpawner;

    [SerializeField]
    GhostBlock ghostBlock;

    private bool holdcheck = true;

    float nextKeyDowntimer,
        nextKeyLeftRighttimer,
        nextKeyRotatetimer;

    [SerializeField]
    private float nextKeyDownInterval,
        nextKeyLeftRightInterval,
        nextKeyRotateInterval;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private Score score;

    bool gameOver;
    float beforerotationZ;
    float setuplimit = 0f;
    bool setup = false;

    [SerializeField]
    private SuperRotationSystem superRotationSystem;

    private void Start()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();
        board = GameObject.FindObjectOfType<Board>();
        nextSpawner = GameObject.FindObjectOfType<NextSpawner>();

        holdSpawner = GameObject.FindObjectOfType<HoldSpawner>();

        //スポナーの位置を綺麗に
        spawner.transform.position = Rounding.Round(spawner.transform.position);

        nextKeyDowntimer = Time.time + nextKeyDownInterval;

        nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;

        nextKeyRotatetimer = Time.time + nextKeyRotateInterval;

        if (!activeBlock)
        {
            activeBlock = GetNextBlock();
            ghostBlock.CreateGhostBlock(activeBlock);
        }
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private Block GetNextBlock()
    {
        // 次のブロックを取得
        Block nextBlock = nextSpawner.GetAndShiftNextBlock();

        return spawner.SpawnBlock(nextBlock);
    }

    //動く処理
    private void Update()
    {
        if (gameOver)
        {
            return;
        }
        if (activeBlock != null)
        {
            PlayerInput();
            ghostBlock.UpdateGhostBlock(activeBlock, board);
        }
    }

    void PlayerInput()
    {
        //ホールド
        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            Hold();
        }
        //ハードドロップ
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            while (board.IsWithinPosition(activeBlock))
            {
                activeBlock.MoveDown();
            }
            if (board.OverLimit(activeBlock))
            {
                GameOver();
            }
            else
            {
                //一個上げる
                activeBlock.MoveUp();
                BottomBoard();
            }
        }
        //右
        else if (
            (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                && (Time.time > nextKeyLeftRighttimer)
            || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.D)
        )
        {
            activeBlock.MoveRight();

            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.IsWithinPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        //左
        else if (
            (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                && (Time.time > nextKeyLeftRighttimer)
            || Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.A)
        )
        {
            activeBlock.MoveLeft();

            nextKeyLeftRighttimer = Time.time + nextKeyLeftRightInterval;
            if (!board.IsWithinPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        //右回転
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.L))
        {
            beforerotationZ = activeBlock.transform.eulerAngles.z;
            activeBlock.RotateRight();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.IsWithinPosition(activeBlock))
            {
                superRotationSystem.TryRotateLeftRight(activeBlock, 1, beforerotationZ, board);
            }
        }
        //左回転
        else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J))
        {
            beforerotationZ = activeBlock.transform.eulerAngles.z;
            activeBlock.RotateLeft();
            nextKeyRotatetimer = Time.time + nextKeyRotateInterval;
            if (!board.IsWithinPosition(activeBlock))
            {
                superRotationSystem.TryRotateLeftRight(activeBlock, 2, beforerotationZ, board);
            }
        }
        //下加速
        else
        {
            activeBlock.MoveDown();
            if (!board.IsWithinPosition(activeBlock))
            {
                setuplimit += Time.deltaTime;
                if (setuplimit >= 1f)
                {
                    setup = true;
                }
            }
            activeBlock.MoveUp();
            if (
                (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                    && (Time.time > nextKeyDowntimer)
                || Time.time > nextdropTimer
            )
            {
                activeBlock.MoveDown();

                nextKeyDowntimer = Time.time + nextKeyDownInterval;
                nextdropTimer = Time.time + dropInterval;
                if (!board.IsWithinPosition(activeBlock))
                {
                    //一個上げる
                    activeBlock.MoveUp();
                    if (setup)
                    {
                        BottomBoard();
                    }
                }
            }
        }
    }

    //底についたときの処理
    private async void BottomBoard()
    {
        CancelInvoke();
        while (board.IsWithinPosition(activeBlock))
        {
            activeBlock.MoveDown();
        }
        //一個上げる
        activeBlock.MoveUp();

        //座標を保存
        board.SaveBlockInGrid(activeBlock);
        if (board.OverLimit(activeBlock))
        {
            GameOver();
        }
        Block savedBlock = activeBlock;
        activeBlock = null;
        ghostBlock.DestroyGhostBlock();
        await BottomBoardCoroutine(savedBlock);
    }

    private async UniTask BottomBoardCoroutine(Block savedBlock)
    {
        await CheckAdjacentBlockNumbers(savedBlock);
        // 次のブロックをスポーン
        activeBlock = GetNextBlock();
        while (!board.IsWithinPosition(activeBlock))
        {
            activeBlock.MoveUp();
        }
        //ゴーストブロックの変更
        ghostBlock.CreateGhostBlock(activeBlock);

        holdcheck = true;

        setup = false;
        setuplimit = 0f;
        nextKeyDowntimer = Time.time;
        nextKeyLeftRighttimer = Time.time;
        nextKeyRotatetimer = Time.time;
        nextdropTimer = Time.time + dropInterval;
    }

    private async UniTask CheckAdjacentBlockNumbers(Block savedBlock)
    {
        if (savedBlock != null)
        {
            List<BlockPeace> blockPeaces = savedBlock
                .GetComponentsInChildren<BlockPeace>()
                .ToList();
            List<BlockPeace> highValueBlocks = new List<BlockPeace>();
            await MargeBlock(blockPeaces, highValueBlocks);
        }
    }

    private async UniTask MargeBlock(List<BlockPeace> blockPeaces, List<BlockPeace> highValueBlocks)
    {
        blockPeaces = blockPeaces.OrderBy(bp => bp.Number).ToList();
        HashSet<BlockPeace> movedBlocks = new HashSet<BlockPeace>();
        List<Vector3Int> positions = new List<Vector3Int>();
        for (int i = 0; i < blockPeaces.Count; i++)
        {
            BlockPeace blockPeace = blockPeaces[i];
            HashSet<BlockPeace> visited = new HashSet<BlockPeace>();
            ExploreBlock(blockPeace, visited);
            int count = visited.Count;
            if (count >= 2)
            {
                Vector3 newPos = visited
                    .OrderBy(bp => bp.transform.position.y)
                    .First()
                    .transform.position;

                await AnimationMargeBlock(visited, newPos, positions);

                int n = (int)Mathf.Pow(2, count - 1);
                int newNumber = n * blockPeace.Number;
                score.AddScore(newNumber);
                BlockPeace newBlockPeace = board.CreateNewBlock(newPos, newNumber);
                if (newBlockPeace.Number >= 256)
                {
                    highValueBlocks.Add(newBlockPeace);
                }
                await UniTask.WaitUntil(() => visited.All(bp => bp == null));
                blockPeaces.Add(newBlockPeace);
                blockPeaces = blockPeaces.OrderBy(bp => bp.Number).ToList();
            }
        }

        positions = positions.OrderByDescending(pos => pos.y).ToList();
        foreach (Vector3Int pos in positions)
        {
            movedBlocks.AddRange(board.ShiftRowsDownColumn(pos.x, pos.y + 1));
        }
        blockPeaces = movedBlocks.ToList();
        if (blockPeaces.Count > 0)
        {
            await MargeBlock(blockPeaces, highValueBlocks);
        }
        else
        {
            List<BlockPeace> rowsBlockPeaces = new List<BlockPeace>();
            foreach (BlockPeace blockPeace in highValueBlocks)
            {
                if (blockPeace != null)
                {
                    Vector3Int pos = Vector3Int.RoundToInt(blockPeace.transform.position);
                    rowsBlockPeaces.AddRange(board.ClearAllRows(pos.y));
                }
            }
            highValueBlocks = new List<BlockPeace>();
            if (rowsBlockPeaces.Count > 0)
            {
                await MargeBlock(rowsBlockPeaces, highValueBlocks);
            }
        }
    }

    private async UniTask AnimationMargeBlock(
        HashSet<BlockPeace> visited,
        Vector3 newPos,
        List<Vector3Int> positions
    )
    {
        var tasks = new List<UniTask>();
        foreach (BlockPeace bp in visited)
        {
            Vector3Int pos = Vector3Int.RoundToInt(bp.transform.position);
            board.RemoveBlock(pos);

            if (pos != Rounding.Round(newPos))
            {
                positions.Add(pos);
            }

            BlockMover blockMover = bp.gameObject.AddComponent<BlockMover>();
            tasks.Add(blockMover.MoveToPosition(newPos, 0.25f));
        }
        await UniTask.WhenAll(tasks);
    }

    private void ExploreBlock(BlockPeace blockPeace, HashSet<BlockPeace> visited)
    {
        Stack<BlockPeace> stack = new Stack<BlockPeace>();
        stack.Push(blockPeace);
        int count = 0;

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.left,
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.down,
        };

        while (stack.Count > 0)
        {
            BlockPeace current = stack.Pop();
            if (visited.Contains(current) || current == null)
            {
                continue;
            }

            visited.Add(current);
            count++;

            Vector2Int blockPosition = Rounding.RoundToInt(current.transform.position);
            foreach (Vector2Int direction in directions)
            {
                Vector2Int checkPosition = blockPosition + direction;
                Transform neighborTransform = board.GetBlockAtPosition(checkPosition);
                if (neighborTransform != null)
                {
                    BlockPeace adjacentBlockPeace = neighborTransform.GetComponent<BlockPeace>();
                    if (
                        adjacentBlockPeace != null
                        && adjacentBlockPeace.Number == current.Number
                        && !visited.Contains(adjacentBlockPeace)
                    )
                    {
                        stack.Push(adjacentBlockPeace);
                    }
                }
            }
        }
    }

    //ホールド機能
    void Hold()
    {
        if (holdcheck)
        {
            saveBlock = activeBlock;
            if (holdBlock != null)
            {
                activeBlock = spawner.SpawnBlock(holdBlock);
                //ゴーストブロックの変更
                ghostBlock.DestroyGhostBlock();
                ghostBlock.CreateGhostBlock(activeBlock);
            }
            else
            {
                activeBlock = GetNextBlock();
                //ゴーストブロックの変更
                ghostBlock.DestroyGhostBlock();
                ghostBlock.CreateGhostBlock(activeBlock);
            }
            holdBlock = holdSpawner.HoldBlock(saveBlock);
            holdcheck = false;
        }
    }

    //ゲームオーバー
    void GameOver()
    {
        activeBlock.MoveUp();
        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }
        gameOver = true;
    }

    //リトライ
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
