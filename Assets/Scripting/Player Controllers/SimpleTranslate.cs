using UnityEngine;
using UnityEngine.UI;

public class SimpleTranslate : MonoBehaviour
{
	
	private readonly float _moveSpeedMax = 3f;

	private readonly float _moveSpeedSmoothTime = 0.075f;

	private Vector3 _moveSpeedCurrent;

	private Vector3 _moveSpeedTarget;

	private Vector3 _moveSpeedVelocity;

	private Vector3 frameMovement;

	public bool canMove;

	
	private void Update()
	{
		if (canMove)
		{
			HandleInput();
		}
	}
	
	private void HandleInput()
	{
		frameMovement    = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		_moveSpeedTarget = frameMovement.normalized * _moveSpeedMax;
	}

	private void FixedUpdate()
	{
		_moveSpeedCurrent = Vector3.SmoothDamp(_moveSpeedCurrent, _moveSpeedTarget, ref _moveSpeedVelocity, _moveSpeedSmoothTime, float.PositiveInfinity, Time.deltaTime);
		base.transform.Translate(_moveSpeedCurrent * Time.deltaTime, Space.World);
		if (frameMovement != Vector3.zero)
		{
			Quaternion rotation = Quaternion.LookRotation(_moveSpeedCurrent);
		}
	}




}
