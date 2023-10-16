using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FoV))]
public class FoVEditor : Editor {
    
    
    private void OnSceneGUI() {
        FoV fov = (FoV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.ViewDistance);
        Vector3 viewAngleA = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.FieldOfViewAngle / 2);
        Vector3 viewAngleB = DirectionFromAngle(fov.transform.eulerAngles.y, fov.FieldOfViewAngle / 2);
        
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewDistance);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewDistance);
        
        Handles.color = Color.red;

    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees){
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


}
