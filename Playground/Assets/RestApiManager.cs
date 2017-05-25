using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum RequestMethod
{
	GET = 0,
	POST,
	PUT,
	DELETE
}
	
public class RestApiManager
{
	private RestApiManagerHandler handler;
	private string baseUrl;
	private string apiPath;
	private RequestMethod method;
	private string responseString;
	private string postData;
	private byte[] putData;

	private RestApiManagerHandler Handler { 
		get { return this.handler; } 
		set { this.handler = value; } 
	}

	public string BaseUrl {
		get{ return this.baseUrl; }
		set{ this.baseUrl = value; }
	}

	public string ApiPath {
		get{ return this.apiPath; }
		set{ this.apiPath = value; }
	}

	public RequestMethod Method {
		get{ return this.method; }
		set{ this.method = value; }
	}

	public string PostData {
		get{ return this.postData; }
		set{ this.postData = value; }
	}

	public byte[] PutData {
		get{ return this.putData; }
		set{ this.putData = value; }
	}

	public string ResponseString
	{
		get {
			return this.responseString;
		}
	}

	public string FullUrl {
		get {
			return this.BaseUrl + this.ApiPath;
		}
	}

	private UnityWebRequest engine 
	{
		get {
			switch (this.Method) {
			case (RequestMethod.POST):
				return UnityWebRequest.Post (this.FullUrl, this.PostData);
			case (RequestMethod.PUT):
				return UnityWebRequest.Put (this.FullUrl, this.PutData);
			case (RequestMethod.DELETE):
				return UnityWebRequest.Delete (this.FullUrl);
			default:
				return UnityWebRequest.Get (this.FullUrl);
			}
		}
	}
		
	public RestApiManager (RestApiManagerHandler handler, string baseUrl) 
	{
		this.Handler = handler;

		this.BaseUrl = baseUrl;

		this.Method = RequestMethod.GET;
	}

	public void PerformRequest()
	{
		if (this.Handler == null)
			return;

		this.Handler.StartCoroutine (doInBackground());
	}

	private IEnumerator doInBackground()
	{
		UnityWebRequest www;

		if (this.Method == RequestMethod.POST) 
		{
			if (this.PostData == null)
				this.PostData = "";

			Debug.Log ("POST data: " + this.PostData);

			www = UnityWebRequest.Post (this.FullUrl, this.PostData);

			www.SetRequestHeader ("Content-Type", "application/x-www-form-urlencoded");
		}
		else
			www = UnityWebRequest.Get (this.FullUrl);
		
		yield return www.Send ();

		if (www.isError) 
		{
			yield return www.error;

			this.Handler.OnRestApiManagerError(www.error);
		} 
		else 
		{
			yield return www.downloadHandler.text;

			this.responseString = www.downloadHandler.text;

			this.Handler.OnRestApiManagerSuccess (this);
		}
	}
}
