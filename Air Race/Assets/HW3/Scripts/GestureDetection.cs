using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Attributes;

namespace Leap.Unity {
	public class GestureDetection : MonoBehaviour {
		public enum HandJointObj {
			THUMB, INDEX, MIDDLE, PINKY, RING, PALM, FOREARM
		}

		struct HandGestureInfo {
			public Leap.Hand hand;
			public bool isPalmOpen, isFist, isThumbsUp;

			public HandGestureInfo(int i = 0) {
				hand = null;
				isPalmOpen = false;
				isFist = false;
				isThumbsUp = false;
			}

			public void setIsFist(bool b) { isFist = b; }
			public void setIsPalmOpen(bool b) { isPalmOpen = b; }
			public void setIsThumbsUp(bool b) { isThumbsUp = b; }
		}

		[SerializeField] private RigidHand _leftHand;
		[SerializeField] private RigidHand _rightHand;
		[SerializeField] private CapsuleHand _leftCHand;
		[SerializeField] private CapsuleHand _rightCHand;

		private HandGestureInfo handL;
		private HandGestureInfo handR;

		private Countdown countdown;

		private bool isSelectingTrack = true;
		private bool isCountdown = false;
		private bool isFlying = false;

		void Start() {
			handL = new HandGestureInfo();
			handR = new HandGestureInfo();

			countdown = GetComponent<Countdown>();
		}

		void FixedUpdate() {
			handL.hand = _leftHand.GetLeapHand();
			handR.hand = _rightHand.GetLeapHand();

			if (handL.hand != null) {
				handL = setHand(handL);
				GestureDelegation(handL);
			}
			else 
				handL = resetHand(handL);

			if (handR.hand != null) {
				handR = setHand(handR);
				GestureDelegation(handR);
			}
			else
				handR = resetHand(handR);

		}

		private HandGestureInfo setHand(HandGestureInfo hand) {
			//Check Fist
			hand.setIsFist(hand.hand.GetFistStrength() > 0.7f);
			//Check Palm
			hand.isPalmOpen = hand.hand.GetFistStrength() < 0.13f;
			//check ThumbsUp
			float thumbPinkyDistance = hand.hand.GetThumb().TipPosition.y - hand.hand.GetPinky().TipPosition.y;
			bool extended = hand.hand.GetThumb().IsExtended;
			bool closedFist = hand.hand.GetFistStrength() > 0.78f;
			bool isUpwards = thumbPinkyDistance > 0.1f;
			hand.isThumbsUp = extended && closedFist && isUpwards;

			return hand;
		}

		private HandGestureInfo resetHand(HandGestureInfo h) {
			h.isFist = false;
			h.isPalmOpen = false;
			h.isThumbsUp = false;
			h.hand = null;

			return h;
		}

		private void GestureDelegation(HandGestureInfo hand) {
			//If current phase is track selection
			if (isSelectingTrack) {
				isSelectingTrack = trackSelectionPhase(hand);
				isCountdown = !isSelectingTrack;
			}

			if (isCountdown) {
				if (hand.hand.IsLeft)
					countdown.setLeftThumbsUp(hand.isThumbsUp);
				else 
					countdown.setRightThumbsUp(hand.isThumbsUp);

				if (countdown.getIsReady()) {
					isFlying = true;
					isCountdown = false;
					countdown.startCountdown();
				}
			}

			if (isFlying) {
				if (hand.hand.IsLeft) {
					//flyingScript.accelerate(hand.isFist);
				}
				else {
					Vector3 sourcePos = hand.hand.PalmPosition.ToVector3();
					Vector3 endPos = hand.hand.GetMiddle().TipPosition.ToVector3();
					//might have to translate to WORLD coordinates
					//flyingScript.move(hand.isPalm, sourcePos - endPos)
				}
			}
		}

		//Track selection phase
		private bool trackSelectionPhase(HandGestureInfo hand) {
			HandRaycaster hr;

			if (hand.hand.IsLeft) 
				hr = _leftHand.GetComponent<HandRaycaster>();
			else
				hr = _rightHand.GetComponent<HandRaycaster>();

			Vector3 palmPos = hand.hand.PalmPosition.ToVector3();
			Vector3 dir = hand.hand.PalmNormal.ToVector3();

			Color sphereColor = Color.blue;

			if (hand.hand.IsLeft) {
				dir = _leftHand.transform.GetChild((int)HandJointObj.PALM).transform.GetChild(0).position;
				if (_leftCHand.getSphereMat() == null)
					return true;

				sphereColor = _leftCHand.getSphereMat().color;
			}
			else {
				dir = _rightHand.transform.GetChild((int)HandJointObj.PALM).transform.GetChild(0).position;
				if (_rightCHand.getSphereMat() == null)
					return true;

				sphereColor = _rightCHand.getSphereMat().color;
			}

			dir = dir - palmPos;
			dir = dir.normalized;

			//returns true if selection is complete (reversed here)
			bool enableRaycaster = hr.shootRaycast(palmPos, dir, hand.isPalmOpen, sphereColor);

			//Diabled the raycaster if the selection is complete
			if (enableRaycaster) {
				hr = _leftHand.GetComponent<HandRaycaster>();
				hr.enabled = false;
				hr = _leftHand.GetComponent<HandRaycaster>();
				hr.enabled = false;
			}

			return !enableRaycaster;
		}


	}
}