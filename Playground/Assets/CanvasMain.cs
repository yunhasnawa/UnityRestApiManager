using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;

public class CanvasMain : /* For REST API*/ RestApiManagerHandler /* End For*/ 
{
	private Button buttonSebelumnya;
	private Button buttonSelanjutnya;
	private Button buttonTestApi;
	private Text textInfo;
	private Image imageJawabanA;
	private Image imageJawabanB;
	private Image imageJawabanC;

	private int nomorHalaman;

	// For REST API
	private const string API_BASE_URL = "http://alacarte.kuwais.net/api"; // Ganti dengan alamat web Anda!
	private RestApiManager apiManager;
	// End For

	// Use this for initialization
	public void Start () 
	{
		Debug.Log ("[CanvasMain] I'm alive!");

		this.nomorHalaman = 1;

		this.initComponents ();

	}
	
	// Update is called once per frame
	public void Update () 
	{
		
	}

	private void initComponents()
	{
		this.imageJawabanA = GameObject.Find ("ImageJawabanA").GetComponent<Image> ();
		this.imageJawabanB = GameObject.Find ("ImageJawabanB").GetComponent<Image> ();
		this.imageJawabanC = GameObject.Find ("ImageJawabanC").GetComponent<Image> ();
		this.textInfo = GameObject.Find ("TextInfo").GetComponent<Text> ();
		this.buttonSebelumnya = GameObject.Find ("ButtonSebelumnya").GetComponent<Button> ();
		this.buttonSelanjutnya = GameObject.Find ("ButtonSelanjutnya").GetComponent<Button> ();
		this.buttonTestApi = GameObject.Find ("ButtonTestApi").GetComponent<Button> ();

		this.buttonSebelumnya.onClick.AddListener (() => buttonSebelumnya_onClick());
		this.buttonSelanjutnya.onClick.AddListener (() => buttonSelanjutnya_onClick());
		this.buttonTestApi.onClick.AddListener (() => buttonTestApi_onClick());

		this.changeJawabanImages ();

		// For REST API
		this.apiManager = new RestApiManager (this, API_BASE_URL);
		// End For
	}

	public void buttonSebelumnya_onClick()
	{
		Debug.Log ("[ButtonSebelumnya] I'm clicked!");

		if (this.nomorHalaman > 1)
			this.nomorHalaman--;

		this.changeJawabanImages ();
	}

	public void buttonSelanjutnya_onClick()
	{
		Debug.Log ("[ButtonSelanjutnya] I'm clicked!");

		if (this.nomorHalaman < 3)
			this.nomorHalaman++;

		this.changeJawabanImages ();
	}

	private void changeJawabanImages()
	{
		string assetNameA = this.nomorHalaman + "_a";
		string assetNameB = this.nomorHalaman + "_b";
		string assetNameC = this.nomorHalaman + "_c";
		
		Sprite spriteA = Resources.Load<Sprite> (assetNameA);
		Sprite spriteB = Resources.Load<Sprite> (assetNameB);
		Sprite spriteC = Resources.Load<Sprite> (assetNameC);

		this.imageJawabanA.sprite = spriteA;
		this.imageJawabanB.sprite = spriteB;
		this.imageJawabanC.sprite = spriteC;

		string info = "Kuis halaman: " + this.nomorHalaman;
		this.textInfo.text = info;
	}

	// For REST API
	public void buttonTestApi_onClick()
	{
		// Ganti dengan pengaturan yang sesuai alamat web Anda!
		this.apiManager.Method = RequestMethod.POST;
		this.apiManager.ApiPath = "/public/credential/login";
		// Ini parameter yang dikirim (jika methodnya POST. Kalau GET, gabungin langsung di ApiPath pakai tanda tanya '?');
		// Contoh: this.apiManager.ApiPath = "/public/credential/login?api_key=abc1234&username=yunhasnawa&password=bismillah";
		this.apiManager.PostData = "api_key=abc1234&username=yunhasnawa&password=bismillah";

		this.apiManager.PerformRequest ();
	}

	public override void OnRestApiManagerSuccess(RestApiManager manager)
	{
		string result = manager.ResponseString;

		EditorUtility.DisplayDialog ("Tes Aja", result, "OK");

		Debug.Log (result);
	}

	public override void OnRestApiManagerError(string error)
	{
		EditorUtility.DisplayDialog ("Tes Aja", error, "OK");

		Debug.LogError ("REST API Response: " + "(ERROR) " + error);
	}
	// End For
}
