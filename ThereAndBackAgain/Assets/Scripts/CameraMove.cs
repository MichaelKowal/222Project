using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	[SerializeField]
	private float cameraSpeed = 0;

	private float xMax;
	private float yMin;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		GetInput();
	}

	private void GetInput()
	{
		if(Input.GetKey(KeyCode.W) || Input.mousePosition.y > Camera.main.pixelHeight - 20)
		{
			transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < 20)
		{
			transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < 20)
		{
			transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Camera.main.pixelWidth - 20)
		{
			transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
		}

		transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin, 0), -10);
	}

	public void SetCamLimit(Vector3 maxTile)
	{
		Vector3 point = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));

		//if camera zoom is added this needs tweak
		xMax = maxTile.x - point.x;
		yMin = maxTile.y - point.y;
	}
}
