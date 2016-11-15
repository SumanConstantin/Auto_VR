using UnityEngine;
using System.Collections;

public class MainModel : MonoBehaviour {

	///[SerializeField]
	//private GameObject prefabAuto1, prefabAuto2, prefabAuto3;

	public GameObject prefabAuto1;
	public GameObject prefabAuto2;
	public GameObject prefabAuto3;

	[SerializeField]
	private GameObject camera1, camera2, cameraHolder1, cameraHolder2;
	[SerializeField]
	private GameObject canvas1, canvas2;

	private GameObject autoObj;

	private GameObject doorR;
	private GameObject doorL;
	private GameObject wiperR;
	private GameObject wiperL;
	private GameObject doorWindowR;
	private GameObject doorWindowL;
	private GameObject doorHandleR;
	private GameObject doorHandleL;
	private GameObject doorHandle2R;
	private GameObject doorHandle2L;
	private GameObject trunk;

	private Light exteriorLightR;
	private Light exteriorLightL;
	private Light interiorLight;

	private float windowRInitPositionY = 0f;
	private float windowLInitPositionY = 0f;
	private const float rotationSpeed = .5f;
	private float rotationDirection = 0;		//	-1 => right; 1 => left; 0 => no rotation

	private Vector3 initialPosition = new Vector3( 27, 11, 2);
	private Quaternion initialRotation = new Quaternion( 0f, 0.24f, 0f, 1.0f);

	// Use this for initialization
	void Start () {
		SelectAuto(1);
	}

	private void SetupInitialAutoFunctionality()
	{
		// TODO: the namings of left and right doors are not consistent
		// The door window's animation's hierarchy is not consistent
		doorR = autoObj.transform.Find("car_exterior/R_car_door").gameObject;
		doorL = autoObj.transform.Find("car_exterior/L_car_L_door").gameObject;
		wiperR = autoObj.transform.Find("car_exterior/car_wipers/R_wiper").gameObject;
		wiperL = autoObj.transform.Find("car_exterior/car_wipers/L_wiper").gameObject;
		doorWindowR = autoObj.transform.Find("car_exterior/R_car_door/R_door_window").gameObject;
		doorWindowL = autoObj.transform.Find("car_exterior/L_car_L_door/L_door_window/L_door_window").gameObject;
		doorHandleR = autoObj.transform.Find("car_exterior/R_car_door/R_handles/R_handle2").gameObject;
		doorHandleL = autoObj.transform.Find("car_exterior/L_car_L_door/L_handles/L_handle2").gameObject;
		doorHandle2R = autoObj.transform.Find("car_exterior/R_car_door/R_handles/R_handle3").gameObject;
		doorHandle2L = autoObj.transform.Find("car_exterior/L_car_L_door/L_handles/L_handle3").gameObject;
		trunk = autoObj.transform.Find("car_exterior/car_trunk_door").gameObject;
		exteriorLightR = autoObj.transform.Find("Light_system/R_headlight").GetComponent<Light>();
		exteriorLightL = autoObj.transform.Find("Light_system/L_headlight").GetComponent<Light>();
		interiorLight = autoObj.transform.Find("Light_system/interior_light").GetComponent<Light>();

		windowRInitPositionY = doorWindowR.transform.position.y;
		windowLInitPositionY = doorWindowL.transform.position.y;

		ResetAnimatedFunctionality();
	}

	public void ResetAnimatedFunctionality()
	{		
		// Reset position of all elements
		doorR.transform.Rotate(Vector3.up, -doorR.transform.localEulerAngles.y);
		doorL.transform.Rotate(Vector3.up, -doorL.transform.localEulerAngles.y);
		wiperR.transform.Rotate(Vector3.forward, -wiperR.transform.localEulerAngles.z);
		wiperL.transform.Rotate(Vector3.forward, -wiperL.transform.localEulerAngles.z);
		doorWindowR.transform.position = new Vector3(doorWindowR.transform.position.x, windowRInitPositionY, doorWindowR.transform.position.z);
		doorWindowL.transform.position = new Vector3(doorWindowL.transform.position.x, windowLInitPositionY, doorWindowL.transform.position.z);
		doorHandleR.transform.Rotate(Vector3.up, 0f);
		doorHandleL.transform.Rotate(Vector3.up, 0f);
		doorHandle2R.transform.Rotate(Vector3.right, 0f);
		doorHandle2L.transform.Rotate(Vector3.right, 0f);
		trunk.transform.Rotate(Vector3.right, -trunk.transform.localEulerAngles.x);

		// Stop all animations
		doorR.GetComponent<Animation>().Stop();
		doorL.GetComponent<Animation>().Stop();
		wiperR.GetComponent<Animation>().Stop();
		wiperL.GetComponent<Animation>().Stop();
		doorWindowR.GetComponent<Animation>().Stop();
		doorWindowL.GetComponent<Animation>().Stop();
		doorHandleR.GetComponent<Animation>().Stop();
		doorHandleL.GetComponent<Animation>().Stop();
		doorHandle2R.GetComponent<Animation>().Stop();
		doorHandle2L.GetComponent<Animation>().Stop();
		trunk.GetComponent<Animation>().Stop();

		// Set wrap mode
		doorR.GetComponent<Animation>().wrapMode = WrapMode.Once;
		doorL.GetComponent<Animation>().wrapMode = WrapMode.Once;
		wiperR.GetComponent<Animation>().wrapMode = WrapMode.Once;
		wiperL.GetComponent<Animation>().wrapMode = WrapMode.Once;
		doorWindowR.GetComponent<Animation>().wrapMode = WrapMode.Once;
		doorWindowL.GetComponent<Animation>().wrapMode = WrapMode.Once;
		doorHandleR.GetComponent<Animation>().wrapMode = WrapMode.Once;
		doorHandleL.GetComponent<Animation>().wrapMode = WrapMode.Once;
		doorHandle2R.GetComponent<Animation>().wrapMode = WrapMode.Once;
		doorHandle2L.GetComponent<Animation>().wrapMode = WrapMode.Once;
		trunk.GetComponent<Animation>().wrapMode = WrapMode.Once;

		// Set negative speed, which changes the direction of play
		//(will be switched to positive at the moment of activation, before playing)
		doorR.GetComponent<Animation>()["R_door_open"].speed = -1;
		doorL.GetComponent<Animation>()["L_door_open"].speed = -1;
		wiperR.GetComponent<Animation>()["R_wiper_move"].speed = -1;
		wiperL.GetComponent<Animation>()["L_wiper_move"].speed = -1;
		doorWindowR.GetComponent<Animation>()["R_window_down"].speed = -1;
		doorWindowL.GetComponent<Animation>()["L_window_down"].speed = -1;
		doorHandleR.GetComponent<Animation>()["R_door_handle"].speed = -1;
		doorHandleL.GetComponent<Animation>()["L_door_handle_2"].speed = -1;	// Naming inconsistency
		doorHandle2R.GetComponent<Animation>()["R_door_handle_2"].speed = -1;
		doorHandle2L.GetComponent<Animation>()["L_door_handle"].speed = -1;		// Naming inconsistency
		trunk.GetComponent<Animation>()["trunk_open"].speed = -1;
	}
		
	// Update is called once per frame
	void Update () {
		UpdateRotation();
	}

	//-------------------
	// Select Auto

	public void SelectAuto(int prefabNr)
	{
		Quaternion rotation = initialRotation;

		if(autoObj != null)
		{
			Quaternion rot = autoObj.transform.rotation;
			rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);
			Destroy(autoObj);
		}

		GameObject prefab = (GameObject)this.GetType().GetField("prefabAuto"+prefabNr.ToString()).GetValue(this);
		autoObj = (GameObject)Instantiate(prefab, initialPosition, rotation);

		SetupInitialAutoFunctionality();
	}

	//-------------------

	//-------------------
	// Rotate Auto

	void UpdateRotation()
	{
		autoObj.transform.Rotate(Vector3.up, rotationSpeed * rotationDirection);
	}

	public void SetRotateDirection(int value)
	{
		rotationDirection = value;
	}

	public void ResetRotation()
	{
		Quaternion r = initialRotation;
		autoObj.transform.rotation = new Quaternion( r.x, r.y, r.z, r.w );
		SetRotateDirection(0);
	}
	//-----------------------------------

	//---------------------
	// Auto Functionality

	public void Doors()
	{
		float animSpeed = 6;
		Animation anim;
		AnimationState animState;

		// Right door
		anim = doorR.GetComponent<Animation>();
		animState = anim["R_door_open"];
		StartAnim(anim, animState, animSpeed);

		// Left door
		anim = doorL.GetComponent<Animation>();
		animState = anim["L_door_open"];
		StartAnim(anim, animState, animSpeed);
	}

	private void StartAnim(Animation anim, AnimationState animState, float baseSpeed)
	{
		float speed = animState.speed > 0 ? baseSpeed * -1 : baseSpeed;
		animState.speed = speed;
		animState.time = speed > 0 ? 0 : animState.length;
		anim.Play();
	}

	public void Trunk()
	{
		float animSpeed = 1.5f;
		Animation anim;
		AnimationState animState;

		// Trunk
		anim = trunk.GetComponent<Animation>();
		animState = anim["trunk_open"];
		StartAnim(anim, animState, animSpeed);
	}

	public void ResetFunctionality()
	{
		ResetAnimatedFunctionality();
		exteriorLightR.enabled = exteriorLightL.enabled = interiorLight.enabled = false;
	}

	public void Lights()
	{
		exteriorLightR.enabled = !exteriorLightR.enabled;
		exteriorLightL.enabled = !exteriorLightL.enabled;

		interiorLight.enabled = !interiorLight.enabled;
	}

	//------------------------------------


	//---------------------
	// Camera
	private Vector3 cameraPostition1 = new Vector3(0f, 0f, 0f);
	private Vector3 cameraPostition2 = new Vector3(0f, 0f, 0f);
	private Vector3 cameraRotation1 = new Vector3(0f, 0f, 0f);
	private Vector3 cameraRotation2 = new Vector3(0f, 0f, 0f);
	private bool camSwitched = false;
	public void SwitchCameraPosition()
	{
		//camera1.SetActive(!camera1.activeSelf);
		//camera2.SetActive(!camera2.activeSelf);
		Vector3 pos = camSwitched ? cameraPostition1 : cameraPostition2;
		camera1.transform.position = new Vector3(pos.x, pos.y, pos.z);

		camSwitched = !camSwitched;
	}

	public void SwitchCamera()
	{
		ResetRotation();
		
		Vector3 position;
		Vector3 rotation;
		GameObject parent;

		if(!camSwitched)
		{
			parent = cameraHolder2;
			position = cameraPostition2;
			rotation = cameraRotation2;
		}
		else
		{
			parent = cameraHolder1;
			position = cameraPostition1;
			rotation = cameraRotation1;
		}

		camera1.transform.SetParent(parent.transform);
		camera1.transform.localPosition = new Vector3(0f, 0f, 0f);

		canvas1.SetActive(!canvas1.activeInHierarchy);
		canvas2.SetActive(!canvas2.activeInHierarchy);

		camSwitched = !camSwitched;
	}

	//------------------------------------
}
