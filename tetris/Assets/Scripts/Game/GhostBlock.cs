using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GhostBlock : MonoBehaviour
{
    Block ghostBlock;
    // ゴーストブロックを作成
    public void CreateGhostBlock(Block activeBlock)
    {
        if (activeBlock != null)
        {
            ghostBlock = Instantiate(
                activeBlock,
                activeBlock.transform.position,
                activeBlock.transform.rotation
            );
            // ゴーストブロックの色や透明度を変更
            ChangeGhostAppearance();
        }
    }
    // ゴーストブロックの外観を変更
    void ChangeGhostAppearance()
    {
        foreach (Transform child in ghostBlock.transform)
        {
            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                // ゴーストブロックの透明度を設定
                Color color = renderer.color;
                color.a = 0f; // 透明度を設定
                renderer.color = color;

                // ゴーストブロックの描画順を後ろに設定
                renderer.sortingOrder = -1; // アクティブブロックより低い値にする
                // ゴーストブロックのすべての子オブジェクトからCanvasを持つものを取得して処理
                foreach (Canvas childCanvas in ghostBlock.GetComponentsInChildren<Canvas>())
                {
                    childCanvas.sortingOrder = -1;
                }
            }
        }
    }

    // ゴーストブロックをアップデート
    public void UpdateGhostBlock(Block activeBlock, Board board)
    {
        if (ghostBlock != null && activeBlock != null)
        {
            // ゴーストブロックをアクティブブロックと同じ位置に配置
            ghostBlock.transform.position = activeBlock.transform.position;
            ghostBlock.transform.rotation = activeBlock.transform.rotation;

            // ゴーストブロックを落下させる
            while (board.IsWithinPosition(ghostBlock))
            {
                ghostBlock.MoveDown();
            }

            // 1つ上に戻す（衝突する直前の位置にする）
            ghostBlock.MoveUp();
        }
    }
    public void DestroyGhostBlock()
    {
        if (ghostBlock != null)
        {
            Destroy(ghostBlock.gameObject);
        }
    }
}