using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PockyDelete : MonoBehaviour
{
    private FoodWaste fw;           // �p�[�e�B�N�����
    public GameObject player;       // �v���C���[���
    private bool EatFlg;            // �������Ԏ擾�p
    RigidOnOff flg;                 // �ϐ�(�t���O)�擾�p
    public AudioClip audioClip;     // �����Đ��p

    // �G�t�F�N�g
    private FACrePockyEff FA_cpe;
    private FBCrePockyEff FB_cpe;
    private FLCrePockyEff FL_cpe;
    private FRCrePockyEff FR_cpe;

    private bool bLoop;
    private Vector3 Vec;

    // =====================================================================================================
    // Start is called before the first frame update
    // =====================================================================================================
    void Start()
    {
        flg = player.GetComponent<RigidOnOff>();
        bLoop = false;
        if (tag == "PockyStart")
        {
            Vec = this.transform.position;
        }
        else
        {
            Vec = new Vector3(0.0f, 0.0f, 0.0f);
        }

        //audioSource = GetComponent<AudioSource>();
    }

    // =====================================================================================================
    // Update is called once per frame
    // =====================================================================================================
    void Update()
    {
        //EatFlg = flg.PockyStartFlg;
    }

    // =====================================================================================================
    // �v���C���[�ɐG�ꂽ��|�b�L�[��1���폜
    // =====================================================================================================
    void OnCollisionStay(Collision collision)
    {
        EatFlg = flg.PockyStartFlg;

        if (collision.gameObject.name == "Player" && EatFlg)
        {
            // �v���C���[�ɐG�ꂽ��̏������̒��Ȃ̂�collision�g���Ă�
            AudioSource audioSource = collision.gameObject.GetComponent<AudioSource>();
            // �I�u�W�F�N�g��\��
            this.gameObject.SetActive(false);
            // �p�[�e�B�N������
            fw = GetComponent<FoodWaste>();
            fw.CreateParticle(collision, (int)flg.GetStartFrom(), this.tag);
            if(Gamepad.current != null)
            {
                // �R���g���[���[�U��
                Gamepad.current.SetMotorSpeeds(0.3f, 0.0f);
                Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
            }

            #region SetPos
            // �|�b�L�[�����ʒu�E�G�t�F�N�g
            if (!bLoop)
            {
                //��
                if (flg.GetStartFrom() == RigidOnOff.StratFrom.Above /*&& this.gameObject.CompareTag("PockyStart")*/)
                {
                    FA_cpe = GetComponent<FACrePockyEff>();
                    if (FA_cpe != null)
                    {
                        FA_cpe.SetPos(collision.transform.position);
                    }
                }
                //��
                if (flg.GetStartFrom() == RigidOnOff.StratFrom.Below /*&& this.gameObject.CompareTag("PockyStart")*/)
                {
                    FB_cpe = GetComponent<FBCrePockyEff>();
                    if (FB_cpe != null)
                    {
                        FB_cpe.SetPos(collision.transform.position);
                    }
                }
                //��
                if (flg.GetStartFrom() == RigidOnOff.StratFrom.Left &&
                    this.gameObject.CompareTag("PockyStart_S"))
                {
                    FL_cpe = GetComponent<FLCrePockyEff>();
                    if (FL_cpe != null)
                    {
                        FL_cpe.SetPos(collision.transform.position);
                    }
                }
                //�E
                if (flg.GetStartFrom() == RigidOnOff.StratFrom.Right &&
                    this.gameObject.CompareTag("PockyStart_S"))
                {
                    FR_cpe = GetComponent<FRCrePockyEff>();
                    if (FR_cpe != null)
                    {
                        FR_cpe.SetPos(collision.transform.position);
                    }
                }
            }

            #endregion

            // �����o��
            audioSource.PlayOneShot(audioClip);
            // �I��
            if (gameObject.CompareTag("PockyEnd") || gameObject.CompareTag("PockyEnd_S"))
            {
                player.GetComponent<RigidOnOff>().PockyMoveFinish(this.transform.position); // �ړ��I��
                this.transform.parent.gameObject.SetActive(false);  // ��\��
                Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);         // �R���g���[���[�U����~
            }
        }

        if (!this.gameObject.activeSelf)    // �|�b�L�[����\���Ȃ�
        {
            // �Z�b��ɍĕ\��
            Invoke(nameof(RebirthPockys), 10.0f);

            //�ォ��
            if (flg.GetStartFrom() == RigidOnOff.StratFrom.Above /*&& this.gameObject.CompareTag("PockyStart")*/)
            {
                FA_cpe = GetComponent<FACrePockyEff>();
                if (FA_cpe != null)
                {
                    FA_cpe.CreatePocky(8.0f);
                }
            }
            //������
            if (flg.GetStartFrom() == RigidOnOff.StratFrom.Below /*&& this.gameObject.CompareTag("PockyStart")*/)
            {
                FB_cpe = GetComponent<FBCrePockyEff>();
                if (FB_cpe != null)
                {
                    FB_cpe.CreatePocky(8.0f);
                }
            }
            //�E����
            if (flg.GetStartFrom() == RigidOnOff.StratFrom.Right &&
                this.gameObject.CompareTag("PockyStart_S"))
            {
                FR_cpe = GetComponent<FRCrePockyEff>();
                if (FR_cpe != null)
                {
                    FR_cpe.CreatePocky(8.0f);
                }
            }
            //������
            if (flg.GetStartFrom() == RigidOnOff.StratFrom.Left &&
               this.gameObject.CompareTag("PockyStart_S"))
            {
                FL_cpe = GetComponent<FLCrePockyEff>();
                if (FL_cpe != null)
                {
                    FL_cpe.CreatePocky(8.0f);
                }
            }
        }
    }

    // =====================================================================================================
    // �|�b�L�[�̍ĕ\��
    // =====================================================================================================
    void RebirthPockys()
    {
        bool playerCollision = false;

        // �v���C���[��bone�̐ڐG��Ԃ��擾
        // --- �c
        if (this.transform.parent.transform.parent.Find("bone") != null)
        {
            playerCollision = this.transform.parent.transform.parent.Find("bone").GetComponent<PockyBoneDelete>().collision;
        }
        // --- ��
        else if (this.transform.parent.transform.parent.Find("boneS") != null)
        {
            playerCollision = this.transform.parent.transform.parent.Find("boneS").GetComponent<PockyBoneDelete>().collision;
        }

        // �G��Ă��Ȃ����
        if (!playerCollision)
        {
            if (this.transform.parent.transform.parent.Find("bone") != null)
            {
                //this.transform.parent.transform.parent.Find("bone").GetComponent<BoxCollider>().isTrigger = false;
                this.gameObject.SetActive(true);    // �\��
                flg.SetResetStratFrom(RigidOnOff.StratFrom.NoSignal);
                // �I�_�Ɣ���̕���
                if (this.CompareTag("PockyEnd"))
                {
                    this.transform.parent.gameObject.SetActive(true);
                    this.transform.parent.transform.parent.Find("bone").GetComponent<BoxCollider>().isTrigger = true;
                }
            }
            else if (this.transform.parent.transform.parent.Find("boneS") != null)
            {
                //this.transform.parent.transform.parent.Find("boneS").GetComponent<BoxCollider>().isTrigger = false;
                this.gameObject.SetActive(true);    // �\��
                flg.SetResetStratFrom(RigidOnOff.StratFrom.NoSignal);
                if (this.CompareTag("PockyEnd_S"))
                {
                    this.transform.parent.gameObject.SetActive(true);
                    this.transform.parent.transform.parent.Find("boneS").GetComponent<BoxCollider>().isTrigger = true;
                }
            }
        }
        else
        {
            Invoke(nameof(RebirthPockys), 0.05f);   // �f�B���C�������čĕ\��
        }
    }
}
