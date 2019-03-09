//
// Unityちゃん用の三人称カメラ
// 
// 2013/06/07 N.Kobyasahi
//
using UnityEngine;
using System.Collections;
using UnityEngine.PostProcessing;

namespace UnityChan
{
	public class ThirdPersonCamera : MonoBehaviour
	{
        public PostProcessingProfile timeSlowProfile;
        public PostProcessingProfile normalProfile;
        public PostProcessingProfile gameOverProfile;
        public PostProcessingProfile pauseProfile;
        public TimeController controller;
		public float smooth = 3f;		// カメラモーションのスムーズ化用変数
		Transform standardPos;			// the usual position for the camera, specified by a transform in the game
		Transform frontPos;         // Front Camera locater

        // スムーズに繋がない時（クイック切り替え）用のブーリアンフラグ
        public TimeController control;
        bool gameStart = false;

        public GameObject Canvas;

        public UnityChanControlScriptWithRgidBody script;
	
	
		void Start ()
		{
			// 各参照の初期化
			standardPos = GameObject.Find ("CamPos").transform;
		
			if (GameObject.Find ("FrontPos"))
				frontPos = GameObject.Find ("FrontPos").transform;

			//カメラをスタートする
			//transform.position = standardPos.position;	
			//transform.forward = standardPos.forward;	
		}
	
		void FixedUpdate ()	// このカメラ切り替えはFixedUpdate()内でないと正常に動かない
		{
            gameStart = script.gameStart;
            Canvas.SetActive(!gameStart);

			if (gameStart)
            {
				// return the camera to standard position and direction
				setCameraPositionNormalView ();
			}
            else
            {
                transform.position = Vector3.Lerp(transform.position, frontPos.position, Time.fixedDeltaTime * smooth); 
                transform.forward = Vector3.Lerp (transform.forward, frontPos.forward, Time.fixedDeltaTime * smooth);
            }

            if (controller.timeSlow)
            {
                GetComponent<PostProcessingBehaviour>().profile = timeSlowProfile;
            }
            else if (control.health <= 0)
            {
                GetComponent<PostProcessingBehaviour>().profile = gameOverProfile;
            }
            else if (TimeScale.player == 0)
            {
                GetComponent<PostProcessingBehaviour>().profile = pauseProfile;
            }
            else
            {
                GetComponent<PostProcessingBehaviour>().profile = normalProfile;
            }
		}

		void setCameraPositionNormalView ()
		{
			//if (control.timeSlow == false) {
				// the camera to standard position and direction
				transform.position = Vector3.Lerp (transform.position, standardPos.position, Time.fixedDeltaTime * smooth);	
				transform.forward = Vector3.Lerp (transform.forward, standardPos.forward, Time.fixedDeltaTime * smooth);
			/*} else {
				// the camera to standard position and direction / Quick Change
				transform.position = standardPos.position;	
				transform.forward = standardPos.forward;
			}*/
		}

        public void openGame()
        {
            gameStart = true;
        }
	}
}