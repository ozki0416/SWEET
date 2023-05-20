using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidOnOff : MonoBehaviour
{
    #region 変数宣言部

    private     Rigidbody       rb;                         // 自身のRigidbody情報

    private     Vector2         PlayerPos;                  // 現在位置
    private     GameObject      player;                     // 自身
    private     PlayerMove      playerMove;                 // プレイヤーの通常移動情報
    public      float           angle;                      // ポッキー移動中にプレイヤーに与える角度(縦用)
    public      float           angls;                      // ポッキー移動中にプレイヤーに与える角度(横用)

    private     GameObject      nearObj;                    // 最も近いオブジェクト(縦始点参照用)
    private     GameObject      nearObjS;                   // 最も近いオブジェクト(横始点参照用)

    public      bool            PockyStartFlg;              // ポッキー食べられますよ
    public      bool            UpDownCheck;                // そのポッキー移動は上下ですか？
    public      bool            RightCheck;                 // そのポッキー移動は左右ですか？
    private     GameObject[]    High, Low;                  // ポッキーの端(上端, 下端)
    private     GameObject[]    Right, Left;                // ポッキーの端(右端, 左端)
    
    public      bool            pockyBiteFlg = false;       // ポッキーをかじっているよ
    public      bool            PockyAnim;                  // プレイヤーのアニメーションフラグ

    private     StratFrom       ePos;                       // エフェクト用

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

        // 近いポッキーを初期取得
        nearObj = SearchTag(gameObject, "PockyCenter");
        nearObjS = SearchTag(gameObject, "PockyCenter_S");

        player = GameObject.Find("Player"); // プレイヤー取得
        playerMove = player.GetComponent<PlayerMove>(); // プレイヤー移動情報取得
            
        #region ポッキーの始点・終点タグの一括取得
        High = GameObject.FindGameObjectsWithTag("PockyEnd");
        Low = GameObject.FindGameObjectsWithTag("PockyStart");

        Right = GameObject.FindGameObjectsWithTag("PockyStart_S");
        Left = GameObject.FindGameObjectsWithTag("PockyEnd_S");
        #endregion

        rb = GetComponent<Rigidbody>(); // Rigidbodyの情報を取得
        ePos = StratFrom.NoSignal;
    }

    // =====================================================================================================
    // Update is called once per frame
    // =====================================================================================================
    void Update()
    {
         //最も近かったオブジェクトを取得し続ける
         nearObj = SearchTag(gameObject, "PockyCenter");
         nearObjS = SearchTag(gameObject, "PockyCenter_S");  // 横ポッキーの中心点

        // ポッキーとプレイヤーの位置に応じてポッキー始点を変更する
        if (nearObj != null) ChangePockyTag(); 
        if (nearObjS != null) ChangePockyTag();
    }

    // =====================================================================================================
    // ポッキーでの移動
    // =====================================================================================================
    void OnTriggerStay(Collider other)
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyの情報を取得
        PlayerPos = transform.position; // プレイヤーの位置を取得

        if (pockyBiteFlg && !PockyStartFlg)
        {
            PockyStartFlg = true;   // 移動解禁
            GetComponent<PlayerMove>().move = new Vector2(0.0f, 0.0f);  // 直前までの加速度を消去
        }

        // 始点に触れたら
        if (PockyStartFlg == true)
        {
            #region 上下移動
            // 上下移動
            if (other.CompareTag("PockyBone"))
            {
                // プレイヤーの移動量を無くす
                GetComponent<PlayerMove>().move = new Vector2(0.0f, 0.0f);
                // プレイヤーを回転させる用の変数
                angle = 0;
                // アニメーション変更
                PockyAnim = true;
                // 角度を求める
                if (High[0].tag == "PockyStart")
                {
                    // アニメーション変更
                    PockyAnim = true;
                    // エフェクト
                    ePos = StratFrom.Above;
                    // 下向きになるよう回転
                    angle = -90;
                    // 移動
                    this.transform.position = new Vector3(other.transform.position.x, this.transform.position.y - 0.2f, 0.0f);
                }
                if (Low[0].tag == "PockyStart")
                {
                    // アニメーション変更
                    PockyAnim = true;
                    // エフェクト
                    ePos = StratFrom.Below;
                    // 上向きになるよう回転
                    angle = 90;
                    // 重力無効
                    rb.useGravity = false;
                    // 移動
                    this.transform.position = new Vector3(other.transform.position.x, this.transform.position.y + 0.2f, 0.0f);
                }
                // 角度設定
                transform.eulerAngles = new Vector3(0, 0, angle);
            }
            #endregion

            #region 左右移動
            // 左右移動
            else if (other.CompareTag("PockyBone_S"))
            {
                // プレイヤーを回転させる用の変数
                float angls = 0.0f;
                // アニメーション変更
                PockyAnim = true;
                // 角度を求める
                if (Right[0].tag == "PockyStart_S")
                {
                    // アニメーション変更
                    PockyAnim = true;
                    // 角度戻す用
                    RightCheck = true;
                    // エフェクト
                    ePos = StratFrom.Right;
                    // 左向きになるよう回転
                    angls = -180;
                    // 移動
                    this.transform.position = new Vector3(this.transform.position.x - 0.2f, other.transform.position.y, 0.0f);
                }
                if (Left[0].tag == "PockyStart_S")
                {
                    // アニメーション変更
                    PockyAnim = true;
                    // エフェクト
                    ePos = StratFrom.Left;
                    // 右向きになるよう回転
                    angls = 0;
                    // 移動
                    this.transform.position = new Vector3(this.transform.position.x + 0.2f, other.transform.position.y, 0.0f);
                }
                // 角度設定
                transform.eulerAngles = new Vector3(0, angls, 0);
            }
            #endregion
        }
        else
        {  
            PockyAnim = false;  // アニメーション終了
        }
    }

    // =====================================================================================================
    // ポッキー移動の終了
    // =====================================================================================================
    //public void PockyMoveFinish()
    //{
    //    rb.useGravity = true;           // 重力有効
    //    PockyStartFlg = false;          // かじられません           // 移動終了
    //    playerMove.jumpFlg = false;
    //    playerMove.onGroundFlg = false;
    //    transform.eulerAngles = new Vector3(0, 0, 0);   // プレイヤーの角度を戻す
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
        // 重力有効
        rb.useGravity = true;
        // ポッキー移動の終了
        PockyStartFlg = false;
        // ジャンプ不可
        playerMove.jumpFlg = false;
        // 接地していない
        playerMove.onGroundFlg = false;
        // プレイヤーの角度を戻す
        if (!RightCheck) transform.eulerAngles = new Vector3(0, 0, 0);
        if (RightCheck)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            RightCheck = false;
        }
        // 位置変更
        this.transform.position = pos;
        // ポッキー終了時に跳ねる
        if (UpDownCheck)    
        {
            playerMove.move.y = 0.0f;
            playerMove.move.y = 60.0f;  // 上下移動時には上に跳ねる
            UpDownCheck = false;
        }
        else
        {
            playerMove.move.y = 30.0f;
            playerMove.move.x = 60.0f;  // 左右移動時には横に跳ねる
        }
        // ポッキー食べ終わりました
        pockyBiteFlg = false;
    }

    // =====================================================================================================
    // プレイヤーに近いオブジェクトのタグを参照
    // =====================================================================================================
    GameObject SearchTag(GameObject nowObj, string tagName)
    {
        float tmpDis = 0;               //距離用一時変数
        float nearDis = 0;              //最も近いオブジェクトの距離
        GameObject targetObj = null;    //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName))
        {
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;
            }

        }
        //最も近かったオブジェクトを返す
        return targetObj;
    }

    // =====================================================================================================
    // プレイヤーの位置に応じてポッキーの始点を変更
    // =====================================================================================================
    public void ChangePockyTag()
    {
        #region 縦ポッキーの始点変更
        // プレイヤーがポッキーの中心以下・下端のタグがStartじゃない・移動していない
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

            // プレイヤーがポッキーの中心以上・上端のタグがStartじゃない・移動していない
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

        #region 横ポッキーの始点変更
        // プレイヤーがポッキーの中心以上・右端のタグがStartじゃない・移動していない
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
            // プレイヤーがポッキーの中心以下・左端のタグがStartじゃない・移動していない
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
    // エフェクト取得
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