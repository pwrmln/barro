using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	// public Text splash;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene (1, LoadSceneMode.Single);
		}
	}

}
