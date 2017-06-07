
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpringGUI
{
    public class SpringGUIMenuOptions
    {
        private const string UI_LAYER_NAME             = "UI";
        private const string kStandardSpritePath       = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath     = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath                 = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath            = "UI/Skin/Checkmark.psd";
        private const string kDropdownArrowPath        = "UI/Skin/DropdownArrow.psd";
        private const string kMaskPath                 = "UI/Skin/UIMask.psd";

        private static SpringGUIDefaultControls.Resources s_StandardResources;
        private static SpringGUIDefaultControls.Resources GetStandardResources( )
        {
            if ( s_StandardResources.standard == null )
            {
                s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
                s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePath);
                s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
                s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
                s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
                s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPath);
                s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
            }
            return s_StandardResources;
        }

        private static void SetPositionVisibleinSceneView( RectTransform canvasRTransform , RectTransform itemTransform )
        {
            // Find the best scene view
            SceneView sceneView = SceneView.lastActiveSceneView;
            if ( sceneView == null && SceneView.sceneViews.Count > 0 )
                sceneView = SceneView.sceneViews[0] as SceneView;

            // Couldn't find a SceneView. Don't set position.
            if ( sceneView == null || sceneView.camera == null )
                return;

            // Create world space Plane from canvas position.
            Vector2 localPlanePosition;
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            if ( RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform , new Vector2(camera.pixelWidth / 2 , camera.pixelHeight / 2) , camera , out localPlanePosition) )
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x , 0 , canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y , 0 , canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * ( 0 - canvasRTransform.pivot.x ) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * ( 0 - canvasRTransform.pivot.y ) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * ( 1 - canvasRTransform.pivot.x ) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * ( 1 - canvasRTransform.pivot.y ) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x , minLocalPosition.x , maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y , minLocalPosition.y , maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }
        private static void PlaceUIElementRoot( GameObject element , MenuCommand menuCommand )
        {
            GameObject parent = menuCommand.context as GameObject;
            if (null == parent || parent.GetComponentInParent<Canvas>())
                parent = GetOrCreateCanvasGameObject();
            element.name = GameObjectUtility.GetUniqueNameForSibling(parent.transform,element.name);
            Undo.RegisterCreatedObjectUndo(element,"Create" + element.name);
            Undo.SetTransformParent(element.transform,parent.transform,"Parent" + element.name);
            if( parent != menuCommand.context)

            element.transform.SetParent(parent.transform);
            if(parent != menuCommand.context)
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());
            Selection.activeGameObject = element;
        }
        public static GameObject CreateNewUI( )
        {
            GameObject canvas = new GameObject();
            canvas.name = GameObjectUtility.GetUniqueNameForSibling(null , "Canvas");
            canvas.layer = LayerMask.NameToLayer(UI_LAYER_NAME);
            canvas.AddComponent<Canvas>();
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            Undo.RegisterCreatedObjectUndo(canvas , "Create" + canvas.name);
            CreateEventSystem(false);
            return canvas;
        }
        public static GameObject GetOrCreateCanvasGameObject( )
        {
            GameObject selectedGo = Selection.activeGameObject;
            Canvas canvas = ( selectedGo != null ) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if ( canvas != null && canvas.gameObject.activeInHierarchy )
                return canvas.gameObject;
            canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
            if ( canvas != null && canvas.gameObject.activeInHierarchy )
                return canvas.gameObject;
            return CreateNewUI();
        }
        private static void CreateEventSystem( bool select )
        {
            CreateEventSystem(select , null);
        }
        private static void CreateEventSystem( bool select , GameObject parent )
        {
            var esys = Object.FindObjectOfType<EventSystem>();
            if ( esys == null )
            {
                var eventSystem = new GameObject("EventSystem");
                GameObjectUtility.SetParentAndAlign(eventSystem , parent);
                esys = eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(eventSystem , "Create " + eventSystem.name);
            }

            if ( select && esys != null )
            {
                Selection.activeGameObject = esys.gameObject;
            }
        }

        #region 目录树 && 目录节点模板

        [MenuItem("GameObject/UI/UITree",false,2063)]
        public static void AddUITree( MenuCommand menuCommand ) 
        {
            GameObject uiTree = SpringGUIDefaultControls.CreaatUITree(GetStandardResources());
            PlaceUIElementRoot(uiTree,menuCommand);
        }

        [MenuItem("GameObject/UI/UITreeNode",false,2064)]
        public static void AddUITreeNode( MenuCommand menuCommand )
        {
            GameObject uiTreeNode = SpringGUIDefaultControls.CreaatUITree(GetStandardResources());
            PlaceUIElementRoot(uiTreeNode,menuCommand);
        }

        #endregion

        #region 双击按钮 && 长击按钮

        [MenuItem("GameObject/UI/Buttons/DoubleClickButton" , false , 2065)]
        public static void AddDoubleClickButton( MenuCommand menuCommand )
        {
            GameObject dcButton = SpringGUIDefaultControls.CreateDoubleClickButton(GetStandardResources());
            PlaceUIElementRoot(dcButton,menuCommand);
        }

        [MenuItem("GameObject/UI/Buttons/LongClickButton" , false , 2066)]
        public static void AddLongClickButton( MenuCommand menuCommand )
        {
            GameObject lcButton = SpringGUIDefaultControls.CreateLongClickButton(GetStandardResources());
            PlaceUIElementRoot(lcButton,menuCommand);
        }

        #endregion

        #region 日历 && 日期拾取器

        [MenuItem("GameObject/UI/Calendar")]
        public static void AddCalendar( MenuCommand menuCommand )
        {
            GameObject calendar = SpringGUIDefaultControls.CreateCalendar(GetStandardResources());
            PlaceUIElementRoot(calendar,menuCommand);
            calendar.transform.localPosition = Vector3.zero;
        }

        [MenuItem("GameObject/UI/DatePicker")]
        public static void AddDatePicker( MenuCommand menuCommand )
        {
            GameObject datePicker = SpringGUIDefaultControls.CreateDatePicker(GetStandardResources());
            PlaceUIElementRoot(datePicker,menuCommand);
            datePicker.transform.localPosition = Vector3.zero;
        }

        #endregion
    }
}