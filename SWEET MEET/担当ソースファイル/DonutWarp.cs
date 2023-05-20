using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DonutWarp : MonoBehaviour
{
    public GameObject obj;      // �v���C���[
    public DonutWarp transObj;  // ���[�v��̃I�u�W�F�N�g
    private Vector3 transVec;   // ���[�v��̍��W
    public bool moveStatus;     // �ړ����
    private bool test;

    private PlayerStatus donutStatus;   // �h�[�i�c�׸ޗp
    private WarpEff wf;

    // =====================================================================================================
    // Start is called before the first frame update
    // =====================================================================================================
    void Start()
    {
        transVec = transObj.transform.position;         // ���[�v��̍��W���擾����
        moveStatus = true;      // ���[�v�\

        donutStatus = obj.GetComponent<PlayerStatus>(); // �J�������o�ؑ֗p
        wf = GetComponent<WarpEff>();
    }

    // =====================================================================================================
    // Update is called once per frame
    // =====================================================================================================
    void Update()
    {

    }

    // =====================================================================================================
    // �������ă��[�v
    // =====================================================================================================
    void OnTriggerStay(Collider other)
    {
        #region �L�[�{�[�h
        // ���[�v�\��Ԃ̂݃��[�v����
        if (moveStatus && transObj.gameObject.activeSelf)
        {
            if (Keyboard.current.zKey.isPressed)
            {
                // �J������Ԃ�ύX
                donutStatus.donutFlg = true;
                // �G�t�F�N�g
                wf.SetInstantiateMode(false);
                // ���[�v����
                Invoke(nameof(warp), 1);
                // �h�[�i�c���\��
                this.gameObject.SetActive(false);
                // ��\���Ȃ�ĕ\��
                if (!this.gameObject.activeSelf)
                {
                    wf.SetInstantiateMode(true);
                    Invoke(nameof(DonutRebirth), 5);
                }
            }
        }
        #endregion

        #region �R���g���[���[
        if (moveStatus && transObj.gameObject.activeSelf)
        {
            if (Gamepad.current != null)
            {
                if (Gamepad.current.rightTrigger.isPressed)
                {
                    // �J������Ԃ�ύX
                    donutStatus.donutFlg = true;
                    // �G�t�F�N�g
                    wf.SetInstantiateMode(false);
                    // ���[�v����
                    Invoke(nameof(warp), 1);
                    // �h�[�i�c���\��
                    this.gameObject.SetActive(false);
                    // ��\���Ȃ�ĕ\��
                    if (!this.gameObject.activeSelf)
                    {
                        Invoke(nameof(DonutRebirth), 5);
                    }
                }
            }
        }
        #endregion
    }

    // =====================================================================================================
    // ��x����Ă���ă��[�v
    // =====================================================================================================
    void OnTriggerExit(Collider other)
    {
        moveStatus = true;  // ���[�v�\
    }

    // =====================================================================================================
    // �{�`�������[�v����
    // =====================================================================================================
    void warp()
    {
        transObj.moveStatus = false;            // �����ɂ̓��[�v�s��
        obj.transform.position = transVec;      // �ړI�̍��W�܂ňړ�
    }

    // =====================================================================================================
    // �h�[�i�c�̍ĕ\��
    // =====================================================================================================
    void DonutRebirth()
    {
        this.gameObject.SetActive(true);
    }
}
