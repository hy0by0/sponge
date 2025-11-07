using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public GameObject WaterBulletPrefab; // ここに水弾のプレハブを入れる
    public int fire = 1;                //発射左右方向変数
    public float bulletSpeed = 1f;    //弾発射スピード
    bool SplashMode = false;
    public float intervalTime = 0.3f; //再び水を吸えるまでの時間(水放出アニメーションの時間にしておく)
    public float time = 0;

    public bool BigMode = false;
    public float BigScale = 4.0f;
    public float NormalScale = 1.0f;
    float Scale_X;
    float Scale;
    BoxCollider2D boxCol;

    //public int DashCount = 1;
    bool canDash = true;
    bool isDashing = false;
    [SerializeField] float dashingForce = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashCoolDown = 1.0f;

    public float speed = 15.0f;
    public float jumpForce = 10.0f;
    float axisX = 0.0f;
    public string direction = "right";
    public bool jumpFlag = false;

    public GroundCheck groundCheck;
    public bool onGround = false;

    public static string gameState = "playing";


    //Animator animator;
    //public string stopAnime = "PlayerStopAnime";
    //public string missAnime = "PlayerMissAnime";
    //string nowAnime = "";
    //string oldAnime = "";

    [SerializeField] TrailRenderer tr;

    Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        //animator = GetComponent<Animator>();
        //nowAnime = stopAnime;
        //oldAnime = stopAnime;
        NormalScale = this.transform.localScale.x;
        Scale_X = this.transform.localScale.x;
        gameState = "playing";
        tr.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }

        if (isDashing)//ダッシュ中なら何もしない
        {
            return;
        }

        axisX = Input.GetAxisRaw("Horizontal");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (BigMode)
            {
                SplashMode = true;
                for (int i = 0; i < 6; i += 2) //右方向へ水の弾を放出させる
                {
                    GameObject WaterBullet = Instantiate(WaterBulletPrefab, transform.position, Quaternion.identity); //水弾生成
                    Rigidbody2D WaterRbody = WaterBullet.GetComponent<Rigidbody2D>(); //水のrbody取得
                    Vector2 bulletVec = new Vector2(bulletSpeed + axisX * speed * 0.5f, 2 + i); //撃ちだすベクトルを決める
                    WaterRbody.AddForce(bulletVec, ForceMode2D.Impulse); //決めたベクトル方向へ撃ちだす
                }

                for (int i = 0; i < 6; i += 2) //左方向へも水を放出させる
                {
                    GameObject WaterBullet = Instantiate(WaterBulletPrefab, transform.position, Quaternion.identity); //水弾生成
                    Rigidbody2D WaterRbody = WaterBullet.GetComponent<Rigidbody2D>(); //水のrbody取得
                    Vector2 bulletVec = new Vector2(-bulletSpeed + axisX * speed * 0.5f, 2 + i); //撃ちだすベクトルを決める
                    WaterRbody.AddForce(bulletVec, ForceMode2D.Impulse); //決めたベクトル方向へ撃ちだす
                }

                ChangeScale(false);
            }

            else if (canDash)
            {
                StartCoroutine(Dash());//ダッシュのコルーチン開始
            }

        }



        if (axisX > 0)
        {
            this.transform.localScale = new Vector2(Scale_X, this.transform.localScale.y);
            direction = "right";
        }
        else if (axisX < 0)
        {
            this.transform.localScale = new Vector2(-Scale_X, this.transform.localScale.y);
            direction = "left";
        }
    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }

        if (isDashing)//ダッシュ中なら何もしない
        {
            return;
        }

        if (SplashMode)
        {
            if (time < intervalTime)
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                SplashMode = false;
            }
        }

        onGround = groundCheck.IsGround();

        if (jumpFlag)
        {
            rbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            jumpFlag = false;
        }

        rbody.velocity = new Vector2(axisX * speed, rbody.velocity.y);

        //if (nowAnime != oldAnime) // アニメーションの変更を反映(ミスとクリア以外)
        //{
        //    oldAnime = nowAnime;
        //animator.Play(nowAnime);
        //}
    }


    public void Jump()
    {
        if (onGround)
        {
            jumpFlag = true;
        }

    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rbody.gravityScale;//元の重力を代入
        rbody.gravityScale = 0;
        rbody.velocity = new Vector2(transform.localScale.x * dashingForce, 0);//重力がない状態で向いてる方向にダッシュ
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rbody.gravityScale = originalGravity;//重力をもとに戻す
        isDashing = false;
        //rbody.velocity = new Vector2(0, 0);//速度を0にする

        yield return new WaitForSeconds(dashCoolDown);

        canDash = true;
    }


    public void InWater()
    {
        ChangeScale(true);
    }

    public void ChangeScale(bool bigflag)
    {
        if (bigflag)
        {
            if (!BigMode && !SplashMode)
            {
                int Direct_x = 1;
                if (this.transform.localScale.x / NormalScale >= 0)
                {
                    Direct_x = 1;
                }
                else
                {
                    Direct_x = -1;
                }
                Scale_X *= BigScale;
                Scale = NormalScale * BigScale;
                transform.DOScale(new Vector2(Direct_x * Scale, Scale), 0.3f);
                BigMode = true;
            }
        }
        else
        {
            if (BigMode)
            {
                int Direct_x = 1;
                if (this.transform.localScale.x / BigScale >= 0)
                {
                    Direct_x = 1;
                }
                else
                {
                    Direct_x = -1;
                }
                Scale_X = NormalScale;
                Scale = NormalScale;
                transform.DOScale(new Vector2(Direct_x * Scale, Scale), 0.3f);
                BigMode = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.gameObject.tag == "Water")
        //{
        //    Debug.Log("<color=red>水を吸うぜ！！</color>");
        //    InWater();
        //}

        if (collider.gameObject.tag == "Goal")
        {
            Debug.Log("<color=red>ゴールした</color>");
            Goal();
        }

        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log("<color=red>敵にあたった</color>");
            Miss();
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Water")
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("<color=red>水吸えないよ</color>");
            }
            else
            {
                Debug.Log("<color=red>水を吸うぜ！！</color>");
                InWater();
            }

        }
    }




    public void Goal()
    {
        //Debug.Log("<color=red>まじでゴールした</color>");
        gameState = "clearStage";
        GameStop();
    }


    public void Miss()
    {
        //Debug.Log("<color=red>いってええええ！</color>");
        gameState = "miss";
        //animator.Play(missAnime);
        transform.DOLocalMoveY(1, 1f);
        GameStop();
    }


    void GameStop()
    {
        rbody.velocity = new Vector2(0, 0);
    }
}