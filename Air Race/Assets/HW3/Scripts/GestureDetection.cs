using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Attributes;
/**
 ** For reusability, the following functions' contents can be removed:
 **		-resetComponents()
 ** 	-GestureDelegation
 ** Methods below //-------------------------- can be deleted
 **/
namespace Leap.Unity {
	public class GestureDetection : MonoBehaviour {
		public enum HandJointObj {
			THUMB, INDEX, MIDDLE, PINKY, RING, PALM, FOREARM
		}

		struct HandGestureInfo {
			public Leap.Hand hand;
			public bool isPalmOpen, isFist, isThumbsUp, isThumbsDown;

			public HandGestureInfo(int i = 0) {
				hand = null;
				isPalmOpen = false;
				isFist = false;
				isThumbsUp = false;
				isThumbsDown = false;
			}

			public void setIsFist(bool b) { isFist = b; }
			public void setIsPalmOpen(bool b) { isPalmOpen = b; }
			public void setIsThumbsUp(bool b) { isThumbsUp = b; }
			public void setIsThumbsDown(bool b) { isThumbsDown = b; }
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
			bool isUpwards = thumbPinkyDistance > 0.09f;
			bool isDownwards = thumbPinkyDistance < 0.078f;
			hand.isThumbsUp = extended && closedFist && isUpwards;
			hand.isThumbsDown = extended && closedFist && isDownwards;

			return hand;
		}

		private HandGestureInfo resetHand(HandGestureInfo h) {
			h.isFist = false;
			h.isPalmOpen = false;
			h.isThumbsUp = false;
			h.hand = null;

			resetComponents();

			return h;
		}

	//---------------------------------------------------------------------------------
	//---------------------Application Specific Method Bodies--------------------------
	//---------------------------------------------------------------------------------

		private void resetComponents() {
			GetComponent<PlayerMovement>().setIsFlying(false);
			GetComponent<PlayerMovement>().setIsFlying(false);
		}

		private void GestureDelegation(HandGestureInfo hand) {
			//If current phase is track selection
			if (isSelectingTrack) {
				isSelectingTrack = trackSelectionPhase(hand);
				isCountdown = !isSelectingTrack;
				if (isCountdown)
					GetComponent<AudioController>().playTrackSelected();
			}
			//Countdown phase
			else if (isCountdown) {
				if (hand.hand.IsLeft)
					countdown.setLeftThumbsUp(hand.isThumbsUp);
				else 
					countdown.setRightThumbsUp(hand.isThumbsUp);

				if (countdown.getIsReady()) {
					isFlying = true;
					isCountdown = false;
					countdown.startCountdown();
				}
				else {
					if (hand.isThumbsDown)
						GetComponent<PlayerPositionSwitcher>().switchPosition();
				}
			}
			//AirRace phase
			else if (isFlying) {
				if (hand.hand.IsLeft && GetComponent<PlayerMovement>().enabled)
					GetComponent<PlayerMovement>().setIsFlying(hand.isFist);
				else {
					GetComponent<PlayerMovement>().fly(getDirection(hand));
				}
			}
		}

	//---------------------------------------------------------------------------------
	//---------------------Application Specific Methods--------------------------------
	//---------------------------------------------------------------------------------
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

		//Flying phase
		private Vector3 getDirection(HandGestureInfo hand) {
			Vector3 dir = hand.hand.PalmNormal.ToVector3();
			dir = dir.normalized;
			return new Vector3(dir.x, dir.y, dir.z);
		}

		public bool getLeftFist() { return handL.isFist; }
	}
}