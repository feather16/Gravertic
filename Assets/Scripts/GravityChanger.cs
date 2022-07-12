using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : Entity
{
    [SerializeField] private AudioClip gravityChangeSound;

    public readonly static Vector3 ROT_OF_NO_GRAV = new Vector3(60, 0, 0);

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // êGÇÍÇƒÇ‡å¯â Ç™ñ≥Ç¢èÍçáÇÕêFÇîñÇ≠Ç∑ÇÈ
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        bool noEffect
            = Game.gravity == RotationToGravity(transform.eulerAngles);
        color.a = noEffect ? 0.55f : 1.0f;
        renderer.color = color;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        // èdóÕïœçX
        Vector2 prevGrav = new Vector2(Game.gravity.x, Game.gravity.y);
        if (collider.name == "Player")
        {
            Vector3 rot = transform.eulerAngles;
            Game.gravity = RotationToGravity(rot);
        }
        if(Game.gravity != prevGrav)
        {
            Game.audioSource.PlayOneShot(gravityChangeSound, 0.7f);
            string name = "GravityChangeEffect";
            GameObject prefab = Resources.Load<GameObject>(@"Prefabs/" + name);
            GameObject obj = Instantiate(
                prefab,
                transform.position,
                Quaternion.Euler(0, 0, 0)
            );
            obj.name = name;
            obj.transform.parent = transform;
        }
    }

    public static Vector2 RotationToGravity(Vector3 rotation)
    {
        Vector2 ret;
        if (rotation == ROT_OF_NO_GRAV) ret = Vector2.zero;
        else if (rotation.z == 0) ret = Vector2.up;
        else if (rotation.z == 90) ret = Vector2.left;
        else if (rotation.z == 180) ret = Vector2.down;
        else ret = Vector2.right;
        return ret;
    }

    public static Vector3 GravityToRotation(Vector2 gravity)
    {
        Vector3 ret;
        if (gravity == Vector2.zero) ret = ROT_OF_NO_GRAV;
        else if (gravity == Vector2.up) ret = new Vector3(0, 0, 0);
        else if (gravity == Vector2.left) ret = new Vector3(0, 0, 90);
        else if (gravity == Vector2.down) ret = new Vector3(0, 0, 180);
        else ret = new Vector3(0, 0, 270);
        return ret;
    }
}
