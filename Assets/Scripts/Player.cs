using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : Entity
{
    [SerializeField] private float MAX_SPEED;
    [SerializeField] private float FIRE_COOL_TIME;
    [SerializeField] private AudioClip bulletSound;

    private int lastFiredTime = -1;

    private Button upButton, downButton, leftButton, rightButton, returnButton;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        GameObject statusPanel = GameObject.Find("StatusPanel");
        
        foreach(Transform transform in statusPanel.transform)
        {
            Button button;
            if(transform.TryGetComponent(out button))
            {
                if (button.name == "UpButton") upButton = button;
                else if (button.name == "DownButton") downButton = button;
                else if (button.name == "LeftButton") leftButton = button;
                else if (button.name == "RightButton") rightButton = button;
                else if (button.name == "ReturnButton") returnButton = button;
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // プレイヤーの速度
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (upButton.isPressed) v += 1;
        if (downButton.isPressed) v -= 1;
        if (leftButton.isPressed) h -= 1;
        if (rightButton.isPressed) h += 1;
        h = Mathf.Clamp(h, -1, 1);
        v = Mathf.Clamp(v, -1, 1);
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = MAX_SPEED * new Vector2(h, v);

        // プレイヤーの向き
        List<float> velArr = new List<float> { v, -h, -v, h };
        float maxVel = velArr.Max();
        if (maxVel > 0)
        {
            float zAngle = velArr.IndexOf(maxVel) * 90;
            Vector3 ang = transform.eulerAngles;
            ang.z = zAngle;
            transform.eulerAngles = ang;
        }

        // Enterで弾発射
        bool returnPressed =
            Input.GetKey(KeyCode.Return) || returnButton.isPressed ||
            Input.GetKey(KeyCode.Space);
        if (returnPressed && 
            (Game.tick - lastFiredTime) * Time.deltaTime >= FIRE_COOL_TIME)
        {
            Game.audioSource.PlayOneShot(bulletSound, 0.25f);
            string name = "Bullet";
            Vector2 bulletDir;
            if (transform.eulerAngles.z == 0) bulletDir = new Vector2(0, 1);
            else if (transform.eulerAngles.z == 90) bulletDir = new Vector2(-1, 0);
            else if (transform.eulerAngles.z == 180) bulletDir = new Vector2(0, -1);
            else bulletDir = new Vector2(1, 0);
            GameObject prefab = Resources.Load<GameObject>(@"Prefabs/" + name);
            GameObject obj = Instantiate(
                prefab, 
                transform.position + 0.8f * new Vector3(bulletDir.x, bulletDir.y, 0), 
                Quaternion.Euler(0, 0, transform.eulerAngles.z + 90)
            );
            obj.name = name;
            obj.GetComponent<Rigidbody2D>().velocity = bulletDir;
            lastFiredTime = Game.tick;
        }

        // プレイヤーのHPバー
        GameObject playerHPBar = GameObject.Find("PlayerHPBar");
        Slider hpSlider = playerHPBar.GetComponent<Slider>();
        hpSlider.value = (float)hp / MAX_HP;

        // プレイヤーのHPテキスト
        GameObject playerHPText = GameObject.Find("PlayerHPText");
        Text hpText = playerHPText.GetComponent<Text>();
        hpText.text = $"{hp} / {MAX_HP}";

        // 背景移動
        float xDiffMax = 235;
        float yDiffMax = 240;
        float maxBackMoveX = 2 * xDiffMax / (Game.stageMaxX - Game.stageMinX - 2);
        float maxBackMoveY = 2 * yDiffMax / (Game.stageMaxY - Game.stageMinY - 2);
        float maxBackMove = Mathf.Min(maxBackMoveX, maxBackMoveY, 4);
        float centerX = (Game.stageMinX + Game.stageMaxX) / 2.0f;
        float centerY = (Game.stageMinY + Game.stageMaxY) / 2.0f;
        GameObject background = GameObject.Find("Background");
        background.transform.localPosition = new Vector3(
            maxBackMove * (centerX - transform.position.x),
            maxBackMove * (centerY - transform.position.y),
            0
        );
    }
}
