using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    //　ポーズした時に表示するUIのプレハブ
    [SerializeField]
    private GameObject pauseUIPrefab;
    [SerializeField]
    private GameObject pauseUIPrefab_Select;
    [SerializeField]
    private GameObject pauseUIPrefab_Area;

    //　ポーズUIのインスタンス
    private GameObject pauseUIInstance;

    private void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        // キーボード
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (pauseUIInstance == null)
            {
                // エリアセレクトで表示するポーズ画面
                if(SceneManager.GetActiveScene().name == "SelectArea")
                {
                    pauseUIInstance = GameObject.Instantiate(pauseUIPrefab_Area) as GameObject; // UI表示
                }
                // ステージセレクトシーンで表示するポーズ画面
                if (SceneManager.GetActiveScene().name == "SelectStage1" ||
                        SceneManager.GetActiveScene().name == "SelectStage2" ||
                        SceneManager.GetActiveScene().name == "SelectStage3")
                {
                    pauseUIInstance = GameObject.Instantiate(pauseUIPrefab_Select) as GameObject;
                }
                // ゲーム画面で表示するポーズ画面
                if (SceneManager.GetActiveScene().name != "SelectStage1" &&
                        SceneManager.GetActiveScene().name != "SelectStage2" &&
                        SceneManager.GetActiveScene().name != "SelectStage3" &&
                        SceneManager.GetActiveScene().name != "SelectArea")
                {
                    pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                }
                Time.timeScale = 0f;
            }
            else
            {
                Destroy(pauseUIInstance);
                Time.timeScale = 1f;
            }
        }

        // コントローラー
        if (Gamepad.current != null)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                if (pauseUIInstance == null)
                {
                    // エリアセレクトで表示するポーズ画面
                    if (SceneManager.GetActiveScene().name == "SelectArea")
                    {
                        pauseUIInstance = GameObject.Instantiate(pauseUIPrefab_Area) as GameObject;
                    }
                    // ステージセレクトシーンで表示するポーズ画面
                    if (SceneManager.GetActiveScene().name == "SelectStage1" ||
                            SceneManager.GetActiveScene().name == "SelectStage2" ||
                            SceneManager.GetActiveScene().name == "SelectStage3")
                    {
                        pauseUIInstance = GameObject.Instantiate(pauseUIPrefab_Select) as GameObject;
                    }
                    // ゲーム画面で表示するポーズ画面
                    if (SceneManager.GetActiveScene().name != "SelectStage1" &&
                            SceneManager.GetActiveScene().name != "SelectStage2" &&
                            SceneManager.GetActiveScene().name != "SelectStage3" &&
                            SceneManager.GetActiveScene().name != "SelectArea")
                    {
                        pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
                    }
                    Time.timeScale = 0f;
                }
                else
                {
                    Destroy(pauseUIInstance);
                    Time.timeScale = 1f;
                }
            }
        }
    }
}

