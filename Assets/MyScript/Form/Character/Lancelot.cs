using UnityEngine;
using System.Collections;

public class Lancelot : MonoBehaviour {

	private Character thisCharacter;

	public Character ThisCharacter {
		get {
			return thisCharacter;
		}
		set {
			thisCharacter = value;
		}
	}

	// Use this for initialization
	void Start () {
		thisCharacter = new Servant ("Lancelot", 3,Servant.ServantClass.Berserker, null, "Berserker");
		if (gameObject.name == "Lancelot") {
			System.Console.WriteLine("Lancelot appeared!!!!");
			//Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnPointerEnter()
	{
		System.Console.WriteLine("This Character is Lancelot");
	}
}
