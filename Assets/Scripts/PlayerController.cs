using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("移动速度")] public float speed;
    private Vector2 movement;

    [Header("弹跳力")] public float jumpForce;


    [Header("平台Layer")] public LayerMask platformLayer;

    [Header("风力")] public float windPower;

    [Header("生命值")] public float healthPoint = 100;
    [Header("分数")] public float scorePoint = 0;

    [Header("陷阱伤害")] public float trapDamage = 10;

    [Header("HP显示")] public Text healthPointText;
    [Header("分数显示")] public Text ScoreText;
    public float maxJumpCount = 2;
    private float _jumpCount;
    private bool _jumpKeyDown;

    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;

    public bool
        dead,
        isGround,
        hit,
        fall,
        doubleJump,
        jump,
        run;

    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int Fall = Animator.StringToHash("fall");
    private static readonly int DoubleJump = Animator.StringToHash("doubleJump");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Run = Animator.StringToHash("run");


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;

        movement.x = Input.GetAxisRaw("Horizontal");
        _jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
        ChangeAnimator();
        IsGround();
        JumpTrigger();
        Score();
        Dead();
    }


    void FixedUpdate()
    {
        Move();
    }

    public void MoveToLeft()
    {
        movement.x = -1;
        Debug.Log("MoveToLeft" + movement.x);
    }

    public void MoveToRight()
    {
        movement.x = 1;
        Debug.Log("MoveToRight" + movement.x);
    }

    public void JumpButton()
    {
        _jumpKeyDown = true;
    }

    private void Score()
    {
        scorePoint += Time.deltaTime;
        ScoreText.text = scorePoint.ToString(CultureInfo.CurrentCulture);
    }

    /**
     * 判定角色DEAD
     */
    private void Dead()
    {
        healthPointText.text = healthPoint.ToString(CultureInfo.CurrentCulture);
        if (healthPoint <= 0)
        {
            dead = true;
            GameManager.ShowGamePanel();
        }
    }


    //碰到风扇上方会吹起来
    //碰到平台就落下
    //碰到弹跳会起跳

    /**
     * 触发平台效果
     */
    public void FallingPlatformTrigger()
    {
        SoundManager.PlayFallingplatformSound();
    }

    /**
     * 触发尖刺球效果
     */
    public void SpikedBallTrigger(GameObject attackObject)
    {
        //不重复受伤
        if (!hit)
            healthPoint -= trapDamage;
        var positionX = attackObject.transform.position.x;
        if (positionX < transform.position.x)
            _rigidbody2D.AddForce(Vector2.right * jumpForce);
        else
            _rigidbody2D.AddForce(Vector2.left * jumpForce);
        SoundManager.PlayHitSound();
        hit = true;
    }

    /**
     * 触发尖刺效果
     */
    public void SpikeTrigger()
    {
        //不重复受伤
        if (!hit)
            healthPoint -= trapDamage;
        SoundManager.PlayHitSound();
        hit = true;
        _rigidbody2D.AddForce(Vector2.down * jumpForce);
    }

    /**
     * 判断是否落在平台上
     */
    private void IsGround()
    {
        isGround = _boxCollider2D.IsTouchingLayers(platformLayer);
    }

    /*
     * 修改角色动画
     */
    private void ChangeAnimator()
    {
        fall = false;
        if (_rigidbody2D.velocity.y < 0)
            fall = true;

        jump = false;
        if (_rigidbody2D.velocity.y > 0)
            jump = true;

        run = false;
        if (Math.Abs(_rigidbody2D.velocity.x) > 0 && isGround)
            run = true;

        if (_rigidbody2D.velocity.x < 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        if (_rigidbody2D.velocity.x > 0)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

        _animator.SetBool(Hit, hit);
        _animator.SetBool(Fall, fall);
        _animator.SetBool(DoubleJump, doubleJump);
        _animator.SetBool(Jump, jump);
        _animator.SetBool(Run, run);
    }

    private void RecoverIdle()
    {
        if (hit)
            hit = false;
    }


    private void Move()
    {
        _rigidbody2D.velocity = new Vector2(movement.x * speed * Time.fixedDeltaTime, _rigidbody2D.velocity.y);
    }

    /**
     * 跳跃
     * 在平台上才能起跳
     * 跳跃次数不能超过maxJumpCount
     */
    private void JumpTrigger()
    {
        // Debug.Log("jump" + jump);
        //一段跳
        if (_jumpKeyDown && isGround)
        {
            _jumpCount++;
            _rigidbody2D.AddForce(Vector2.up * jumpForce);
            jump = true;
            SoundManager.PlayJumpSound();
        }

        //二段跳
        if (_jumpKeyDown && !isGround)
        {
            jump = false;
            doubleJump = true;
            _jumpCount++;
            if (_jumpCount < maxJumpCount)
            {
                _rigidbody2D.AddForce(Vector2.up * jumpForce);
                SoundManager.PlayJumpSound();
            }
        }

        //触碰平台重置跳跃
        if (_boxCollider2D.IsTouchingLayers())
        {
            jump = doubleJump = false;
            _jumpCount = 0;
        }
    }

    //触发风扇效果
    public void FanTrigger(GameObject platform)
    {
        _rigidbody2D.AddForce(Vector2.up * windPower);
    }

    //触发蹦床效果
    public void TrampolineTrigger(GameObject platform)
    {
        _rigidbody2D.AddForce(Vector2.up * jumpForce);
        SoundManager.PlayTrampolineSound();
    }
}