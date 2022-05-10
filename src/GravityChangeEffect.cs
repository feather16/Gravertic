using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChangeEffect : MonoBehaviour
{
    [SerializeField] private float MAX_RADIUS;
    [SerializeField] private float TIME;
    private int bornTick;

    // Start is called before the first frame update
    void Start()
    {
        bornTick = Game.tick;
    }

    // Update is called once per frame
    void Update()
    {
        // TIME•bŒo‚Á‚½‚çÁ‚¦‚é
        if((Game.tick - bornTick) * Time.deltaTime > TIME)
        {
            Destroy(gameObject);
        }

        float timeRate = (Game.tick - bornTick) * Time.deltaTime / TIME;
        float scale = (1 - timeRate) * 1 + timeRate * MAX_RADIUS;
        transform.localScale = new Vector3(scale, scale, 1);
        var renderer = GetComponent<SpriteRenderer>();
        var color = renderer.color;
        color.a = 1 - timeRate;
        renderer.color = color;
    }
}
