using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    private bool FirstPush = false;

    public void PressStart()
    {
        if (!FirstPush)
        {
            SceneManager.LoadScene("GameScene");
            FirstPush = true;
        }
    }
}
