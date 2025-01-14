using UnityEngine;
using TMPro;

public class BlockController : MonoBehaviour
{
    public int number; // ブロックに表示される数字
    public TMP_Text numberText; // TextMeshPro参照
    public float fallSpeed = 1.0f; // ブロックの落下速度（1マス落下ごとの遅延）
    public float gridSize = 1.0f; // グリッドサイズ
    public float moveDelay = 0.2f; // 左右移動の遅延（タイムインターバル）
    public float dropDelay = 1.0f; // ブロックの落下インターバル（1秒ごとに落下）

    private float fallTimer = 0.0f; // 落下タイマー
    private float moveTimer = 0.0f; // 左右移動タイマー
    private bool isFalling = true; // ブロックが落下中かどうか
    private bool isGrounded = false; // ブロックが地面に接触して停止したかどうか
    private bool hasStopped = false; // ブロックが完全に停止したか
    private BlockSpawner blockSpawner; // BlockSpawnerへの参照

    void Start()
    {
        blockSpawner = FindObjectOfType<BlockSpawner>();
        UpdateNumberDisplay(); // 初期の数字を表示
    }

    void Update()
    {
        if (isFalling && !isGrounded && !hasStopped)
        {
            HandleFalling();  // 落下処理
            HandleMovement(); // 左右移動の処理
        }
    }

    // ブロックを自動的に下に落とす処理（テトリスのように1マスごとに）
    void HandleFalling()
    {
        fallTimer += Time.deltaTime; // 時間の経過を追跡
        if (fallTimer >= dropDelay) // 指定された時間ごとに1マス落下
        {
            // 下に1マス（gridSize分）移動
            transform.position += new Vector3(0, -gridSize, 0);
            SnapToGrid(); // グリッドにスナップ
            fallTimer = 0.0f; // タイマーをリセット
        }
    }

    // 左右移動の処理（テトリスのように1マスごとに）
    void HandleMovement()
    {
        moveTimer += Time.deltaTime; // 時間の経過を追跡
        if (moveTimer >= moveDelay) // 一定時間ごとに左右に1マス移動
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                // 左に1マス（gridSize分）移動
                transform.position += new Vector3(-gridSize, 0, 0);
                SnapToGrid(); // グリッドにスナップ
                moveTimer = 0.0f; // タイマーをリセット
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                // 右に1マス（gridSize分）移動
                transform.position += new Vector3(gridSize, 0, 0);
                SnapToGrid(); // グリッドにスナップ
                moveTimer = 0.0f; // タイマーをリセット
            }
        }
    }

    // 座標をグリッドにスナップする処理（テトリスのように動きをグリッドに合わせる）
    void SnapToGrid()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x / gridSize) * gridSize,
            Mathf.Round(transform.position.y / gridSize) * gridSize,
            transform.position.z
        );
    }

    // 数字を設定するメソッド
    public void SetNumber(int newNumber)
    {
        number = newNumber;
        UpdateNumberDisplay();
    }

    // 数字をTextMeshProに表示する処理
    void UpdateNumberDisplay()
    {
        if (numberText != null)
        {
            numberText.text = number.ToString();
        }
    }

    // 衝突処理
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFalling && !isGrounded && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Block"))
        {
            isFalling = false;
            isGrounded = true;
            hasStopped = true;

            SnapToGrid(); // グリッドにスナップ
            blockSpawner.BlockStopped();
        }

        // 他のブロックと接触した場合に合成処理
        if (collision.gameObject.tag == "Block")
        {
            BlockController otherBlock = collision.gameObject.GetComponent<BlockController>();

            if (otherBlock.number == this.number)
            {
                MergeBlocks(otherBlock);
            }
        }
    }

    // ブロックを合体させる処理
    void MergeBlocks(BlockController otherBlock)
    {
        this.number *= 2;
        UpdateNumberDisplay(); // 数字を更新

        if (this.number >= 1024)
        {
            Destroy(this.gameObject); // 数字が1024以上なら消滅
        }
        else
        {
            Destroy(otherBlock.gameObject); // 他のブロックを消去
        }
    }
}
