using System;


namespace Custom.Rendering
{
    using UnityEngine;

    [ExecuteAlways]
    public class PlanarReflection : MonoBehaviour
    {
        // referenses
        public Camera        mainCamera;
        public Camera        reflectionCamera;
        public Transform     reflectionPlane;
        public RenderTexture outputTexture;
        public RenderTexture modifiedTexture;
        public Material        reflectionMat;
        // parameters
        public bool copyCameraParamerers;
        public float verticalOffset;
        private bool isReady;

        // cache
        private Transform mainCamTransform;
        private Transform reflectionCamTransform;


        void Start()
        {
            modifiedTexture                = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
            reflectionCamera.targetTexture = modifiedTexture;
            reflectionMat.SetTexture("_ReflectionTex", modifiedTexture);
        }

        private void Update()
        {
        
            if (isReady)
                RenderReflection();
        }

        private void RenderReflection()
        {
            Vector3 cameraDirectionWorldSpace = mainCamTransform.forward;
            Vector3 cameraUpWorldSpace = mainCamTransform.up;
            Vector3 cameraPositionWorldSpace = mainCamTransform.position;

            cameraPositionWorldSpace.y += verticalOffset;

            Vector3 cameraDirectionPlaneSpace = reflectionPlane.InverseTransformDirection(cameraDirectionWorldSpace);
            Vector3 cameraUpPlaneSpace = reflectionPlane.InverseTransformDirection(cameraUpWorldSpace);
            Vector3 cameraPositionPlaneSpace = reflectionPlane.InverseTransformPoint(cameraPositionWorldSpace);

            cameraDirectionPlaneSpace.y *= -1;
            cameraUpPlaneSpace.y *= -1;
            cameraPositionPlaneSpace.y *= -1;

            cameraDirectionWorldSpace = reflectionPlane.TransformDirection(cameraDirectionPlaneSpace);
            cameraUpWorldSpace = reflectionPlane.TransformDirection(cameraUpPlaneSpace);
            cameraPositionWorldSpace = reflectionPlane.TransformPoint(cameraPositionPlaneSpace);

            reflectionCamTransform.position = cameraPositionWorldSpace;
            reflectionCamTransform.LookAt(cameraPositionWorldSpace + cameraDirectionWorldSpace, cameraUpWorldSpace);
        }

        private void OnValidate()
        {
            if (mainCamera != null)
            {
                mainCamTransform = mainCamera.transform;
                isReady = true;
            }
            else
                isReady = false;

            if (reflectionCamera != null)
            {
                reflectionCamTransform = reflectionCamera.transform;
                isReady = true;
            }
            else
                isReady = false;

            if(isReady && copyCameraParamerers)
            {
                copyCameraParamerers = !copyCameraParamerers;
                reflectionCamera.CopyFrom(mainCamera);

                reflectionCamera.targetTexture = modifiedTexture;
            }
        }
    }
}