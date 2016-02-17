using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	public bool paused = false;
	public GameObject pauseMenu;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseToggle();
		}
	}

	void PauseToggle()
	{
		paused = !paused;
		pauseMenu.SetActive(paused);

		if (paused)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	void ExitGame()
	{
		Application.Quit();
	}
}
