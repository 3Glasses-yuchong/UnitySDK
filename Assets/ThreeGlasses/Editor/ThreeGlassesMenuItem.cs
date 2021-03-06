﻿#define TGDEBUG
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ThreeGlasses
{
#if UNITY_EDITOR

    public class ThreeGlassesMenuItem
    {
        const string kEnableHeadDisplay = "3Glasses/EnableHeadDisplay";

        [MenuItem(kEnableHeadDisplay, true)]
        public static bool ToggleSimulationModeValidate()
        {
            Menu.SetChecked(kEnableHeadDisplay, GameObject.FindObjectOfType(typeof(ThreeGlassesCamera)) != null);
            return true;
        }

        [MenuItem(kEnableHeadDisplay)]
		public static void ToggleSimulationMode ()
		{
            if(GameObject.FindObjectOfType(typeof(ThreeGlassesCamera)) != null)
            {
                clear3Glasses();
            }
            else
            {
                clear3Glasses();
                add3Glasses();
            }
        }

        public static void add3Glasses()
        {
            // bind camera
            Camera[] cams = GameObject.FindObjectsOfType<Camera>();
            float depth = 0;
            Camera cam;
            if(cams.Length > 0)
            {
                depth = cams[0].depth;
                cam = cams[0];
                foreach (var item in cams)
                {
                    if(item.depth < depth)
                    {
                        depth = item.depth;
                        cam = item;
                    }
                }
            }
            else
            {
                GameObject mainCamera = new GameObject("MainCamera");
                Undo.RegisterCreatedObjectUndo(mainCamera, "create camera");
                cam = Undo.AddComponent<Camera>(mainCamera);
            }

            // bind script
            Undo.AddComponent<ThreeGlassesCamera>(cam.gameObject);
            
            Selection.activeGameObject = cam.gameObject;
        }
        public static void clear3Glasses()
        {
            // remove ThreeGlassesCamera form maincamera
            Component[] objects = GameObject.FindObjectsOfType<ThreeGlassesCamera>();
            foreach (var item in objects)
            {
                Undo.DestroyObjectImmediate(item);
            }
            // remove ThreeGlassesSubCamera
            objects = GameObject.FindObjectsOfType<ThreeGlassesSubCamera>();
            foreach (var item in objects)
            {
                Undo.DestroyObjectImmediate(item);
            }
        }
    }
#endif
}
