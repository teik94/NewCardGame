using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public void ToHostScene()
	{
		Application.LoadLevel("HostScene");
	}

	public void ToMenuScene()
	{
		Application.LoadLevel("Menu");
	}
	public void ToJoinScene()
	{
		Application.LoadLevel("JoinScene");
	}
	public void ToInGameScene()
	{
		Application.LoadLevel("InGameScene");
	}
}
