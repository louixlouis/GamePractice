using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    // Member variables.
    public enum State
    {
        None = -1,
        Ready,
        Appear,
        Battle,
        Dead,
        Disappear,
    }

    [SerializeField]
    State CurrentState = State.None;
    const float MaxSpeed = 10.0f;
    const float MaxSpeedTime = 0.5f;

    [SerializeField]
    Vector3 TargetPosition;

    [SerializeField]
    float CurrentSpeed;

    Vector3 CurrentVelocity;

    float MoveStartTime = 0.0f;
    //float BattleStartTime = 0.0f;
    float LastBattleUpdateTime = 0.0f;

    [SerializeField]
    int FireRemainCount = 5;

    [SerializeField]
    Transform FireTransform;

    [SerializeField]
    GameObject Bullet;

    [SerializeField]
    float BulletSpeed = 10;

    [SerializeField]
    int GamePoint = 10;

    public string FilePath
    {
        get;
        set;
    }
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    protected override void UpdateActor()
    {
        // GetKeyDown method
        // Appear
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Appear(new Vector3(7.0f, transform.position.y, transform.position.z));
        //}
        
        switch (CurrentState)
        {
            case State.None:
            case State.Ready:
                break;
            case State.Dead:
                break;
            case State.Appear:
            case State.Disappear:
                UpdateSpeed();
                UpdateMove();
                break;
            case State.Battle:
                UpdateBattle();
                break;
            default:
                Debug.LogError("Undefined state");
                break;
        }
    }

    void UpdateSpeed()
    {
        // Math function module.
        // Built-in
        // Lerp = Linear interpolation.
        // Math.Lerp(���۰�, ����, �˰���� ����(0 ~ 1 ������ ��)) 
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time - MoveStartTime) / MaxSpeedTime);
    }

    void UpdateMove()
    {
        // Ÿ������������ �Ÿ� ���ϱ�. 
        float distance = Vector3.Distance(TargetPosition, transform.position);

        // ��������
        if (distance == 0)
        {
            Arrived();
            return;
        }

        // ����ӵ� = ���⺤��(direction vector, unit vector) * �ӷ�
        CurrentVelocity = (TargetPosition - transform.position).normalized * CurrentSpeed;

        // Vector3.SmoothDamp(������ġ, ��ǥ����, ����ӵ�, �ð�, �ִ�ӷ�) �� �������� ������ ����.
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, distance / CurrentSpeed, MaxSpeed);
    }

    void UpdateBattle()
    {
        if (Time.time - LastBattleUpdateTime > 1.0f)
        {
            if (FireRemainCount > 0 )
            {
                Fire();
                FireRemainCount--;
            }
            else
            {
                Disappear(new Vector3(-15.0f, transform.position.y, transform.position.z));
            }
            LastBattleUpdateTime = Time.time;
        }
    }

    void Arrived()
    {
        CurrentSpeed = 0.0f;

        if (CurrentState == State.Appear)
        {
            CurrentState = State.Battle;
            LastBattleUpdateTime = Time.time;
        }

        else // if (CurrentState == State.Disappear)
        {
            CurrentState = State.None;
        }
    }

    public void Appear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
        MoveStartTime = Time.time;
    }

    void Disappear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0.0f;

        CurrentState = State.Disappear;
        MoveStartTime = Time.time;
    }

    // Collision
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("other = " + other.name);
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            if (!player.IsDead)
                player.OnCrash(this, CrashDamage);
        }
    }

    public override void OnCrash(Actor attacker, int damage)
    {
        base.OnCrash(attacker, damage);
    }

    public void Fire()
    {
        GameObject go = Instantiate(Bullet);
        Bullet bullet = go.GetComponent<Bullet>();
        // Enemy Bullet�� ��ǥ�踦 Y������ 180 ȸ�� �߱� ������ right�� ������ �ڵ����� �ݴ밡 ��.
        bullet.Fire(this, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
    }

    protected override void OnDead(Actor killer)
    {
        base.OnDead(killer);

        SystemManager.Instance.GamePointAccumulator.Accumulate(GamePoint);
        CurrentState = State.Dead;
        Destroy(gameObject);
    }
}
