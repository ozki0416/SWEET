using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PockyBoneDelete : MonoBehaviour
{
    GameObject player;      // プレイヤー情報
    RigidOnOff ronoff;      // ポッキー上昇スクリプトの取得

    public bool UDcheck;    // 上昇中フラグを入れる器
    public bool collision = false;  // プレイヤーと当たっているかどうか

    // =====================================================================================================
    // Start is called before the first frame update
    // =====================================================================================================
    void Start()
    {
        player = GameObject.Find("Player");             // プレイヤー取得
        ronoff = player.GetComponent<RigidOnOff>();     // プレイヤー情報取得
    }

    // =====================================================================================================
    // Update is called once per frame
    // =====================================================================================================
    void Update()
    {

    }

    // =====================================================================================================
    // 接触判定
    // =====================================================================================================
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collision = true;   // 接触
        }
    }

    // =====================================================================================================
    // ポッキーから離れた際にBoneを消す
    // =====================================================================================================
    private void OnTriggerExit(Collider other)
    {
        // 上昇中フラグの取得
        UDcheck = ronoff.UpDownCheck;

        if (other.gameObject.CompareTag("Player"))
        {
            collision = false;
        }

        if (other.gameObject.CompareTag("Player") && UDcheck)
        {
            // 非表示
            this.gameObject.SetActive(false);
        }
        // Boneが非表示なら
        if (!this.gameObject.activeSelf)
        {
            // 再表示
            Invoke(nameof(RebirthBone), 9.0f);
        }
    }

    // =====================================================================================================
    // Boneの再表示
    // =====================================================================================================
    void RebirthBone()
    {
        this.gameObject.SetActive(true);    // 表示
    }
}
