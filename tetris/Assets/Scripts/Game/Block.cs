using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private bool canRotate = true;

    public List<int> peaceNumber = new List<int> { 2, 4, 8, 16 };

    [SerializeField]
    BlockPeace[] blockPeaces;

    //動き方
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    //各種動き
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    //回転
    public void RotateRight()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }

    public void RotateLeft()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }

    //ランダムなブロックの中身作成
    public void MakeRandomPeace()
    {
        foreach (BlockPeace blockPeace in blockPeaces)
        {
            int randomIndex = Random.Range(0, peaceNumber.Count);
            int randomPeaceNumber = peaceNumber[randomIndex];
            blockPeace.SetNumber(randomPeaceNumber);
        }
    }
}
