using UnityEngine;
using TMPro;

public class BlockPeace : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NumberText;
    public int Number { get; private set; }
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetNumber(int number)
    {
        Number = number;
        if (NumberText != null)
        {
            NumberText.text = Number.ToString();
        }
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = GetColorFromNumber(Number);
        }
    }

    private Color GetColorFromNumber(int number)
    {
        // Numberを基に色相(Hue)を計算。数値が大きくなると色相が変化する
        float hue = ((number * 1000) % 360) / 360f; // 0-1の範囲で色相を設定

        // 彩度(Saturation)は固定値で、鮮やかな色を出す。必要に応じて調整。
        float saturation = ((number * 10000) % 1283) / 1283f;

        // 明度(Value)は、ある程度の範囲で変化させ、白と被らないようにする
        float value = Mathf.Clamp(0.3f + (Mathf.Abs(Mathf.Sin(100 * number) * 0.5f)), 0.3f, 0.8f); //0.3から0.8の範囲で明度を変動

        Color hsvColor = Color.HSVToRGB(hue, saturation, value);
        return hsvColor;
    }
}
