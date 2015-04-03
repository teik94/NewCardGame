using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization

	void Start () {

	}

	public void CreateDummy ()
	{
		Servant berserker = new Servant ("Lancelot", 3, Servant.ServantClass.Berserker, null, "Berserker");
		Texture2D texture = Resources.Load ("Characters/" + berserker.Asset)  as Texture2D;
		Sprite dummy = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0f, 0f));
		//Instantiate (dummy);


		GameObject mainPanel = GameObject.Find ("Main Panel");
		GameObject go = new GameObject("Test");
		go.name = "Lancelot";
		go.transform.SetParent (mainPanel.transform);

		go.AddComponent(typeof(Lancelot));
		RectTransform rt = go.AddComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (372, 520);
		//rt.SetLeftTopPosition(new Vector2(0,0));
		rt.anchoredPosition = new Vector2(0,0);

		UnityEngine.UI.Image cv = go.AddComponent<UnityEngine.UI.Image>();
		//cv.fillCenter = true;
		cv.sprite = dummy;

	}

    public void OnMouseEnter()
    {
        Debug.Log("Mouse enter");        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

public static class RectTransformExtensions
{
	public static void SetDefaultScale(this RectTransform trans) {
		trans.localScale = new Vector3(1, 1, 1);
	}
	public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec) {
		trans.pivot = aVec;
		trans.anchorMin = aVec;
		trans.anchorMax = aVec;
	}
	
	public static Vector2 GetSize(this RectTransform trans) {
		return trans.rect.size;
	}
	public static float GetWidth(this RectTransform trans) {
		return trans.rect.width;
	}
	public static float GetHeight(this RectTransform trans) {
		return trans.rect.height;
	}
	
	public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
	}
	
	public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
	}
	public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
	}
	public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
	}
	public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) {
		trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
	}
	
	public static void SetSize(this RectTransform trans, Vector2 newSize) {
		Vector2 oldSize = trans.rect.size;
		Vector2 deltaSize = newSize - oldSize;
		trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
		trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
	}
	public static void SetWidth(this RectTransform trans, float newSize) {
		SetSize(trans, new Vector2(newSize, trans.rect.size.y));
	}
	public static void SetHeight(this RectTransform trans, float newSize) {
		SetSize(trans, new Vector2(trans.rect.size.x, newSize));
	}
}
