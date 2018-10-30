using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFaceCamera : MonoBehaviour
{
	void Update ()
    {
       this.transform.LookAt(Camera.main.transform.position);
       this.transform.Rotate(new Vector3(0, 180, 0));
	}
}
