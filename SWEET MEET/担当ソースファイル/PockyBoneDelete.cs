using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PockyBoneDelete : MonoBehaviour
{
    GameObject player;      // �v���C���[���
    RigidOnOff ronoff;      // �|�b�L�[�㏸�X�N���v�g�̎擾

    public bool UDcheck;    // �㏸���t���O�������
    public bool collision = false;  // �v���C���[�Ɠ������Ă��邩�ǂ���

    // =====================================================================================================
    // Start is called before the first frame update
    // =====================================================================================================
    void Start()
    {
        player = GameObject.Find("Player");             // �v���C���[�擾
        ronoff = player.GetComponent<RigidOnOff>();     // �v���C���[���擾
    }

    // =====================================================================================================
    // Update is called once per frame
    // =====================================================================================================
    void Update()
    {

    }

    // =====================================================================================================
    // �ڐG����
    // =====================================================================================================
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collision = true;   // �ڐG
        }
    }

    // =====================================================================================================
    // �|�b�L�[���痣�ꂽ�ۂ�Bone������
    // =====================================================================================================
    private void OnTriggerExit(Collider other)
    {
        // �㏸���t���O�̎擾
        UDcheck = ronoff.UpDownCheck;

        if (other.gameObject.CompareTag("Player"))
        {
            collision = false;
        }

        if (other.gameObject.CompareTag("Player") && UDcheck)
        {
            // ��\��
            this.gameObject.SetActive(false);
        }
        // Bone����\���Ȃ�
        if (!this.gameObject.activeSelf)
        {
            // �ĕ\��
            Invoke(nameof(RebirthBone), 9.0f);
        }
    }

    // =====================================================================================================
    // Bone�̍ĕ\��
    // =====================================================================================================
    void RebirthBone()
    {
        this.gameObject.SetActive(true);    // �\��
    }
}
