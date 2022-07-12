using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int MAX_HP = -1;
    [SerializeField] protected int hp = -1; // �f�o�b�O�̂��߂�SerializeField�ɂ��Ă�

    protected readonly float INVI_TIME = 1.0f;
    protected int lastDamagedTick;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        hp = MAX_HP;
        lastDamagedTick = -10000;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // ���G���Ԓ��̓_��
        int blinkingCount = 5;
        float phase = (Game.tick - lastDamagedTick) * Time.deltaTime / INVI_TIME;
        if(phase < 1)
        {
            bool disappeared
                = (int)(phase * blinkingCount * 2) % 2 == 0;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Color color = renderer.color;
            color.a = disappeared ? 0 : 1;
            renderer.color = color;
        }
    }

    public virtual void Damaged(int damage = 1)
    {
        if (
            hp != -1 && 
            (Game.tick - lastDamagedTick) * Time.deltaTime >= INVI_TIME)
        {
            hp = Mathf.Max(hp - damage, 0);
            if(hp > 0)
            {
                lastDamagedTick = Game.tick;
                /* �����Ń_���[�W���󂯂�����SE */
            }
            else
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        hp = 0;
        Destroy(gameObject);
    }
}
