using Cysharp.Threading.Tasks;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    public async UniTask MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        transform.position = targetPosition;
        Destroy(gameObject);
    }
}
