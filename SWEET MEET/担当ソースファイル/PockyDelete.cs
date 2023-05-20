using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PockyDelete : MonoBehaviour
{
    private FoodWaste fw;           // パーティクル情報
    public GameObject player;       // プレイヤー情報
    private bool EatFlg;            // かじる状態取得用
    RigidOnOff flg;                 // 変数(フラグ)取得用
    public AudioClip audioClip;     // 音声再生用

    // エフェクト
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
    // プレイヤーに触れたらポッキーを1つずつ削除
    // =====================================================================================================
    void OnCollisionStay(Collision collision)
    {
        EatFlg = flg.PockyStartFlg;

        if (collision.gameObject.name == "Player" && EatFlg)
        {
            // プレイヤーに触れたらの条件式の中なのでcollision使ってる
            AudioSource audioSource = collision.gameObject.GetComponent<AudioSource>();
            // オブジェクト非表示
            this.gameObject.SetActive(false);
            // パーティクル発生
            fw = GetComponent<FoodWaste>();
            fw.CreateParticle(collision, (int)flg.GetStartFrom(), this.tag);
            if(Gamepad.current != null)
            {
                // コントローラー振動
                Gamepad.current.SetMotorSpeeds(0.3f, 0.0f);
                Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
            }

            #region SetPos
            // ポッキー復活位置・エフェクト
            if (!bLoop)
            {
                //上
                if (flg.GetStartFrom() == RigidOnOff.StratFrom.Above /*&& this.gameObject.CompareTag("PockyStart")*/)
                {
                    FA_cpe = GetComponent<FACrePockyEff>();
                    if (FA_cpe != null)
                    {
                        FA_cpe.SetPos(collision.transform.position);
                    }
                }
                //下
                if (flg.GetStartFrom() == RigidOnOff.StratFrom.Below /*&& this.gameObject.CompareTag("PockyStart")*/)
                {
                    FB_cpe = GetComponent<FBCrePockyEff>();
                    if (FB_cpe != null)
                    {
                        FB_cpe.SetPos(collision.transform.position);
                    }
                }
                //左
                if (flg.GetStartFrom() == RigidOnOff.StratFrom.Left &&
                    this.gameObject.CompareTag("PockyStart_S"))
                {
                    FL_cpe = GetComponent<FLCrePockyEff>();
                    if (FL_cpe != null)
                    {
                        FL_cpe.SetPos(collision.transform.position);
                    }
                }
                //右
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

            // 音を出す
            audioSource.PlayOneShot(audioClip);
            // 終了
            if (gameObject.CompareTag("PockyEnd") || gameObject.CompareTag("PockyEnd_S"))
            {
                player.GetComponent<RigidOnOff>().PockyMoveFinish(this.transform.position); // 移動終了
                this.transform.parent.gameObject.SetActive(false);  // 非表示
                Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);         // コントローラー振動停止
            }
        }

        if (!this.gameObject.activeSelf)    // ポッキーが非表示なら
        {
            // 〇秒後に再表示
            Invoke(nameof(RebirthPockys), 10.0f);

            //上から
            if (flg.GetStartFrom() == RigidOnOff.StratFrom.Above /*&& this.gameObject.CompareTag("PockyStart")*/)
            {
                FA_cpe = GetComponent<FACrePockyEff>();
                if (FA_cpe != null)
                {
                    FA_cpe.CreatePocky(8.0f);
                }
            }
            //下から
            if (flg.GetStartFrom() == RigidOnOff.StratFrom.Below /*&& this.gameObject.CompareTag("PockyStart")*/)
            {
                FB_cpe = GetComponent<FBCrePockyEff>();
                if (FB_cpe != null)
                {
                    FB_cpe.CreatePocky(8.0f);
                }
            }
            //右から
            if (flg.GetStartFrom() == RigidOnOff.StratFrom.Right &&
                this.gameObject.CompareTag("PockyStart_S"))
            {
                FR_cpe = GetComponent<FRCrePockyEff>();
                if (FR_cpe != null)
                {
                    FR_cpe.CreatePocky(8.0f);
                }
            }
            //左から
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
    // ポッキーの再表示
    // =====================================================================================================
    void RebirthPockys()
    {
        bool playerCollision = false;

        // プレイヤーとboneの接触状態を取得
        // --- 縦
        if (this.transform.parent.transform.parent.Find("bone") != null)
        {
            playerCollision = this.transform.parent.transform.parent.Find("bone").GetComponent<PockyBoneDelete>().collision;
        }
        // --- 横
        else if (this.transform.parent.transform.parent.Find("boneS") != null)
        {
            playerCollision = this.transform.parent.transform.parent.Find("boneS").GetComponent<PockyBoneDelete>().collision;
        }

        // 触れていない状態
        if (!playerCollision)
        {
            if (this.transform.parent.transform.parent.Find("bone") != null)
            {
                //this.transform.parent.transform.parent.Find("bone").GetComponent<BoxCollider>().isTrigger = false;
                this.gameObject.SetActive(true);    // 表示
                flg.SetResetStratFrom(RigidOnOff.StratFrom.NoSignal);
                // 終点と判定の復活
                if (this.CompareTag("PockyEnd"))
                {
                    this.transform.parent.gameObject.SetActive(true);
                    this.transform.parent.transform.parent.Find("bone").GetComponent<BoxCollider>().isTrigger = true;
                }
            }
            else if (this.transform.parent.transform.parent.Find("boneS") != null)
            {
                //this.transform.parent.transform.parent.Find("boneS").GetComponent<BoxCollider>().isTrigger = false;
                this.gameObject.SetActive(true);    // 表示
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
            Invoke(nameof(RebirthPockys), 0.05f);   // ディレイをかけて再表示
        }
    }
}
