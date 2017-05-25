using UnityEngine;

public abstract class RestApiManagerHandler : MonoBehaviour
{
	public abstract void OnRestApiManagerSuccess(RestApiManager manager);
	public abstract void OnRestApiManagerError(string error);
}