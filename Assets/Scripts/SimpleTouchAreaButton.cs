using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleTouchAreaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private bool touched;
	private int PointID;
	private bool canFire;

	void Awake (){
		touched = false;
		canFire = false;
	}

	public void OnPointerDown(PointerEventData data){
		if (!touched) {
			touched = true;
			canFire = true;
			PointID = data.pointerId;
		}
	}

	public void OnPointerUp(PointerEventData data){
		if (data.pointerId == PointID) {
			touched = false;
			canFire = false;
		}
	}

	public bool CanFire(){
		return canFire;
	}
}