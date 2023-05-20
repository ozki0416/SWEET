using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    //�@�|�[�Y�������ɕ\������UI�̃v���n�u
    [SerializeField]
    private GameObject pauseUIPrefab;
    [SerializeField]
    private GameObject pauseUIPrefab_Select;
    [SerializeField]
    private GameObject pauseUIPrefab_Area;

    //�@�|�[�YUI�̃C���X�^���X
    private GameObject pauseUIInstance;

    private void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        // �L�[�{�[�h
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (pauseUIInstance == null)
            {
                // �G���A�Z���N�g�ŕ\������|�[�Y���
                if(SceneManager.GetActiveScene().name == "SelectArea")
                {
                    pauseUIInstance = GameObject.Instantiate(pauseUIPrefab_Area) as GameObject; // UI�\��
                }
                // �X�e�[�W�Z���N�g�V�[���ŕ\������|�[�Y���
                if (SceneManager.GetActiveScene().name == "SelectStage1" ||
                        SceneManager.GetActiveScene().name == "SelectStage2" ||
                        SceneManager.GetActiveScene().name == "SelectStage3")
                {
                    pauseUIInstance = GameObject.Instantiate(pauseUIPrefab_Select) as GameObject;
                }
                // �Q�[����ʂŕ\������|�[�Y���
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

        // �R���g���[���[
        if (Gamepad.current != null)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                if (pauseUIInstance == null)
                {
                    // �G���A�Z���N�g�ŕ\������|�[�Y���
                    if (SceneManager.GetActiveScene().name == "SelectArea")
                    {
                        pauseUIInstance = GameObject.Instantiate(pauseUIPrefab_Area) as GameObject;
                    }
                    // �X�e�[�W�Z���N�g�V�[���ŕ\������|�[�Y���
                    if (SceneManager.GetActiveScene().name == "SelectStage1" ||
                            SceneManager.GetActiveScene().name == "SelectStage2" ||
                            SceneManager.GetActiveScene().name == "SelectStage3")
                    {
                        pauseUIInstance = GameObject.Instantiate(pauseUIPrefab_Select) as GameObject;
                    }
                    // �Q�[����ʂŕ\������|�[�Y���
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

