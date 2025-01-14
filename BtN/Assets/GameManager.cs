using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject blockPrefab; // ブロックのプレハブ
    public Vector3 spawnPosition = new Vector3(0, 30, 0); // スポーンする位置

    // 新しいブロックをスポーンする関数
    public void SpawnNewBlock()
    {
        Instantiate(blockPrefab, spawnPosition, Quaternion.identity); // ブロックを生成
    }
}

