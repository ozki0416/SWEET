using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    // �J������h�炷(����:�h��鎞��, �k�x)
    private IEnumerator DoShake(float duration, float magnitude)
    {
        var pos = transform.localPosition;  // ���݃J�����ʒu�̕ۑ�

        var elapsed = 0f;   // �o�ߎ��Ԃ̏�����

        if (elapsed < duration)
        {
            // XY���W�ɗ͂�������
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z); // �J�����ʒu�̍X�V

            elapsed += Time.deltaTime;  // �o�ߎ��Ԃ̉��Z

            yield return null;  // �ҋ@
        }

        transform.localPosition = pos;  // �J���������Ƃ̈ʒu�ɖ߂�
    }
}