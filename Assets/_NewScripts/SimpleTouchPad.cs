﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	//Origin point to set
	public float smoothing;

	private Vector2 origin;
	private Vector2 direction;
	private Vector2 smoothDirection;
	private bool touched; 
	private int pointerID;

	void Awake () {
		direction = Vector2.zero;
		touched = false;
	}

	public void OnPointerDown (PointerEventData data) {
		//Set out start point
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;
			origin = data.position;
		}
	}

	public void OnDrag (PointerEventData data) {
		//Compare the diffrence between our start point and current pointer position
		if (data.pointerId == pointerID) {
			Vector2 currentPosition = data.position;
			Vector2 directionRaw = currentPosition - origin;
			direction = directionRaw.normalized;
		}
	}

	public void OnPointerUp (PointerEventData data) {
		//Reset Everything
		if (data.pointerId == pointerID) {
			direction = Vector2.zero;
			touched = false;
		}
	}

	public Vector2 GetDirection () {
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;
	}
}
