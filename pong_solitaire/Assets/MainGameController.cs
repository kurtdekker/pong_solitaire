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

	Camera ourCamera;
	void Start()
	{
		ourCamera = Camera.main;
		StartCoroutine (UpdateTheScoreAndHighScore ());

		SetupGameColors ();
	}

	Material mtl_borders;
	Material mtl_paddle;
	Material mtl_back;
	
	void SetupGameColors()
	{
		mtl_borders = new Material(Shader.Find("Diffuse"));
		mtl_back = new Material(Shader.Find("Diffuse"));
		mtl_paddle = new Material(Shader.Find("Diffuse"));
		
		mtl_borders.color = new Color (1.0f, 0.5f, 0.5f);
		mtl_back.color = new Color (0.8f, 1.0f, 1.0f);
		mtl_paddle.color = new Color (0.4f, 0.8f, 0.8f);
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

	// relating to the playfield area
	const float thickness = 0.25f;
	const float halfThick = thickness / 2;
	const float zDepth = 5.0f;
	GameObject playfield;
	float width,height;

	void FabricateThePlayfield()
	{
		if (playfield)
		{
			Destroy ( playfield);
		}

		playfield = new GameObject ("Playfield");

		// Calculate playfield height based on how far back we are from
		// the camera and the camera's fieldOfView.
		height = Mathf.Tan (ourCamera.fieldOfView * Mathf.Deg2Rad * 0.5f) *
			Mathf.Abs ( Camera.main.transform.position.z - playfield.transform.position.z);

		// match playfield size to screen aspect
		width = (height * Screen.width) / Screen.height;

		// top edge
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.name = "Top";
		cube.GetComponent<Renderer> ().material = mtl_borders;
		cube.transform.parent = playfield.transform;
		cube.transform.localPosition = Vector3.up * (height + halfThick);
		cube.transform.localScale = new Vector3 ((width + thickness) * 2, thickness, zDepth);
		// and the bottom
		cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.name = "Bottom";
		cube.GetComponent<Renderer> ().material = mtl_borders;
		cube.transform.parent = playfield.transform;
		cube.transform.localPosition = Vector3.down * (height + halfThick);
		cube.transform.localScale = new Vector3 ((width + thickness) * 2, thickness, zDepth);
		// far right wall
		cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.name = "Right";
		cube.GetComponent<Renderer> ().material = mtl_borders;
		cube.transform.parent = playfield.transform;
		cube.transform.localPosition = Vector3.right * (width + halfThick);
		cube.transform.localScale = new Vector3 (thickness, height * 2, zDepth);
		// back wall
		cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.name = "BackWall";
		cube.GetComponent<Renderer> ().material = mtl_back;
		cube.transform.parent = playfield.transform;
		cube.transform.localPosition = Vector3.forward * zDepth * 0.5f;
		cube.transform.localScale = new Vector3 (width * 2, height * 2, thickness);
	}

	void InitGame()
	{
		FabricateThePlayfield ();
		score = 0;

		playerPaddle = GameObject.CreatePrimitive (PrimitiveType.Cube);
		playerPaddle.transform.localScale = new Vector3 (playerPaddleXSize, playerPaddleYZSize, playerPaddleYZSize);
		playerPaddle.GetComponent<Renderer> ().material = mtl_paddle;

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

	GameObject playerPaddle;
	float playerXPositionFractionOfWidth = -0.80f;
	float playerPaddleSizeFractionOfHeight = 0.15f;
	const float playerPaddleXSize = thickness;
	float playerPaddleYZSize
	{
		get
		{
			return height * playerPaddleSizeFractionOfHeight;
		}
	}

	void GatherPlayerMouseInput()
	{
		Plane plane = new Plane (Vector3.back, playfield.transform.position);
		float enter;
		Ray ray = ourCamera.ScreenPointToRay (Input.mousePosition);
		if (plane.Raycast ( ray, out enter))
		{
			Vector3 playerPosition = ray.GetPoint ( enter);

			// jam at at the left edge where you belong
			playerPosition.x = width * playerXPositionFractionOfWidth;

			// constrain top and bottom
			if (playerPosition.y > height - playerPaddleYZSize / 2)
			{
				playerPosition.y = height - playerPaddleYZSize / 2;
			}
			if (playerPosition.y < -height - playerPaddleYZSize / 2)
			{
				playerPosition.y = -height - playerPaddleYZSize / 2;
			}
			playerPaddle.transform.position = playerPosition;
		}
	}

	void PlayOneFrame()
	{
		GatherPlayerMouseInput();
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
			PlayOneFrame();
			break;
		}
	}
}
