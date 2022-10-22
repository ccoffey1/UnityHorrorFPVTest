using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
DragRigidbodyUse.cs ver. 11.1.16 - wirted by ThunderWire Games * Script for Drag & Drop & Throw Objects & Draggable Door & PickupObjects
*/

[System.Serializable]
public class GrabObjectClass
{
	public bool m_FreezeRotation;
	public float m_PickupRange = 3f;
	public float m_ThrowStrength = 50f;
	public float m_distance = 3f;
	public float m_maxDistanceGrab = 4f;
}

[System.Serializable]
public class ItemGrabClass
{
	public bool m_FreezeRotation;
	public float m_ItemPickupRange = 2f;
	public float m_ItemThrow = 45f;
	public float m_ItemDistance = 1f;
	public float m_ItemMaxGrab = 2.5f;
}

[System.Serializable]
public class DoorGrabClass
{
	public float m_DoorPickupRange = 2f;
	public float m_DoorThrow = 10f;
	public float m_DoorDistance = 2f;
	public float m_DoorMaxGrab = 3f;
}

[System.Serializable]
public class TagsClass
{
	public string m_InteractTag = "Interact";
	public string m_InteractItemsTag = "InteractItem";
	public string m_DoorsTag = "Door";
}

public class DragRigidBodyUse : MonoBehaviour
{

	public GameObject playerCam;

	public string GrabButton = "Grab";
	public string ThrowButton = "Throw";
	public string UseButton = "Use";
	public GrabObjectClass ObjectGrab = new GrabObjectClass();
	public ItemGrabClass ItemGrab = new ItemGrabClass();
	public DoorGrabClass DoorGrab = new DoorGrabClass();
	public TagsClass Tags = new TagsClass();

	private float PickupRange = 3f;
	private float ThrowStrength = 50f;
	private float Distance = 3f;
	private float MaxDistanceGrab = 4f;

	private Ray playerAim;
	private GameObject objectHeld;
	private GameObject objectThrown;
	private bool isObjectHeld;
	private bool tryPickupObject;

	void Start()
	{
		isObjectHeld = false;
		tryPickupObject = false;
		objectHeld = null;
	}

	void FixedUpdate()
	{
		if (Input.GetButton(GrabButton))
		{
			if (!isObjectHeld)
			{
				tryPickObject();
				tryPickupObject = true;
			}
			else
			{
				HoldObject();
			}
		}
		else if (isObjectHeld)
		{
			DropObject();
		}

		if (Input.GetButton(ThrowButton) && isObjectHeld)
		{
			isObjectHeld = false;
			objectHeld.GetComponent<Rigidbody>().useGravity = true;
			ThrowObject();
		}

		if (Input.GetButton(UseButton))
		{
			Use();
		}
	}

	private void tryPickObject()
	{
		Ray playerAim = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		if (Physics.Raycast(playerAim, out hit, PickupRange) && hit.collider.gameObject != objectThrown)
		{
			objectHeld = hit.collider.gameObject;
			if (hit.collider.CompareTag(Tags.m_InteractTag) && tryPickupObject)
			{
				isObjectHeld = true;
				Rigidbody objectBody = objectHeld.GetComponent<Rigidbody>();
				objectBody.useGravity = false;
				objectBody.freezeRotation = ObjectGrab.m_FreezeRotation;

				/**/
				PickupRange = ObjectGrab.m_PickupRange;
				ThrowStrength = ObjectGrab.m_ThrowStrength;
				Distance = ObjectGrab.m_distance;
				MaxDistanceGrab = ObjectGrab.m_maxDistanceGrab;
			}
			if (hit.collider.CompareTag(Tags.m_InteractItemsTag) && tryPickupObject)
			{
				isObjectHeld = true;
				Rigidbody objectBody = objectHeld.GetComponent<Rigidbody>();
				objectBody.useGravity = true;
				objectBody.freezeRotation = ItemGrab.m_FreezeRotation;

				/**/
				PickupRange = ItemGrab.m_ItemPickupRange;
				ThrowStrength = ItemGrab.m_ItemThrow;
				Distance = ItemGrab.m_ItemDistance;
				MaxDistanceGrab = ItemGrab.m_ItemMaxGrab;
			}
			if (hit.collider.CompareTag(Tags.m_DoorsTag) && tryPickupObject)
			{
				isObjectHeld = true;
				Rigidbody objectBody = objectHeld.GetComponent<Rigidbody>();
				objectBody.useGravity = true;
				objectBody.freezeRotation = false;

				/**/
				PickupRange = DoorGrab.m_DoorPickupRange;
				ThrowStrength = DoorGrab.m_DoorThrow;
				Distance = DoorGrab.m_DoorDistance;
				MaxDistanceGrab = DoorGrab.m_DoorMaxGrab;
			}
		}
	}

	private void HoldObject()
	{
		Ray playerAim = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

		Vector3 nextPos = playerCam.transform.position + playerAim.direction * Distance;
		Vector3 currPos = objectHeld.transform.position;

		objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

		if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > MaxDistanceGrab)
		{
			DropObject();
		}
	}

	private void DropObject()
	{
		isObjectHeld = false;
		tryPickupObject = false;
		Rigidbody objectBody = objectHeld.GetComponent<Rigidbody>();
		objectBody.useGravity = true;
		objectBody.freezeRotation = false;
		objectBody.velocity = Vector3.zero;
		objectHeld = null;
	}

	private void ThrowObject()
	{
		Rigidbody objectBody = objectHeld.GetComponent<Rigidbody>();
		objectBody.AddForce(playerCam.transform.forward * ThrowStrength);
		objectBody.freezeRotation = false;
		objectThrown = objectHeld;
		StartCoroutine(ClearObjectThrown());
		objectHeld = null;
	}

	private void Use()
	{
		objectHeld.SendMessage("UseObject", SendMessageOptions.DontRequireReceiver); //Every script attached to the PickupObject that has a UseObject function will be called.
	}

	private IEnumerator ClearObjectThrown()
    {
		yield return new WaitForSeconds(0.5f);
		objectThrown = null;
    }
}
