﻿//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
    // 必要なコンポーネントの列記
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class UnityChanControlScriptWithRgidBody : MonoBehaviour
    {

        public float animSpeed = 1.5f;              // アニメーション再生速度設定
        public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
        public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                    // このスイッチが入っていないとカーブは使われない
        public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）
        public bool onSkater = false;

        // 以下キャラクターコントローラ用パラメタ
        // 前進速度
        public float forwardSpeed = 7.0f;
        // 後退速度
        public float backwardSpeed = 3.0f;
        // 旋回速度
        public float rotateSpeed = 3.0f;
        // ジャンプ威力
        public float jumpPower = 1.0f;
        // キャラクターコントローラ（カプセルコライダ）の参照
        private CapsuleCollider col;
        private Rigidbody rb;
        // キャラクターコントローラ（カプセルコライダ）の移動量
        private Vector3 velocity;
        // CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
        private float orgColHight;
        private Vector3 orgVectColCenter;
        private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
        private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照
        private int idleTime = 0;

        public AudioSource baka;

        private GameObject cameraObject;    // メインカメラへの参照

        // アニメーター各ステートへの参照
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int locoState = Animator.StringToHash("Base Layer.Locomotion");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");
        static int restState = Animator.StringToHash("Base Layer.Rest");
        static int waitState = Animator.StringToHash("Base Layer.IdleAnim");
        static int damage = Animator.StringToHash("Base Layer.Damage");
        static int slide = Animator.StringToHash("Base Layer.Slide");
        static int poke = Animator.StringToHash("Base Layer.Poke");

        public bool attacking = false;
        public bool damaged = false;

        public bool gameStart = false;

        // 初期化
        void Start()
        {
            // Animatorコンポーネントを取得する
            anim = GetComponent<Animator>();
            // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            //メインカメラを取得する
            cameraObject = GameObject.FindWithTag("MainCamera");
            // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
            orgColHight = col.height;
            orgVectColCenter = col.center;
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        }

        void Update()
        {
            if (Input.GetButtonDown("Jump") && gameStart)
            {  // スペースキーを入力したら

                //アニメーションのステートがLocomotionの最中のみジャンプできる
                    //ステート遷移中でなかったらジャンプできる
                    if (!anim.IsInTransition(0))
                    {
                        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                        anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                    }
            }
        }


        // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
        void FixedUpdate()
        {
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);    // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
            if (damaged || attacking)
            {
                col.center = new Vector3(0, .5f, 0);
                col.height = .5f;
            }
            else
            {
                resetCollider();
            }
            if (gameStart && !damaged)
            {
                float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
                float v = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定義
                anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
                anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
                anim.speed = animSpeed * TimeScale.player;                             // Animatorのモーション再生速度に animSpeedを設定する
                rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする



                // 以下、キャラクターの移動処理
                velocity = new Vector3(0, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得
                                                        // キャラクターのローカル空間での方向に変換
                velocity = transform.TransformDirection(velocity);
                //以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
                if (v > 0.1)
                {
                    velocity *= forwardSpeed;       // 移動速度を掛ける
                }
                else if (v < -0.1)
                {
                    velocity *= backwardSpeed;  // 移動速度を掛ける
                }


                // 上下のキー入力でキャラクターを移動させる
                transform.localPosition += velocity * Time.fixedDeltaTime * TimeScale.player;

                // 左右のキー入力でキャラクタをY軸で旋回させる
                transform.Rotate(0, h * rotateSpeed * TimeScale.player, 0);
            }


            // 以下、Animatorの各ステート中での処理
            // Locomotion中
            // 現在のベースレイヤーがlocoStateの時
            if (currentBaseState.fullPathHash == locoState)
            {
                attacking = false;
                //カーブでコライダ調整をしている時は、念のためにリセットする
                idleTime = 0;
                if (useCurves)
                {
                    resetCollider();
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    anim.SetBool("Attack", true);
                    attacking = true;
                }
            }
            // JUMP中の処理
            // 現在のベースレイヤーがjumpStateの時
            else if (currentBaseState.fullPathHash == jumpState)
            {
                if (!anim.IsInTransition(0))
                {

                    // 以下、カーブ調整をする場合の処理
                    if (useCurves)
                    {
                        // 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
                        // JumpHeight:JUMP00でのジャンプの高さ（0〜1）
                        // GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
                        float jumpHeight = anim.GetFloat("JumpHeight");
                        float gravityControl = anim.GetFloat("GravityControl");
                        if (gravityControl > 0)
                            rb.useGravity = false;  //ジャンプ中の重力の影響を切る

                        // レイキャストをキャラクターのセンターから落とす
                        Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
                        RaycastHit hitInfo = new RaycastHit();
                        // 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
                        if (Physics.Raycast(ray, out hitInfo))
                        {
                            if (hitInfo.distance > useCurvesHeight)
                            {
                                col.height = orgColHight - jumpHeight;          // 調整されたコライダーの高さ
                                float adjCenterY = orgVectColCenter.y + jumpHeight;
                                col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
                            }
                            else
                            {
                                // 閾値よりも低い時には初期値に戻す（念のため）					
                                resetCollider();
                            }
                        }
                    }
                    // Jump bool値をリセットする（ループしないようにする）				
                    anim.SetBool("Jump", false);
                }
            }
            // IDLE中の処理
            // 現在のベースレイヤーがidleStateの時
            else if (currentBaseState.fullPathHash == idleState)
            {
                attacking = false;
                damaged = false;
                //カーブでコライダ調整をしている時は、念のためにリセットする
                idleTime++;
                if (useCurves)
                {
                    resetCollider();
                }

                if (idleTime > 360)
                {
                    int rand = Random.Range(0, 2);

                    switch (rand)
                    {
                        case 0:
                            anim.SetBool("Rest", true);
                            break;
                        case 1:
                            anim.SetBool("IdleAnim", true);
                            break;
                        default:
                            anim.SetBool("IdleAnim", true);
                            break;
                    }

                    idleTime = 0;
                }
                // スペースキーを入力したらRest状態になる
                if (Input.GetButtonDown("Jump") && !anim.IsInTransition(0))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    anim.SetBool("Jump", true);
                }
            }
            // REST中の処理
            // 現在のベースレイヤーがrestStateの時
            else if (currentBaseState.fullPathHash == restState || currentBaseState.fullPathHash == waitState
                                                                || currentBaseState.fullPathHash == poke)
            {
                //cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
                // ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Rest", false);
                    anim.SetBool("IdleAnim", false);
                    anim.SetBool("Poke", false);
                }
                if (Input.GetButtonDown("Jump") && !anim.IsInTransition(0))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    anim.SetBool("Jump", true);
                }
            }
            else if (currentBaseState.fullPathHash == damage)
            {
                damaged = true;
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Damage", false);
                }
            }
            else if (currentBaseState.fullPathHash == slide)
            {
                attacking = true;
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Attack", false);
                }
            }
        }
        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Skater"))
            {
                forwardSpeed = 1.4f;
                backwardSpeed = .6f;
            }
            else if (other.gameObject.CompareTag("Axle"))
            {
                forwardSpeed = 3.5f;
                backwardSpeed = 1.5f;
            }
            if (other.gameObject.CompareTag("Skater"))
            {
                onSkater = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Platform") || other.gameObject.CompareTag("Skater") || other.gameObject.CompareTag("Axle"))
            {
                forwardSpeed = 7f;
                backwardSpeed = 3f;
                onSkater = false;
            }
        }

        // キャラクターのコライダーサイズのリセット関数
        void resetCollider()
        {
            // コンポーネントのHeight、Centerの初期値を戻す
            col.height = orgColHight;
            col.center = orgVectColCenter;
        }

        public void ClickOnUnityChan()
        {
            anim.SetBool("Poke", true);
            baka.PlayDelayed(1f);
        }

        public void openGame()
        {
            gameStart = true;
        }

        public void Restart()
        {
            anim.SetFloat("Speed", 0);
            gameStart = false;
        }
    }
}
