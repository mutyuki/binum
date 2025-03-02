using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    int tmp = 0;

    [SerializeField]
    TextMeshProUGUI scoreText;

    // スコアを追加する関数
    public void AddScore(int score)
    {
        tmp += score;
        scoreText.text = tmp.ToString();
    }
}
