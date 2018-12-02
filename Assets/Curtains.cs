using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Curtains : MonoBehaviour {
	public FixedCamera Camera;
	public RectTransform Left;
	public RectTransform Right;
	public RectTransform Top;
	public RectTransform Bottom;
	private RectTransform _rect;

	private void Awake() {
		_rect = GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void LateUpdate () {
		float ratio = Camera.Rect.width / Camera.Rect.height;
		float myRatio = _rect.rect.width / _rect.rect.height;
		if (ratio > myRatio) {
			// top/bottom
			Right.sizeDelta = Left.sizeDelta = Vector2.zero;
		} else {
			// left/right
			float spacer = (_rect.rect.width - (_rect.rect.height * ratio)) / 2;
			Left.sizeDelta = Right.sizeDelta = new Vector2(spacer, _rect.rect.height);
			Top.sizeDelta = Bottom.sizeDelta = Vector2.zero;
			Left.anchoredPosition = new Vector2(0, 0);
			Right.anchoredPosition = new Vector2(_rect.rect.width - spacer, 0);
		}
	}
}
