using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // 重力
    [SerializeField] private float GRAVITY_SCALE;
    public static Vector2 gravity = new Vector2(0, 0);

    // ステージ
    public static int stageMinX, stageMaxX, stageMinY, stageMaxY;

    public static int difficulty = 1;

    public static AudioSource audioSource;

    public static int tick { get; private set; } = 0;

    //public static float FPS;

    // Start is called before the first frame update
    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        SetWalls();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        Physics2D.gravity = GRAVITY_SCALE * gravity;
        tick++;

        // 重力の矢印
        GameObject gravArrow = GameObject.Find("CurrentGravity");
        gravArrow.transform.eulerAngles 
            = GravityChanger.GravityToRotation(gravity);

        // プレイヤーをカメラで追う
        GameObject player = GameObject.Find("Player");
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            transform.position.z
        );
    }

    private void SetWalls()
    {
        // 壁を配置
        Entity[] entities = FindObjectsOfType<Entity>();
        float minX = entities[0].transform.position.x;
        float maxX = entities[0].transform.position.x;
        float minY = entities[0].transform.position.y;
        float maxY = entities[0].transform.position.y;
        foreach (Entity entity in entities)
        {
            Vector2 pos = entity.transform.position;
            Vector2 scl = entity.transform.localScale;
            minX = Mathf.Min(pos.x - (scl.x - 1) / 2 - 1, minX);
            maxX = Mathf.Max(pos.x + (scl.x - 1) / 2 + 1, maxX);
            minY = Mathf.Min(pos.y - (scl.y - 1) / 2 - 1, minY);
            maxY = Mathf.Max(pos.y + (scl.y - 1) / 2 + 1, maxY);
        }
        minX = Mathf.Floor(minX);
        maxX = Mathf.Ceil(maxX);
        minY = Mathf.Floor(minY);
        maxY = Mathf.Ceil(maxY);
        for (int x = (int)minX; x <= (int)maxX; x++)
        {
            for (int y = (int)minY; y <= (int)maxY; y++)
            {
                if(
                    x == (int)minX || x == (int)maxX || 
                    y == (int)minY || y == (int)maxY)
                {
                    string name = "Wall";
                    GameObject prefab = Resources.Load<GameObject>(@"Prefabs/" + name);
                    GameObject obj = Instantiate(
                        prefab, 
                        new Vector2(x, y), 
                        Quaternion.Euler(0, 0, 0)
                    );
                    obj.name = name;
                }
            }
        }
        stageMinX = (int)minX;
        stageMaxX = (int)maxX;
        stageMinY = (int)minY;
        stageMaxY = (int)maxY;
    }
}
