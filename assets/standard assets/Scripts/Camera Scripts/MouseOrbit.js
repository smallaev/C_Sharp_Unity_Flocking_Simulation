var target : Transform;
var distance = 10.0;
var distance1;

var xSpeed = 250.0;
var ySpeed = 120.0;

var yMinLimit = -20;
var yMaxLimit = 80;

private var x = 0.0;
private var y = 0.0;

    var sensitivityDistance = -7.5f;
    var damping = 2.5f;
    var min = -15;
    var max = -2;
    var zdistance: Vector3;
@script AddComponentMenu("Camera-Control/Mouse Orbit")

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
   
     distance1 = -10f;
        distance1 = transform.localPosition.z;
 

}

function Update() {	
if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel")>0) {

	transform.Translate(transform.forward*20);

}

if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel")<0) {

	transform.Translate(transform.forward*-20);

}

}

function LateUpdate () {

if (Input.mousePosition.x<Screen.width-160) {
	if (Input.GetMouseButton (2)) {
		
		if (Input.GetKey(KeyCode.LeftAlt)) {
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
 		
 		y = ClampAngle(y, yMinLimit, yMaxLimit);
 		       
        var rotation = Quaternion.Euler(y, x, 0);
 //       var position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
        
        transform.rotation = rotation;
		
		
	
		} else {
			var h = -50.0f * Input.GetAxis ("Mouse X");
			var v = -50.0f * Input.GetAxis ("Mouse Y");
			transform.Translate (h, v, 0);
		}
	}
	
	if (Input.GetMouseButton (1)) {
	x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
 		
 		y = ClampAngle(y, yMinLimit, yMaxLimit);
 		       
        var rotation1 = Quaternion.Euler(y, x, 0);
 //       var position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
        
        transform.rotation = rotation1;
		
	}
	
	if (Input.GetMouseButton (0)) {
	var h1 = -50.0f * Input.GetAxis ("Mouse X");
			var v1 = -50.0f * Input.GetAxis ("Mouse Y");
			transform.Translate (h1, v1, 0);
		
	}
	
   
        
}
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}