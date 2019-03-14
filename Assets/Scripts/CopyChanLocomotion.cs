//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace CopyChan
{
    // 必要なコンポーネントの列記
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class CopyChanLocomotion : MonoBehaviour
    {

        public float animSpeed = 1.5f;              // アニメーション再生速度設定
        public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                    // このスイッチが入っていないとカーブは使われない
        public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

        // 以下キャラクターコントローラ用パラメタ
        // 前進速度
        public float forwardSpeed = 6.7f;
        // 旋回速度
        public float rotateSpeed = 4.0f;

        private GameObject UnityChan;
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
        private float walkTime = 0;
        private float proximity = 0;
        private float activeTime = 0;
        private float walkThreshold;

        public bool attacking = false;
        public bool damaged = false;
        public bool active = false;

        // アニメーター各ステートへの参照
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int walkState = Animator.StringToHash("Base Layer.Walk");
        static int runState = Animator.StringToHash("Base Layer.Run");
        static int damage = Animator.StringToHash("Base Layer.Damage");
        static int kick = Animator.StringToHash("Base Layer.Kick");

        // 初期化
        void Start()
        {
            // Animatorコンポーネントを取得する
            anim = GetComponent<Animator>();
            // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            //メインカメラを取得する
            // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
            orgColHight = col.height;
            orgVectColCenter = col.center;
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
            UnityChan = GameObject.Find("unitychan");
            walkThreshold = Random.Range(55, 120);
        }


        // 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
        void FixedUpdate()
        {
            if (transform.position.y < -20)
                Destroy(gameObject);
            if (transform.position.y < .25)
                active = true;
            if (active && activeTime < 30)
            {
                activeTime++;
            }

            currentBaseState = anim.GetCurrentAnimatorStateInfo(0);    // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
            float v = 0;
            if (active && !attacking && !damaged && activeTime >= 30)
            {
                float distance = Vector3.Distance(UnityChan.transform.position, transform.position);
                if (distance < 2)
                {
                    proximity++;
                    if (proximity >= 120)
                    {
                        anim.SetBool("Attack", true);
                    }
                }

                if (walkTime > walkThreshold)
                    v = .9f;              // 入力デバイスの垂直軸をvで定義
                else
                    v = .3f;
                velocity = new Vector3(0, 0, v);
                velocity = transform.TransformDirection(velocity);
                velocity *= forwardSpeed;       // 移動速度を掛ける

                anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
                anim.speed = animSpeed * TimeScale.enemy;                             // Animatorのモーション再生速度に animSpeedを設定する
                rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする


                Vector3 targetDir = new Vector3(UnityChan.transform.position.x, 0 , UnityChan.transform.position.z) 
                                    - new Vector3(transform.position.x, 0, transform.position.z);

                float track = rotateSpeed * Time.fixedDeltaTime * TimeScale.enemy;

                Vector3 direction = Vector3.RotateTowards(transform.forward, targetDir, track, 0.0f);
                // 上下のキー入力でキャラクターを移動させる
                transform.localPosition += velocity * Time.fixedDeltaTime * TimeScale.enemy;

                // 左右のキー入力でキャラクタをY軸で旋回させる
                transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }


            // 以下、Animatorの各ステート中での処理
            // Locomotion中
            // 現在のベースレイヤーがlocoStateの時
            if (currentBaseState.fullPathHash == walkState || currentBaseState.fullPathHash == runState)
            {
                walkTime++;
                //カーブでコライダ調整をしている時は、念のためにリセットする
                if (useCurves)
                {
                    resetCollider();
                }
            }
            // IDLE中の処理
            // 現在のベースレイヤーがidleStateの時
            else if (currentBaseState.fullPathHash == idleState)
            {
                walkTime = 0;
                damaged = false;
                attacking = false;
                //カーブでコライダ調整をしている時は、念のためにリセットする
                if (useCurves)
                {
                    resetCollider();
                }

            }
            // REST中の処理
            // 現在のベースレイヤーがrestStateの時
            else if (currentBaseState.fullPathHash == damage)
            {
                damaged = true;
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Damage", false);
                }
            }
            else if (currentBaseState.fullPathHash == kick)
            {
                proximity = 0;
                anim.speed = 3f * TimeScale.enemy;
                attacking = true;

                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Attack", false);
                }
            }
        }

        // キャラクターのコライダーサイズのリセット関数
        void resetCollider()
        {
            // コンポーネントのHeight、Centerの初期値を戻す
            col.height = orgColHight;
            col.center = orgVectColCenter;
        }
    }
}
