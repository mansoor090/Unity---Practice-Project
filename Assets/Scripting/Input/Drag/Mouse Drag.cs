using UnityEngine;
using UnityEngine.EventSystems;

public class Doc_Interact : MonoBehaviour
{

	// <- Public Or Serialized (Private) Variables ->

	[SerializeField]                   Camera      camera;
	[SerializeField]                   EventSystem eventSystem;
	[SerializeField] public            GameObject  currentObj;
	[SerializeField]                   Vector2     screenClamp;
	[SerializeField]                   LayerMask   dragableMask;
	[SerializeField, Range(0.1f, 10f)] float       throwForce;
	[SerializeField, Range(0.1f, 4f)]  float       height;
	[SerializeField, Range(0.1f, 10f)] float       lerpSpeed;


	// <- Private Variables ->
	Vector2                    Position;
	Vector2                    deltaChange;
	[SerializeField] Rigidbody currentRigidbody;
	[SerializeField] Collider  currentCollider;
	// <- Properties ->

	public Vector2 DeltaChange => deltaChange;
	public Vector2 ScreenClamp
	{
		get => screenClamp;
		set => screenClamp = value;
	}

	public float ThrowForce
	{
		get => throwForce;
		set => throwForce = value;
	}
	public float Height
	{
		get => height;
		set => height = value;
	}
	public float LerpSpeed
	{
		get => lerpSpeed;
		set => lerpSpeed = value;
	}

	// <- Functions ->


	void Start()
	{
		LoadValues();
	}

	void LoadValues()
	{
		// throwForce = SaveData.Instance.throwForce;
		// height = SaveData.Instance.height;
		// lerpSpeed = SaveData.Instance.followSpeed;
	}

	void Update()
	{
		HandleDrag();
	}


	void HandleDrag()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Position = eventSystem.currentInputModule.input.mousePosition;
		}

		if (Input.GetMouseButton(0))
		{
			// HandlePosition
			Vector2 currentPos = eventSystem.currentInputModule.input.mousePosition;
			deltaChange = currentPos - Position;
			Position    = currentPos;

			//HandleRaycast
			if (!currentObj)
			{
				RaycastHit hitInfo;
				if (Physics.Raycast(camera.ScreenPointToRay(new Vector3(currentPos.x, currentPos.y, 1f)), out hitInfo, Mathf.Infinity, dragableMask))
				{
					currentObj                   = hitInfo.collider.gameObject;
					currentRigidbody             = currentObj.GetComponent<Rigidbody>();
					currentRigidbody.isKinematic = true;
					currentCollider              = currentObj.GetComponent<Collider>();
					currentCollider.enabled      = false;
				}
			}
			else
			{
				float   xPos      = Mathf.Lerp(-screenClamp.x, screenClamp.x, currentPos.x / Screen.width);
				float   zPos      = Mathf.Lerp(-screenClamp.y, screenClamp.y, currentPos.y / Screen.height);
				Vector3 wantedPos = new Vector3(xPos, height, zPos);

				//move & rotate object to mouse position
				currentObj.transform.position = Vector3.Lerp(currentObj.transform.position, wantedPos, Time.deltaTime               * lerpSpeed);
				currentObj.transform.rotation = Quaternion.Slerp(currentObj.transform.rotation, Quaternion.identity, Time.deltaTime * this.lerpSpeed);
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			if (currentObj)
			{
				currentCollider.enabled      = true;
				currentRigidbody.isKinematic = false;
				currentRigidbody.AddForce(new Vector3(deltaChange.x, 0f, deltaChange.y) * throwForce, ForceMode.Acceleration);
				// empty Object
				currentObj       = null;
				currentRigidbody = null;
			}
		}
	}

}