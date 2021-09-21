#if UNITY_EDITOR
      
      using UnityEditor;
      using UnityEngine;
      
      [CustomEditor(typeof( FieldOfView))]
      public class FieldOfViewEditor : Editor
      {
          private void OnSceneGUI()
          {
              FieldOfView fov = (FieldOfView)target;
              Handles.color = Color.white;
              Handles.DrawWireArc(fov.transform.position ,Vector3.up, Vector3.forward,360,fov.viewRadius);
              Handles.color = Color.yellow;
              Handles.DrawWireArc(fov.transform.position,Vector3.up, Vector3.forward , 360,fov.viewRadius + fov.viewRadius);
              Handles.color = Color.white;
              Vector3 viewAngleA = fov.ReturnAngle(fov.viewAngle / 2, false);
              Vector3 viewAngleB = fov.ReturnAngle(-fov.viewAngle / 2, false);
              Handles.DrawLine(fov.transform.position,fov.transform.position+viewAngleA * fov.viewRadius);
              Handles.DrawLine(fov.transform.position,fov.transform.position+viewAngleB * fov.viewRadius);
      
              Handles.color = Color.red;
              if (fov.RequiredTarget)
              {
                  Handles.DrawLine(fov.transform.position + new Vector3(0,0.5f,0), new Vector3(0,0.5f,0) + fov.RequiredTarget.position);
              }
          } 
      }
      #endif
