using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainGameController : MonoBehaviour
{
	public GameObject GreetingScreenRoot;
	public Text TextScore;
	public Text TextHighScore;

	public enum MainGameMode {
		INITIAL,
		GREETING,
		PLAYING,
	}

	MainGameMode mainGameMode = MainGameMode.INITIAL;

	IEnumerator UpdateTheScoreAndHighScore()
	{
		while(true)
		{
			TextScore.text = System.String.Format ("Score: {0}", score);
			TextHighScore.text = System.String.Format ("Best: {0}", score);
			yield return null;
		}
	}

	void Start()
	{
		StartCoroutine (UpdateTheScoreAndHighScore ());
	}

	void ChangeModeToGreeting()
	{
		GreetingScreenRoot.SetActive (true);
		mainGameMode = MainGameMode.GREETING;
	}
	void ChangeModeToPlaying()
	{
		GreetingScreenRoot.SetActive (false);
		mainGameMode = MainGameMode.PLAYING;
		InitGame ();
	}

	public void ButtonActionStartGame()
	{
		ChangeModeToPlaying ();
	}
	public void ButtonActionPLBMGames()
	{
		Application.OpenURL ("http://www.plbm.com");
	}

	int score;

	const string s_highScore = "highScore";
	int highScore
	{
		get
		{
			return PlayerPrefs.GetInt( s_highScore);
		}
		set
		{
			PlayerPrefs.SetInt( s_highScore, value);
		}
	}

	const float zDepth = 5.0f;
	GameObject playfield;
	void FabricateThePlayfield()
	{
		if (playfield)
		{
			Destroy ( playfield);
		}

		playfield = new GameObject ("Playfield");

		// Calculate playfield height based on how far back we are from
		// the camera and the camera's fieldOfView.
		float height = Mathf.Tan (Camera.main.fieldOfView * Mathf.Deg2Rad * 0.5f) *
			Mathf.Abs ( Camera.main.transform.position.z - playfield.transform.position.z);

		// match playfield size to screen aspect
		float width = (height * Screen.width) / Screen.height;

		// top edge
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.name = "Top";
		cube.transform.parent = playfield.transform;
		cube.transform.localPosition = Vector3.up * height;
		cube.transform.localScale = new Vector3 (width * 2, 1, zDepth);
		// and the bottom
		cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.name = "Bottom";
		cube.transform.parent = playfield.transform;
		cube.transform.localPosition = Vector3.down * height;
		cube.transform.localScale = new Vector3 (width * 2, 1, zDepth);
		// far right wall
		cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.name = "Right";
		cube.transform.parent = playfield.transform;
		cube.transform.localPosition = Vector3.right * width;
		cube.transform.localScale = new Vector3 (1.0f, height * 2, zDepth);
		// back wall
		cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.name = "BackWall";
		cube.transform.parent = playfield.transform;
		cube.transform.localPosition = Vector3.forward * zDepth * 0.5f;
		cube.transform.localScale = new Vector3 (width * 2, height * 2, 1.0f);
	}

	void InitGame()
	{
		FabricateThePlayfield ();
		score = 0;

		StartCoroutine (SequenceForGameOver ());
	}

	IEnumerator SequenceForGameOver()
	{
		yield return new WaitForSeconds (2.0f);
		GameOver ();
	}

	void GameOver()
	{
		if (score > highScore)
		{
			highScore = score;
		}
		ChangeModeToGreeting ();
	}

	void Update()
	{
		switch( mainGameMode)
		{
		case MainGameMode.INITIAL :
			ChangeModeToGreeting();
			break;
		case MainGameMode.GREETING :
			if (Input.GetKeyDown( KeyCode.Return))
			{
				ButtonActionStartGame();
			}
			break;
		case MainGameMode.PLAYING :
			break;
		}
	}
}
