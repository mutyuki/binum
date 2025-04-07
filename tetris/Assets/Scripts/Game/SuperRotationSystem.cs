using UnityEngine;

public class SuperRotationSystem : MonoBehaviour
{
    //回転の処理
    public void TryRotateLeftRight(Block block, int rotate, float beforerotationZ, Board board)
    {
        float afterrotationZ = block.transform.eulerAngles.z;
        Vector3 savePosition = block.transform.position;
        Vector3 savePosition1 = block.transform.position;
        Vector3 savePosition2 = block.transform.position;
        if (block.CompareTag("I"))
        {
            switch (afterrotationZ)
            {
                //B
                case 270:
                    switch (beforerotationZ)
                    {
                        //A
                        case 0:
                            for (int i = 0; i < 2; ++i)
                            {
                                block.MoveLeft();
                            }
                            savePosition1 = block.transform.position;
                            break;
                        //C
                        case 180:
                            block.MoveRight();
                            savePosition1 = block.transform.position;
                            break;
                    }
                    break;
                //D
                case 90:
                    switch (beforerotationZ)
                    {
                        //A
                        case 0:
                            block.MoveLeft();
                            savePosition1 = block.transform.position;
                            break;
                        //C
                        case 180:
                            for (int i = 0; i < 2; i++)
                            {
                                block.MoveRight();
                            }
                            savePosition1 = block.transform.position;
                            break;
                    }
                    break;
                //A
                case 0:
                    switch (beforerotationZ)
                    {
                        //D
                        case 90:
                            for (int i = 0; i < 2; i++)
                            {
                                block.MoveLeft();
                            }

                            savePosition1 = block.transform.position;
                            break;
                        //B
                        case 270:
                            for (int i = 0; i < 2; i++)
                            {
                                block.MoveRight();
                            }
                            savePosition1 = block.transform.position;
                            break;
                    }
                    break;
                //C
                case 180:
                    switch (beforerotationZ)
                    {
                        //B
                        case 270:
                            block.MoveLeft();
                            savePosition1 = block.transform.position;
                            break;
                        //D
                        case 90:
                            block.MoveRight();
                            savePosition1 = block.transform.position;
                            break;
                    }
                    break;
            }

            if (!board.IsWithinPosition(block))
            {
                switch (afterrotationZ)
                {
                    //D
                    case 90:
                    //B
                    case 270:
                        switch (beforerotationZ)
                        {
                            //A
                            case 0:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveRight();
                                }
                                savePosition2 = block.transform.position;
                                break;
                            //C
                            case 180:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveLeft();
                                }
                                savePosition2 = block.transform.position;
                                break;
                        }
                        break;
                    //A
                    case 0:
                        switch (beforerotationZ)
                        {
                            //D
                            case 90:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveRight();
                                }
                                savePosition2 = block.transform.position;
                                break;
                            //B
                            case 270:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveLeft();
                                }
                                savePosition2 = block.transform.position;
                                break;
                        }
                        break;
                    //C
                    case 180:
                        switch (beforerotationZ)
                        {
                            //D
                            case 90:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveLeft();
                                }
                                savePosition2 = block.transform.position;
                                break;
                            //B
                            case 270:
                                for (int i = 0; i < 3; i++)
                                {
                                    block.MoveRight();
                                }
                                savePosition2 = block.transform.position;
                                break;
                        }
                        break;
                }
                if (!board.IsWithinPosition(block))
                {
                    switch (afterrotationZ)
                    {
                        //B
                        case 270:
                            block.transform.position = savePosition1;
                            for (int i = 0; i < rotate; i++)
                            {
                                block.MoveDown();
                            }
                            break;
                        //D
                        case 90:
                            block.transform.position = savePosition1;
                            for (int i = 0; i < rotate; i++)
                            {
                                block.MoveUp();
                            }
                            break;
                        //A
                        case 0:
                        //C
                        case 180:
                            switch (beforerotationZ)
                            {
                                //B
                                case 270:
                                    block.transform.position = savePosition1;
                                    for (int i = 2; i > 0; i = i - rotate)
                                    {
                                        block.MoveUp();
                                    }
                                    break;
                                //D
                                case 90:
                                    block.transform.position = savePosition2;
                                    for (int i = 2; i > 0; i = i - rotate)
                                    {
                                        block.MoveDown();
                                    }
                                    break;
                            }
                            break;
                    }

                    if (!board.IsWithinPosition(block))
                    {
                        switch (afterrotationZ)
                        {
                            //B
                            case 270:
                                block.transform.position = savePosition2;
                                for (int i = 2; i > 0; i = i - rotate)
                                {
                                    block.MoveUp();
                                }
                                break;
                            //D
                            case 90:
                                block.transform.position = savePosition2;
                                for (int i = 2; i > 0; i = i - rotate)
                                {
                                    block.MoveDown();
                                }
                                break;
                            //A
                            case 0:
                            //C
                            case 180:
                                switch (beforerotationZ)
                                {
                                    //D
                                    case 90:
                                        block.transform.position = savePosition1;
                                        for (int i = 0; i < rotate; i++)
                                        {
                                            block.MoveUp();
                                        }
                                        break;
                                    //B
                                    case 270:
                                        block.transform.position = savePosition2;
                                        for (int i = 0; i < rotate; i++)
                                        {
                                            block.MoveDown();
                                        }
                                        break;
                                }
                                break;
                        }
                        if (!board.IsWithinPosition(block))
                        {
                            block.transform.position = savePosition;
                            switch (rotate)
                            {
                                case 1:
                                    block.RotateLeft();
                                    break;
                                case 2:
                                    block.RotateRight();
                                    break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            switch (afterrotationZ)
            {
                //B
                case 270:
                    block.MoveLeft();
                    break;
                //D
                case 90:
                    block.MoveRight();
                    break;
                //A
                case 0:
                //C
                case 180:
                    switch (beforerotationZ)
                    {
                        //D
                        case 90:
                            block.MoveLeft();
                            break;
                        //B
                        case 270:
                            block.MoveRight();
                            break;
                    }
                    break;
            }
            if (!board.IsWithinPosition(block))
            {
                switch (afterrotationZ)
                {
                    //D
                    case 90:
                    //B
                    case 270:
                        block.MoveUp();
                        break;
                    //A
                    case 0:
                    //C
                    case 180:
                        block.MoveDown();
                        break;
                }
                if (!board.IsWithinPosition(block))
                {
                    block.transform.position = savePosition;
                    switch (afterrotationZ)
                    {
                        //D
                        case 90:
                        //B
                        case 270:
                            for (int i = 0; i < 2; ++i)
                            {
                                block.MoveDown();
                            }
                            break;
                        //A
                        case 0:
                        //C
                        case 180:
                            for (int i = 0; i < 2; ++i)
                            {
                                block.MoveUp();
                            }
                            break;
                    }
                    if (!board.IsWithinPosition(block))
                    {
                        switch (afterrotationZ)
                        {
                            //B
                            case 270:
                                block.MoveLeft();
                                break;
                            //D
                            case 90:
                                block.MoveRight();
                                break;
                            //A
                            case 0:
                            //C
                            case 180:
                                switch (beforerotationZ)
                                {
                                    //B
                                    case 270:
                                        block.MoveRight();
                                        break;
                                    //D
                                    case 90:
                                        block.MoveLeft();
                                        break;
                                }
                                break;
                        }
                        if (!board.IsWithinPosition(block))
                        {
                            block.transform.position = savePosition;
                            switch (rotate)
                            {
                                case 1:
                                    block.RotateLeft();
                                    break;
                                case 2:
                                    block.RotateRight();
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
