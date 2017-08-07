
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
            if ( parent == null || parent.GetComponentInParent<Canvas>() == null )
                parent = GetOrCreateCanvasGameObject();
            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform , element.name);
            element.name = uniqueName;
            Undo.RegisterCreatedObjectUndo(element , "Create " + element.name);
            Undo.SetTransformParent(element.transform , parent.transform , "Parent " + element.name);
            GameObjectUtility.SetParentAndAlign(element , parent);
            if ( parent != menuCommand.context ) // not a context click, so center in sceneview
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>() , element.GetComponent<RectTransform>());
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

        #region Create Functional Formula Graph / Pie Graph

        [MenuItem("GameObject/UI/SpringGUI/Data Graph/Functional Formula Graph")]
        public static void AddFunctionalGraph( MenuCommand menuCommand )
        {
            GameObject functionalGraph = SpringGUIDefaultControls.CreateFunctionalGraph(GetStandardResources());
            PlaceUIElementRoot(functionalGraph,menuCommand);
            functionalGraph.transform.localPosition = Vector3.zero;
            functionalGraph.AddComponent<FunctionalGraph>();
        }

        [MenuItem("GameObject/UI/SpringGUI/Data Graph/Pie Graph")]
        public static void AddPieGraph( MenuCommand menuCommand )
        {
            GameObject pieGraph = SpringGUIDefaultControls.CreatePieGraph(GetStandardResources());
            PlaceUIElementRoot(pieGraph , menuCommand);
            pieGraph.transform.localPosition = Vector3.zero;
            pieGraph.AddComponent<PieGraph>();
        }

        #endregion

        #region UITree/TreeView

        [MenuItem("GameObject/UI/SpringGUI/Tree/UITree",false,2063)]
        public static void AddUITree( MenuCommand menuCommand ) 
        {
            GameObject uiTree = SpringGUIDefaultControls.CreateUITree(GetStandardResources());
            PlaceUIElementRoot(uiTree,menuCommand);
        }

        [MenuItem("GameObject/UI/SpringGUI/Tree/UITreeNode" , false,2064)]
        public static void AddUITreeNode( MenuCommand menuCommand )
        {
            GameObject uiTreeNode = SpringGUIDefaultControls.CreateUITree(GetStandardResources());
            PlaceUIElementRoot(uiTreeNode,menuCommand);
        }

        #endregion

        #region DoubleClickButton/LongClickButton

        [MenuItem("GameObject/UI/SpringGUI/Buttons/DoubleClickButton" , false , 2065)]
        public static void AddDoubleClickButton( MenuCommand menuCommand )
        {
            GameObject dcButton = SpringGUIDefaultControls.CreateDoubleClickButton(GetStandardResources());
            PlaceUIElementRoot(dcButton,menuCommand);
        }

        [MenuItem("GameObject/UI/SpringGUI/Buttons/LongClickButton" , false , 2066)]
        public static void AddLongClickButton( MenuCommand menuCommand )
        {
            GameObject lcButton = SpringGUIDefaultControls.CreateLongClickButton(GetStandardResources());
            PlaceUIElementRoot(lcButton,menuCommand);
        }

        #endregion

        #region Calendar/DatePicker

        [MenuItem("GameObject/UI/SpringGUI/Calendar",false,2067)]
        public static void AddCalendar( MenuCommand menuCommand )
        {
            GameObject calendar = SpringGUIDefaultControls.CreateCalendar(GetStandardResources());
            PlaceUIElementRoot(calendar,menuCommand);
            calendar.transform.localPosition = Vector3.zero;
        }

        [MenuItem("GameObject/UI/SpringGUI/DatePicker" , false , 2068)]
        public static void AddDatePicker( MenuCommand menuCommand )
        {
            GameObject datePicker = SpringGUIDefaultControls.CreateDatePicker(GetStandardResources());
            PlaceUIElementRoot(datePicker,menuCommand);
            datePicker.transform.localPosition = Vector3.zero;
        }

        #endregion

        #region ColoredTape/ColorPicker

        [MenuItem("GameObject/UI/SpringGUI/ColoredTape/VerticalColoredTape" , false , 2069)]
        public static void AddVerticalColoredTape( MenuCommand menuCommand )
        {
            GameObject coloredTape = SpringGUIDefaultControls.CreataVerticalColoredTape(GetStandardResources());
            PlaceUIElementRoot(coloredTape , menuCommand);
            coloredTape.transform.localPosition = Vector3.zero;
            ColoredTape tape = coloredTape.GetComponent<ColoredTape>();
            tape.TapeDirection = ColoredTape.E_DrawDirection.Vertical;
        }

        [MenuItem("GameObject/UI/SpringGUI/ColoredTape/HorizontalColoredTape" , false , 2070)]
        public static void AddHorizontalColoredTape( MenuCommand menuCommand )
        {
            GameObject coloredTape = SpringGUIDefaultControls.CreataHorizontalColoredTape(GetStandardResources());
            PlaceUIElementRoot(coloredTape , menuCommand);
            coloredTape.transform.localPosition = Vector3.zero;
            ColoredTape tape = coloredTape.GetComponent<ColoredTape>();
            tape.TapeDirection = ColoredTape.E_DrawDirection.Horizontal;
        }

        [MenuItem("GameObject/UI/SpringGUI/ColorPicker" , false , 2071)]
        public static void AddColorPicker( MenuCommand menuCommand )
        {
            GameObject colorPicker = SpringGUIDefaultControls.CreateColorPicker(GetStandardResources());
            PlaceUIElementRoot(colorPicker,menuCommand);
            colorPicker.transform.localPosition = Vector3.zero;
            colorPicker.AddComponent<ColorPicker>();
        }

        #endregion

        #region LineChart/RadarMap

        [MenuItem("GameObject/UI/SpringGUI/Data Graph/LineChart")]
        public static void AddLineChartGraph( MenuCommand menuCommand )
        {
            GameObject lineGraph = SpringGUIDefaultControls.CreateLineChartGraph(GetStandardResources());
            PlaceUIElementRoot(lineGraph , menuCommand);
            lineGraph.transform.localPosition = Vector3.zero;
            lineGraph.AddComponent<LineChart>();
        }

        [MenuItem("GameObject/UI/SpringGUI/Data Graph/RadarMap")]
        public static void AddRadarMap( MenuCommand menuCommand )
        {
            GameObject radarMap = SpringGUIDefaultControls.CreateRadarMap(GetStandardResources());
            PlaceUIElementRoot(radarMap , menuCommand);
            radarMap.transform.localPosition = Vector3.zero;
            radarMap.AddComponent<RadarMap>();
        }

        [MenuItem("GameObject/UI/SpringGUI/Data Graph/BarChart")]
        public static void AddBarChartGraph( MenuCommand menuCommand )  
        {
            GameObject barChart = SpringGUIDefaultControls.CreateBarChart(GetStandardResources());
            PlaceUIElementRoot(barChart,menuCommand);
            barChart.transform.localPosition = Vector3.zero;
            barChart.AddComponent<BarChart>();
        }

        #endregion
    }
}