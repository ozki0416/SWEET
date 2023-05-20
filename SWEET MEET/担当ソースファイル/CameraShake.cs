using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    // カメラを揺らす(引数:揺れる時間, 震度)
    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;  // 現在カメラ位置の保存

        var elapsed = 0f;   // 経過時間の初期化

        if (elapsed < duration)
        {
            // XY座標に力を加える
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z); // カメラ位置の更新

            elapsed += Time.deltaTime;  // 経過時間の加算

            yield return null;  // 待機
        }

        transform.localPosition = pos;  // カメラをもとの位置に戻す
    }
}