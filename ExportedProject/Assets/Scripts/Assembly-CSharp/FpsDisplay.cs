using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
	private float deltaTime;

	private void Awake()
	{
		Application.targetFrameRate = 60;
	}

	private void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	private void OnGUI()
	{
		int width = Screen.width;
		int height = Screen.height;
		GUIStyle gUIStyle = new GUIStyle();
		Rect position = new Rect(0f, 0f, width, height);
		gUIStyle.alignment = TextAnchor.UpperLeft;
		gUIStyle.fontSize = height * 2 / 100;
		gUIStyle.normal.textColor = new Color(255f, 255f, 255f, 1f);
		float num = deltaTime * 1000f;
		float num2 = 1f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
		GUI.Label(position, text, gUIStyle);
	}
}
