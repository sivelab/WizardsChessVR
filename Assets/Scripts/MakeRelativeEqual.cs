using UnityEngine;
using System.Collections;

public class MakeRelativeEqual : MonoBehaviour
{
	public Transform localReference;
	public Transform toTrack;

	public bool useRotation = false;

	void Update ()
	{
		if (useRotation)
			transform.rotation = toTrack.rotation;
		transform.position = transform.position + (toTrack.position - localReference.position);
	}
}
