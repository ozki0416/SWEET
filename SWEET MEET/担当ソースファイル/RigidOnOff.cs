using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidOnOff : MonoBehaviour
{
    #region �ϐ��錾��

    private     Rigidbody       rb;                         // ���g��Rigidbody���

    private     Vector2         PlayerPos;                  // ���݈ʒu
    private     GameObject      player;                     // ���g
    private     PlayerMove      playerMove;                 // �v���C���[�̒ʏ�ړ����
    public      float           angle;                      // �|�b�L�[�ړ����Ƀv���C���[�ɗ^����p�x(�c�p)
    public      float           angls;                      // �|�b�L�[�ړ����Ƀv���C���[�ɗ^����p�x(���p)

    private     GameObject      nearObj;                    // �ł��߂��I�u�W�F�N�g(�c�n�_�Q�Ɨp)
    private     GameObject      nearObjS;                   // �ł��߂��I�u�W�F�N�g(���n�_�Q�Ɨp)

    public      bool            PockyStartFlg;              // �|�b�L�[�H�ׂ��܂���
    public      bool            UpDownCheck;                // ���̃|�b�L�[�ړ��͏㉺�ł����H
    public      bool            RightCheck;                 // ���̃|�b�L�[�ړ��͍��E�ł����H
    private     GameObject[]    High, Low;                  // �|�b�L�[�̒[(��[, ���[)
    private     GameObject[]    Right, Left;                // �|�b�L�[�̒[(�E�[, ���[)
    
    public      bool            pockyBiteFlg = false;       // �|�b�L�[���������Ă����
    public      bool            PockyAnim;                  // �v���C���[�̃A�j���[�V�����t���O

    private     StratFrom       ePos;                       // �G�t�F�N�g�p

    #endregion

    public enum StratFrom
    {
        NoSignal,
        Above = 0,
        Right = 1,
        Left = 2,
        Below = 3
    }

    // =====================================================================================================
    // Start is called before the first frame update
    // =====================================================================================================
    void Start()
    {
        PockyStartFlg = false;
        UpDownCheck = false;
        RightCheck = false;

        // �߂��|�b�L�[�������擾
        nearObj = SearchTag(gameObject, "PockyCenter");
        nearObjS = SearchTag(gameObject, "PockyCenter_S");

        player = GameObject.Find("Player"); // �v���C���[�擾
        playerMove = player.GetComponent<PlayerMove>(); // �v���C���[�ړ����擾
            
        #region �|�b�L�[�̎n�_�E�I�_�^�O�̈ꊇ�擾
        High = GameObject.FindGameObjectsWithTag("PockyEnd");
        Low = GameObject.FindGameObjectsWithTag("PockyStart");

        Right = GameObject.FindGameObjectsWithTag("PockyStart_S");
        Left = GameObject.FindGameObjectsWithTag("PockyEnd_S");
        #endregion

        rb = GetComponent<Rigidbody>(); // Rigidbody�̏����擾
        ePos = StratFrom.NoSignal;
    }

    // =====================================================================================================
    // Update is called once per frame
    // =====================================================================================================
    void Update()
    {
         //�ł��߂������I�u�W�F�N�g���擾��������
         nearObj = SearchTag(gameObject, "PockyCenter");
         nearObjS = SearchTag(gameObject, "PockyCenter_S");  // ���|�b�L�[�̒��S�_

        // �|�b�L�[�ƃv���C���[�̈ʒu�ɉ����ă|�b�L�[�n�_��ύX����
        if (nearObj != null) ChangePockyTag(); 
        if (nearObjS != null) ChangePockyTag();
    }

    // =====================================================================================================
    // �|�b�L�[�ł̈ړ�
    // =====================================================================================================
    void OnTriggerStay(Collider other)
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody�̏����擾
        PlayerPos = transform.position; // �v���C���[�̈ʒu���擾

        if (pockyBiteFlg && !PockyStartFlg)
        {
            PockyStartFlg = true;   // �ړ�����
            GetComponent<PlayerMove>().move = new Vector2(0.0f, 0.0f);  // ���O�܂ł̉����x������
        }

        // �n�_�ɐG�ꂽ��
        if (PockyStartFlg == true)
        {
            #region �㉺�ړ�
            // �㉺�ړ�
            if (other.CompareTag("PockyBone"))
            {
                // �v���C���[�̈ړ��ʂ𖳂���
                GetComponent<PlayerMove>().move = new Vector2(0.0f, 0.0f);
                // �v���C���[����]������p�̕ϐ�
                angle = 0;
                // �A�j���[�V�����ύX
                PockyAnim = true;
                // �p�x�����߂�
                if (High[0].tag == "PockyStart")
                {
                    // �A�j���[�V�����ύX
                    PockyAnim = true;
                    // �G�t�F�N�g
                    ePos = StratFrom.Above;
                    // �������ɂȂ�悤��]
                    angle = -90;
                    // �ړ�
                    this.transform.position = new Vector3(other.transform.position.x, this.transform.position.y - 0.2f, 0.0f);
                }
                if (Low[0].tag == "PockyStart")
                {
                    // �A�j���[�V�����ύX
                    PockyAnim = true;
                    // �G�t�F�N�g
                    ePos = StratFrom.Below;
                    // ������ɂȂ�悤��]
                    angle = 90;
                    // �d�͖���
                    rb.useGravity = false;
                    // �ړ�
                    this.transform.position = new Vector3(other.transform.position.x, this.transform.position.y + 0.2f, 0.0f);
                }
                // �p�x�ݒ�
                transform.eulerAngles = new Vector3(0, 0, angle);
            }
            #endregion

            #region ���E�ړ�
            // ���E�ړ�
            else if (other.CompareTag("PockyBone_S"))
            {
                // �v���C���[����]������p�̕ϐ�
                float angls = 0.0f;
                // �A�j���[�V�����ύX
                PockyAnim = true;
                // �p�x�����߂�
                if (Right[0].tag == "PockyStart_S")
                {
                    // �A�j���[�V�����ύX
                    PockyAnim = true;
                    // �p�x�߂��p
                    RightCheck = true;
                    // �G�t�F�N�g
                    ePos = StratFrom.Right;
                    // �������ɂȂ�悤��]
                    angls = -180;
                    // �ړ�
                    this.transform.position = new Vector3(this.transform.position.x - 0.2f, other.transform.position.y, 0.0f);
                }
                if (Left[0].tag == "PockyStart_S")
                {
                    // �A�j���[�V�����ύX
                    PockyAnim = true;
                    // �G�t�F�N�g
                    ePos = StratFrom.Left;
                    // �E�����ɂȂ�悤��]
                    angls = 0;
                    // �ړ�
                    this.transform.position = new Vector3(this.transform.position.x + 0.2f, other.transform.position.y, 0.0f);
                }
                // �p�x�ݒ�
                transform.eulerAngles = new Vector3(0, angls, 0);
            }
            #endregion
        }
        else
        {  
            PockyAnim = false;  // �A�j���[�V�����I��
        }
    }

    // =====================================================================================================
    // �|�b�L�[�ړ��̏I��
    // =====================================================================================================
    //public void PockyMoveFinish()
    //{
    //    rb.useGravity = true;           // �d�͗L��
    //    PockyStartFlg = false;          // �������܂���           // �ړ��I��
    //    playerMove.jumpFlg = false;
    //    playerMove.onGroundFlg = false;
    //    transform.eulerAngles = new Vector3(0, 0, 0);   // �v���C���[�̊p�x��߂�
    //    if (UpDownCheck)
    //    {
    //        playerMove.move.y = 0.0f;
    //        playerMove.move.y = 60.0f;
    //        UpDownCheck = false;
    //    }
    //    else
    //    {
    //        playerMove.move.y = 30.0f;
    //        playerMove.move.x = 60.0f;
    //    }
    //    pockyBiteFlg = false;
    //}

    public void PockyMoveFinish(Vector3 pos)
    {
        // �d�͗L��
        rb.useGravity = true;
        // �|�b�L�[�ړ��̏I��
        PockyStartFlg = false;
        // �W�����v�s��
        playerMove.jumpFlg = false;
        // �ڒn���Ă��Ȃ�
        playerMove.onGroundFlg = false;
        // �v���C���[�̊p�x��߂�
        if (!RightCheck) transform.eulerAngles = new Vector3(0, 0, 0);
        if (RightCheck)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            RightCheck = false;
        }
        // �ʒu�ύX
        this.transform.position = pos;
        // �|�b�L�[�I�����ɒ��˂�
        if (UpDownCheck)    
        {
            playerMove.move.y = 0.0f;
            playerMove.move.y = 60.0f;  // �㉺�ړ����ɂ͏�ɒ��˂�
            UpDownCheck = false;
        }
        else
        {
            playerMove.move.y = 30.0f;
            playerMove.move.x = 60.0f;  // ���E�ړ����ɂ͉��ɒ��˂�
        }
        // �|�b�L�[�H�׏I���܂���
        pockyBiteFlg = false;
    }

    // =====================================================================================================
    // �v���C���[�ɋ߂��I�u�W�F�N�g�̃^�O���Q��
    // =====================================================================================================
    GameObject SearchTag(GameObject nowObj, string tagName)
    {
        float tmpDis = 0;               //�����p�ꎞ�ϐ�
        float nearDis = 0;              //�ł��߂��I�u�W�F�N�g�̋���
        GameObject targetObj = null;    //�I�u�W�F�N�g

        //�^�O�w�肳�ꂽ�I�u�W�F�N�g��z��Ŏ擾����
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            //���g�Ǝ擾�����I�u�W�F�N�g�̋������擾
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //�I�u�W�F�N�g�̋������߂����A����0�ł���΃I�u�W�F�N�g�����擾
            //�ꎞ�ϐ��ɋ������i�[
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }

        }
        //�ł��߂������I�u�W�F�N�g��Ԃ�
        return targetObj;
    }

    // =====================================================================================================
    // �v���C���[�̈ʒu�ɉ����ă|�b�L�[�̎n�_��ύX
    // =====================================================================================================
    public void ChangePockyTag()
    {
        #region �c�|�b�L�[�̎n�_�ύX
        // �v���C���[���|�b�L�[�̒��S�ȉ��E���[�̃^�O��Start����Ȃ��E�ړ����Ă��Ȃ�
        if (nearObj != null)
        {
            if ((this.gameObject.transform.position.y <=
            nearObj.gameObject.transform.position.y) && (Low[0].tag != "PockyStart") && !PockyStartFlg)
            {
                foreach (var highTags in High)
                {
                    highTags.tag = "PockyEnd";
                }
                foreach (var lowTags in Low)
                {
                    lowTags.tag = "PockyStart";
                }
            }

            // �v���C���[���|�b�L�[�̒��S�ȏ�E��[�̃^�O��Start����Ȃ��E�ړ����Ă��Ȃ�
            if ((this.gameObject.transform.position.y >=
                nearObj.gameObject.transform.position.y) && (High[0].tag != "PockyStart") && !PockyStartFlg)
            {
                foreach (var highTags in High)
                {
                    highTags.tag = "PockyStart";
                }
                foreach (var lowTags in Low)
                {
                    lowTags.tag = "PockyEnd";
                }
            }
        }
        #endregion

        #region ���|�b�L�[�̎n�_�ύX
        // �v���C���[���|�b�L�[�̒��S�ȏ�E�E�[�̃^�O��Start����Ȃ��E�ړ����Ă��Ȃ�
        if (nearObjS != null)
        {
            if ((this.gameObject.transform.position.x >=
                nearObjS.gameObject.transform.position.x) && (Right[0].tag != "PockyStart_S") && !PockyStartFlg)
            {
                foreach (var rightTags in Right)
                {
                    rightTags.tag = "PockyStart_S";
                }
                foreach (var leftTags in Left)
                {
                    leftTags.tag = "PockyEnd_S";
                }
            }
            // �v���C���[���|�b�L�[�̒��S�ȉ��E���[�̃^�O��Start����Ȃ��E�ړ����Ă��Ȃ�
            if ((this.gameObject.transform.position.x <=
            nearObjS.gameObject.transform.position.x) && (Left[0].tag != "PockyStart_S") && !PockyStartFlg)
            {
                foreach (var rightTags in Right)
                {
                    rightTags.tag = "PockyEnd_S";
                }
                foreach (var leftTags in Left)
                {
                    leftTags.tag = "PockyStart_S";
                }
            }
        }
        #endregion

    }

    // =====================================================================================================
    // �G�t�F�N�g�擾
    // =====================================================================================================
    public StratFrom GetStartFrom()
    {
        return ePos;
    }

    public void SetResetStratFrom(StratFrom eST)
    {
        ePos = eST;
    }
}