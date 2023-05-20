using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DonutWarp : MonoBehaviour
{
    public GameObject obj;      // プレイヤー
    public DonutWarp transObj;  // ワープ先のオブジェクト
    private Vector3 transVec;   // ワープ先の座標
    public bool moveStatus;     // 移動状態
    private bool test;

    private PlayerStatus donutStatus;   // ドーナツﾌﾗｸﾞ用
    private WarpEff wf;

    // =====================================================================================================
    // Start is called before the first frame update
    // =====================================================================================================
    void Start()
    {
        transVec = transObj.transform.position;         // ワープ先の座標を取得する
        moveStatus = true;      // ワープ可能

        donutStatus = obj.GetComponent<PlayerStatus>(); // カメラ演出切替用
        wf = GetComponent<WarpEff>();
    }

    // =====================================================================================================
    // Update is called once per frame
    // =====================================================================================================
    void Update()
    {

    }

    // =====================================================================================================
    // かじってワープ
    // =====================================================================================================
    void OnTriggerStay(Collider other)
    {
        #region キーボード
        // ワープ可能状態のみワープ処理
        if (moveStatus && transObj.gameObject.activeSelf)
        {
            if (Keyboard.current.zKey.isPressed)
            {
                // カメラ状態を変更
                donutStatus.donutFlg = true;
                // エフェクト
                wf.SetInstantiateMode(false);
                // ワープ処理
                Invoke(nameof(warp), 1);
                // ドーナツを非表示
                this.gameObject.SetActive(false);
                // 非表示なら再表示
                if (!this.gameObject.activeSelf)
                {
                    wf.SetInstantiateMode(true);
                    Invoke(nameof(DonutRebirth), 5);
                }
            }
        }
        #endregion

        #region コントローラー
        if (moveStatus && transObj.gameObject.activeSelf)
        {
            if (Gamepad.current != null)
            {
                if (Gamepad.current.rightTrigger.isPressed)
                {
                    // カメラ状態を変更
                    donutStatus.donutFlg = true;
                    // エフェクト
                    wf.SetInstantiateMode(false);
                    // ワープ処理
                    Invoke(nameof(warp), 1);
                    // ドーナツを非表示
                    this.gameObject.SetActive(false);
                    // 非表示なら再表示
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
    // 一度離れてから再ワープ
    // =====================================================================================================
    void OnTriggerExit(Collider other)
    {
        moveStatus = true;  // ワープ可能
    }

    // =====================================================================================================
    // 本チャンワープ処理
    // =====================================================================================================
    void warp()
    {
        transObj.moveStatus = false;            // すぐにはワープ不可
        obj.transform.position = transVec;      // 目的の座標まで移動
    }

    // =====================================================================================================
    // ドーナツの再表示
    // =====================================================================================================
    void DonutRebirth()
    {
        this.gameObject.SetActive(true);
    }
}
