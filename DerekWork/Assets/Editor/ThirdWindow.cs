	// C# example
	// Simple script that lets you render the main camera in an editor Window.
	
	using UnityEngine;
	using UnityEditor;
	
	public class ThirdWindow : EditorWindow {
	static Camera camera;
		RenderTexture renderTexture;
	
		[MenuItem("Example/Camera viewer")]
		static void Init() {
			for (int i=0; i<Camera.allCamerasCount; ++i) {
				if (Camera.allCameras[i].name == "CameraThird") {
					camera = Camera.allCameras[i]; 
					break;
				}
			}
			if (!camera) {
				Debug.LogError("Could not find CameraThird");
			}
			EditorWindow editorWindow = GetWindow(typeof(ThirdWindow));
			editorWindow.autoRepaintOnSceneChange = true;
			editorWindow.Show();
		}
		public void Awake () {
			renderTexture = new RenderTexture((int)position.width, 
						(int)position.height, 
						(int)RenderTextureFormat.ARGB32 );
		}
		public void Update() {
			if(camera != null) {
				camera.targetTexture = renderTexture;
				camera.Render();
				//camera.targetTexture = null;	
			}
			try {
				if(renderTexture.width != position.width || 
					renderTexture.height != position.height)
					renderTexture = new RenderTexture((int)position.width, 
								(int)position.height, 
								(int)RenderTextureFormat.ARGB32 );
			} catch (MissingReferenceException e) {
				/*...*/
			}
		}
		void OnGUI() {
			GUI.DrawTexture( new Rect( 0.0f, 0.0f, position.width, position.height), renderTexture );	
		}
	}