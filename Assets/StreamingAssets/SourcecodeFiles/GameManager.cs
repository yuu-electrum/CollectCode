using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{

	//-----------------------------------------------
	// ゲームを管理するマネージャスクリプト.
	//-----------------------------------------------
	private ElapsedTime elapsedTime;
	private UITimerTextController   timer;
	private UIProgressBarController progress;
	private LifeManager life;
	private Pause pauser;

	public void Awake()
	{

		// 動作FPSを60FPSに固定する.
		Application.targetFrameRate = 60;

		elapsedTime = new ElapsedTime();

	}

	public void Start()
	{

		timer    = GameObject.Find("UI").GetComponent<UITimerTextController>();
		progress = GameObject.Find("UI").GetComponent<UIProgressBarController>();
		life     = GameObject.Find("LifeManager").GetComponent<LifeManager>();
		pauser   = GameObject.Find("Pause").GetComponent<Pause>();

	}

	public void Update()
	{

		if(timer == null || progress == null || life == null || pauser == null) return;

		// 残りのゲーム時間を表示する.
		timer.Time(elapsedTime.GetElapsedTime());
		progress.Progress(elapsedTime.GetElapsedTime());

		if(life.Lives == 0 || elapsedTime.GetElapsedTime() == 3600)
		{

			// 残機が0になったり最後まで生き残ったらすべてのGameObjectを止める.
			pauser.PauseAll();

			// リザルト画面を生成する.
			Object result = Resources.Load("Result");
			if(result == null) return;

			GameObject instance = Instantiate(result, this.transform) as GameObject;

			string text = (life.Lives > 0) ? "クリアしました！" : "ゲームオーバー......";
			foreach(Transform t in instance.transform)
			{

				// リザルト画面にテキストを表示する.
				if(t.name != "MainText") continue;
				t.GetComponent<UnityEngine.UI.Text>().text = text;

			}

		}

		elapsedTime.Tick();

	}

}