using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    private enum Drection
    {
        NONE = 0,   // ���o����
        DONUT,      // �h�[�i�c
        START,      // �X�^�[�g
        GOAL,       // �S�[��
        BOSS,       // �{�X�˔j
        STAGESELECT,// �X�e�[�W�Z���N�g
    }

    public GameObject target;                       // �J�����Ǐ]��
    public Vector3 distance = new Vector3(0.0f, 2.0f, -12.0f);  // �Ǐ]��Ƃ̋���
    private Drection directionNo;                   // �ǂ̉��o�ɂ��邩�̕ϐ�(0�͉��o����)
    public float timer = 0.0f;                      // �o�ߎ���
    public float goalTimer = 0.0f;                  // �S�[�����o����
    public float startDirectionTime = 3.0f;         // �X�^�[�g���o�p�^�C�}�[
    public float goalDirectionTimer = 0.1f;         // �S�[�����o�p�^�C�}�[
    public float donutDirectionTimer = 0.05f;       // �h�[�i�c���o�p�^�C�}�[
    private float stageSelectTimer = 0.0f;          // �Z���N�g��ʗp�^�C�}�[
    private GameObject player;                      // �v���C���[
    private PlayerStatus playerStatus;              // �v���C���[���
    private LifeGaugeCharacter life;                // �v���C���[�̎c�@���
    private float goalChild;                        // �S�[���A�C�e����
    private float oldChild;                         // 
    public float maxLeft = 0.0f;                    // �ړ�����(��)
    public float maxRight = 0.0f;                   //         (�E)
    public float maxDown = 0.0f;                    //         (��)
    private float count;                            // �S�[���A�C�e���̒i�K
    private float range;                            // �S�[�����o�͈̔�
    private float oldRange;                         // 
    private bool goalFlg = false;                   // �S�[������
    public bool sceneChangeFlg = false;             // �V�[���J�ڔ���
    public Vector3 targetStageObj = new Vector3();  // �X�e�[�W�I���I�u�W�F�N�g
    public bool targetStageFlg = false;             // �e�X�e�[�W����
    private Vector3 stageObjDistance = new Vector3();           // �X�e�[�W�I���I�u�W�F�N�g�Ƃ̋���

    public GameObject Boss1;
    // =====================================================================================================
    // Start is called before the first frame update
    // =====================================================================================================
    void Start()
    {
        directionNo = Drection.START;
        this.transform.position = target.transform.position;    // �J�����̏����ʒu�ݒ�

        player = GameObject.Find("Player"); // �v���C���[�擾
        playerStatus = player.GetComponent<PlayerStatus>(); // �v���C���[���擾
        life = player.GetComponent<LifeGaugeCharacter>();   // �̗͗ʎ擾
        if (GameObject.Find("makaron") != null)
        {
            // �S�[���I�u�W�F�N�g�i�K�̎擾
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
                // �ʏ�J����
                case Drection.NONE:
                    Boss1UpCamera();
                    transform.position = target.transform.position + distance;  // �Ǐ]

                    // �ړ�����
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

                    // �I�u�W�F�N�g���Ƃ̃J��������
                    if (playerStatus.donutFlg)
                    {
                        directionNo = Drection.DONUT;       // �h�[�i�c���[�v��
                    }
                    if (playerStatus.goalFlg)
                    {
                        directionNo = Drection.GOAL;        // �S�[�����o��
                    }
                    else if (playerStatus.bossFlg)
                    {
                        directionNo = Drection.BOSS;        // �{�X�펞
                    }
                    else if (targetStageFlg)
                    {                          
                        // �I�������X�e�[�W�I�u�W�F�N�g�Ɋ��
                        this.transform.position = new Vector3(targetStageObj.x, this.transform.position.y, this.transform.position.z);
                        stageObjDistance = this.transform.position - targetStageObj;
                        directionNo = Drection.STAGESELECT; // �X�e�[�W�I����
                    }
                    break;
                // �h�[�i�c���[�v��
                case Drection.DONUT:
                    Vector3 warpCamPos = new Vector3(0.0f, 0.0f, 0.0f); // �V�K�J������p��
                    warpCamPos.x = target.transform.position.x - this.transform.position.x;                 // �V�K���W���v�Z
                    warpCamPos.y = target.transform.position.y - (this.transform.position.y - 2.0f);

                    this.transform.position += warpCamPos * donutDirectionTimer * (Time.deltaTime * 400);   // �J�����ړ�
                    Invoke(nameof(DonutDirection), 1);  // �f�B���C��������
                    break;
                // �X�^�[�g���o��
                case Drection.START:
                    StartDirection();
                    break;
                // �S�[�����o��
                case Drection.GOAL:
                    GoalDirection();
                    break;
                // �{�X�펞
                case Drection.BOSS:
                    BossDirection();
                    if (!playerStatus.bossFlg)
                    {
                        directionNo = Drection.NONE;
                    }
                    break;
                // �X�e�[�W�I����
                case Drection.STAGESELECT:
                    StageSelectDirection();
                    break;
            }

            // �S�[����H�׏I����Ĉ�莞�Ԃ�������V�[���J��
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
    // �h�[�i�c���o
    // =====================================================================================================
    void DonutDirection()
    {
        // �v���C���[�ƃJ�����Ԃ̋������Z�o
        float dis = Vector3.Distance(this.transform.position, target.transform.position);
        // ���̋����ɂȂ�����J�������[�h��ύX
        if (dis <= 10.2f)
        {
            playerStatus.donutFlg = false;
            directionNo = Drection.NONE;
        }
    }

    // =====================================================================================================
    // �X�^�[�g���o
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

        // ���W�ɑ΂��Ă��񂾂�distance��K�p���Ă���
        pos.x = (target.transform.position.x) + distance.x * (timer / startDirectionTime) + overLeft * (timer / startDirectionTime);
        pos.y = (target.transform.position.y) + distance.y * (timer / startDirectionTime);
        pos.z = (target.transform.position.z) + distance.z * (timer / startDirectionTime);

        this.transform.position = pos;

        // ��莞�ԂɂȂ�����J�������[�h��ύX
        if (timer >= startDirectionTime)
        {
            directionNo = Drection.NONE;
            timer = 0.0f;
        }
    }

    // =====================================================================================================
    // �S�[�����o
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

            // �J�������񂹂�
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
            // �S�[�����o����ꏊ���甲������
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
                // �J����������
                pos.x *= (goalTimer / goalDirectionTimer);
                pos.y *= (goalTimer / goalDirectionTimer);
                pos.z *= (goalTimer / goalDirectionTimer);
                // �^�C�}�[���[���ɂ���
                goalTimer = 0.0f;
            }
            else
            {
                // �J����������
                pos.x *= (goalTimer / goalDirectionTimer);
                pos.y *= (goalTimer / goalDirectionTimer);
                pos.z *= (goalTimer / goalDirectionTimer);
            }

        }
        this.transform.position += pos;
    }

    //===============================
    //�{�X�p
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

        // X�͋߂Â��I�u�W�F�N�g�̈ʒu�ɌŒ�AY�̓I�u�W�F�N�g�̍����ɏ��X�ɋ߂Â���AZ�̓I�u�W�F�N�g�̈ʒu�ɋ߂Â���
        Vector3 pos = new Vector3(targetStageObj.x, (targetStageObj.y) + (stageObjDistance.y * ((stageSelectTimer) / 1.0f)), (stageObjDistance.z + targetStageObj.z) * stageSelectTimer / 1.0f);
        this.transform.position = pos;
    }
}