using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    private enum Drection
    {
        NONE = 0,   // 演出無し
        DONUT,      // ドーナツ
        START,      // スタート
        GOAL,       // ゴール
        BOSS,       // ボス突破
        STAGESELECT,// ステージセレクト
    }

    public GameObject target;                       // カメラ追従先
    public Vector3 distance = new Vector3(0.0f, 2.0f, -12.0f);  // 追従先との距離
    private Drection directionNo;                   // どの演出にするかの変数(0は演出無し)
    public float timer = 0.0f;                      // 経過時間
    public float goalTimer = 0.0f;                  // ゴール演出時間
    public float startDirectionTime = 3.0f;         // スタート演出用タイマー
    public float goalDirectionTimer = 0.1f;         // ゴール演出用タイマー
    public float donutDirectionTimer = 0.05f;       // ドーナツ演出用タイマー
    private float stageSelectTimer = 0.0f;          // セレクト画面用タイマー
    private GameObject player;                      // プレイヤー
    private PlayerStatus playerStatus;              // プレイヤー情報
    private LifeGaugeCharacter life;                // プレイヤーの残機情報
    private float goalChild;                        // ゴールアイテムの
    private float oldChild;                         // 
    public float maxLeft = 0.0f;                    // 移動制限(左)
    public float maxRight = 0.0f;                   //         (右)
    public float maxDown = 0.0f;                    //         (下)
    private float count;                            // ゴールアイテムの段階
    private float range;                            // ゴール演出の範囲
    private float oldRange;                         // 
    private bool goalFlg = false;                   // ゴール判定
    public bool sceneChangeFlg = false;             // シーン遷移判定
    public Vector3 targetStageObj = new Vector3();  // ステージ選択オブジェクト
    public bool targetStageFlg = false;             // 各ステージ判定
    private Vector3 stageObjDistance = new Vector3();           // ステージ選択オブジェクトとの距離

    public GameObject Boss1;
    // =====================================================================================================
    // Start is called before the first frame update
    // =====================================================================================================
    void Start()
    {
        directionNo = Drection.START;
        this.transform.position = target.transform.position;    // カメラの初期位置設定

        player = GameObject.Find("Player"); // プレイヤー取得
        playerStatus = player.GetComponent<PlayerStatus>(); // プレイヤー情報取得
        life = player.GetComponent<LifeGaugeCharacter>();   // 体力量取得
        if (GameObject.Find("makaron") != null)
        {
            // ゴールオブジェクト段階の取得
            count = GameObject.Find("makaron").transform.childCount;
            goalChild = GameObject.Find("makaron").transform.childCount;
        }
        else
        {
            count = 100;
            goalChild = 100;
        }
        oldChild = goalChild;
        range = count / goalChild;
        oldRange = range;
    }

    // =====================================================================================================
    // Update is called once per frame
    // =====================================================================================================
    void Update()
    {
        oldChild = count;
        if (GameObject.Find("makaron"))
        {
            count = GameObject.Find("makaron").transform.childCount;
        }

        if (!life.deadFlg)
        {
            switch (directionNo)
            {
                // 通常カメラ
                case Drection.NONE:
                    Boss1UpCamera();
                    transform.position = target.transform.position + distance;  // 追従

                    // 移動制限
                    if (this.transform.position.x < maxLeft)
                    {
                        this.transform.position = new Vector3(maxLeft, this.transform.position.y, this.transform.position.z);
                    }
                    if (this.transform.position.x > maxRight)
                    {
                        this.transform.position = new Vector3(maxRight, this.transform.position.y, this.transform.position.z);
                    }
                    if (this.transform.position.y < maxDown)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, maxDown, this.transform.position.z);
                    }

                    // オブジェクトごとのカメラ操作
                    if (playerStatus.donutFlg)
                    {
                        directionNo = Drection.DONUT;       // ドーナツワープ時
                    }
                    if (playerStatus.goalFlg)
                    {
                        directionNo = Drection.GOAL;        // ゴール演出時
                    }
                    else if (playerStatus.bossFlg)
                    {
                        directionNo = Drection.BOSS;        // ボス戦時
                    }
                    else if (targetStageFlg)
                    {                          
                        // 選択したステージオブジェクトに寄る
                        this.transform.position = new Vector3(targetStageObj.x, this.transform.position.y, this.transform.position.z);
                        stageObjDistance = this.transform.position - targetStageObj;
                        directionNo = Drection.STAGESELECT; // ステージ選択時
                    }
                    break;
                // ドーナツワープ時
                case Drection.DONUT:
                    Vector3 warpCamPos = new Vector3(0.0f, 0.0f, 0.0f); // 新規カメラを用意
                    warpCamPos.x = target.transform.position.x - this.transform.position.x;                 // 新規座標を計算
                    warpCamPos.y = target.transform.position.y - (this.transform.position.y - 2.0f);

                    this.transform.position += warpCamPos * donutDirectionTimer * (Time.deltaTime * 400);   // カメラ移動
                    Invoke(nameof(DonutDirection), 1);  // ディレイをかける
                    break;
                // スタート演出時
                case Drection.START:
                    StartDirection();
                    break;
                // ゴール演出時
                case Drection.GOAL:
                    GoalDirection();
                    break;
                // ボス戦時
                case Drection.BOSS:
                    BossDirection();
                    if (!playerStatus.bossFlg)
                    {
                        directionNo = Drection.NONE;
                    }
                    break;
                // ステージ選択時
                case Drection.STAGESELECT:
                    StageSelectDirection();
                    break;
            }

            // ゴールを食べ終わって一定時間たったらシーン遷移
            if (count <= 1.0f)
            {
                if (timer < 0.01f)
                {
                    StartCoroutine(PlayerStatus.Vibration(1.0f, 1.0f, 1.0f));
                    Time.timeScale = 0.5f;
                }
                timer += Time.deltaTime;
                if (timer > 1.3f)
                {
                    Time.timeScale = 1.0f;
                    sceneChangeFlg = true;
                }
            }
        }
    }

    // =====================================================================================================
    // ドーナツ演出
    // =====================================================================================================
    void DonutDirection()
    {
        // プレイヤーとカメラ間の距離を算出
        float dis = Vector3.Distance(this.transform.position, target.transform.position);
        // 一定の距離になったらカメラモードを変更
        if (dis <= 10.2f)
        {
            playerStatus.donutFlg = false;
            directionNo = Drection.NONE;
        }
    }

    // =====================================================================================================
    // スタート演出
    // =====================================================================================================
    private void StartDirection()
    {
        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
        float overLeft = 0.0f;

        if (target.transform.position.x + distance.x < maxLeft)
        {
            overLeft = maxLeft - (target.transform.position.x + distance.x);
        }

        timer += Time.deltaTime;

        // 座標に対してだんだんdistanceを適用していく
        pos.x = (target.transform.position.x) + distance.x * (timer / startDirectionTime) + overLeft * (timer / startDirectionTime);
        pos.y = (target.transform.position.y) + distance.y * (timer / startDirectionTime);
        pos.z = (target.transform.position.z) + distance.z * (timer / startDirectionTime);

        this.transform.position = pos;

        // 一定時間になったらカメラモードを変更
        if (timer >= startDirectionTime)
        {
            directionNo = Drection.NONE;
            timer = 0.0f;
        }
    }

    // =====================================================================================================
    // ゴール演出
    // =====================================================================================================
    private void GoalDirection()
    {
        Vector3 pos = new Vector3();
        goalTimer += Time.deltaTime;

        if (playerStatus.goalFlg)
        {
            goalFlg = true;
            if (oldChild != count)
            {
                goalTimer = 0.0f;
                oldRange = range;
            }

            if (goalTimer > goalDirectionTimer)
            {
                goalTimer = goalDirectionTimer;
            }

            pos = target.transform.position - this.transform.position;
            range = count / goalChild;
            if (range < 0.3f)
            {
                range = 0.3f;
            }
            oldRange -= (oldRange - range) * (goalTimer / goalDirectionTimer);
            if (oldRange < range)
            {
                oldRange = range;
            }

            // カメラを寄せる
            if (count < goalChild)
            {
                pos.x += (distance.x * oldRange);
                pos.y += ((distance.y * oldRange) - distance.y / 2);
                pos.z += (distance.z * oldRange);
            }
            else
            {
                pos.x += (distance.x * range);
                pos.y += (distance.y * range);
                pos.z += (distance.z * range);
            }
        }
        else
        {
            // ゴール演出する場所から抜けた時
            if (goalFlg)
            {
                goalFlg = false;
                goalTimer = 0.0f;
                oldRange = goalChild / goalChild;
            }
            pos = (target.transform.position + distance) - this.transform.position;

            if (goalTimer > goalDirectionTimer)
            {
                directionNo = Drection.NONE;
                goalTimer = goalDirectionTimer;
                // カメラを引く
                pos.x *= (goalTimer / goalDirectionTimer);
                pos.y *= (goalTimer / goalDirectionTimer);
                pos.z *= (goalTimer / goalDirectionTimer);
                // タイマーをゼロにする
                goalTimer = 0.0f;
            }
            else
            {
                // カメラを引く
                pos.x *= (goalTimer / goalDirectionTimer);
                pos.y *= (goalTimer / goalDirectionTimer);
                pos.z *= (goalTimer / goalDirectionTimer);
            }

        }
        this.transform.position += pos;
    }

    //===============================
    //ボス用
    //=================================
    private void BossDirection()
    {

        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
        float range = 0.4f;

        pos.x = target.transform.position.x;
        pos.y = (target.transform.position.y) + distance.y * range;
        pos.z = (target.transform.position.z) + distance.z * range;
        transform.position = pos;
        if (timer > 0.1f)
        {
            StartCoroutine(PlayerStatus.Vibration(1.0f, 1.0f, 1.0f));
        }
        timer += Time.deltaTime;
        if (timer > 1.2f)
        {
            Time.timeScale = 1.0f;
        }
    }
    private void Boss1UpCamera()
    {
        if(Boss1 != null)
        {
            if(target.transform.position.y > 9)
            {
                if (distance.y > -5)
                    distance.y -= 3.0f * Time.deltaTime;
                if (distance.y < -5)
                    distance.y = -5;
            }
            else if(distance.y < 3)
            {
                distance.y += 2.5f * Time.deltaTime; ;
                if (distance.y > 3)
                    distance.y = 3;
            }
        }
    }

    private void StageSelectDirection()
    {
        stageSelectTimer -= Time.deltaTime;
        if (stageSelectTimer < 0.0f)
        {
            directionNo = Drection.NONE;
            stageSelectTimer = 1.0f;
            targetStageFlg = false;
            return;
        }

        float stageDistance = stageSelectTimer / 1.0f;
        if (stageDistance <= 0.3f)
        {
            stageDistance = 0.3f;
            return;
        }

        // Xは近づくオブジェクトの位置に固定、Yはオブジェクトの高さに徐々に近づける、Zはオブジェクトの位置に近づける
        Vector3 pos = new Vector3(targetStageObj.x, (targetStageObj.y) + (stageObjDistance.y * ((stageSelectTimer) / 1.0f)), (stageObjDistance.z + targetStageObj.z) * stageSelectTimer / 1.0f);
        this.transform.position = pos;
    }
}