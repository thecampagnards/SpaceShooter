using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	public float smoothing;

	private Vector2 orign;
	private Vector2 direction;
	private Vector2 smoothDirection;
	private bool touched;
	private int PointID;

	void Awake (){
		direction = Vector2.zero;
		touched = false;
	}

	public void OnPointerDown(PointerEventData data){
		if (!touched) {
			touched = true;
			PointID = data.pointerId;
			orign = data.position;
		}
	}

	public void OnPointerUp(PointerEventData data){
		if (data.pointerId == PointID) {
			direction = Vector2.zero;
			touched = false;
		}
	}

	public void OnDrag(PointerEventData data){
		if (data.pointerId == PointID) {
			Vector2 current = data.position;
			Vector2 directionRaw = current - orign;
			direction = directionRaw.normalized;
		}
	}

	public Vector2 GetDirection(){
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;
	}
}
