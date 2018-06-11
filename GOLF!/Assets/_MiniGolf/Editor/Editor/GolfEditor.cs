using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
public class GolfEditor : EditorWindow {
	private static GolfEditor m_window;
	private  GameObject[] m_tileObjects = new GameObject[25];
	private  GameObject m_ballObject;
	private  GameObject m_childObject;

	public   Texture[] m_textures;
	public Texture m_ballTexture;
	public string[] m_holes = new string[18];
	private int m_selectedHole = 0;
	private int m_prevObjects = -1;
	private GameObject m_parentObject = null;
	[MenuItem ("Window/Golf Editor")]
	static void Init () 
	{
		// Get existing open window or if none, make a new one:
		m_window = (GolfEditor)EditorWindow.GetWindow (typeof (GolfEditor),true);
		if(m_window)
		{
			m_window.init();
		}
		//createScene();
	}
	public void OnDidOpenScene()
	{
		Debug.Log("OnDidOpenScene");
		init();
		Focus();

	}
	public void init()
	{

		for(int i=0; i<m_tileObjects.Length; i++)
		{
			m_tileObjects[i] = Resources.Load("miniGolf/Tile"+(i+1).ToString("00")) as GameObject;
		}
		updateTextures();

		m_ballObject = Resources.Load("miniGolf/Ball") as GameObject;
		m_ballTexture = AssetPreview.GetAssetPreview(m_ballObject);
		for(int i=0; i<18; i++)
		{
			m_holes[i] = "Hole " + (i+1);
			//m_par[i] = 3;
		}


		for(int i=0; i<18; i++)
		{
			GameObject go = GameObject.Find("Hole" + i);
			if(go)
			{
				m_selectedHole = i;
				m_prevObjects = i;
				m_parentObject = go;
				//m_prefab = loadPrefab(m_selectedHole);
			}
		}

		updateHoles();
		Repaint();
	}
	private bool m_tileBool = true;
	private bool m_holeBool = true;

	public delegate void MyFunction(Texture[] textures,int n);


	//public string[] courses = {"Course Alpha","Course Beta","Course Gamma","Course Delta"};
	public int m_courseIndex = 0;
	private bool m_playing = false;
	void Update () {
		if(EditorApplication.isPlaying)
		{
			if(m_playing==false)
			{
				m_playing=true;
				this.Close();
			}
			return;
		}else{
			if(m_playing)
			{
				init ();
				//	repaint();
				m_playing=false;
			}
		}
	}
	void createNewScenes()
	{
		string oringal = EditorApplication.currentScene;
		string[] path = EditorApplication.currentScene.Split('.');
		string dir = getDir();
		Material mat = RenderSettings.skybox;

		for(int i=0; i<18; i++)
		{
			Directory.CreateDirectory(path[0]);
			
			Debug.Log("path"+dir);
			string sceneName = path[0] + "/Hole"+ i.ToString("00") + ".unity";
			string prefabName = dir + "/Hole" + i + ".prefab";
			EditorApplication.NewScene();
			ParScript ps = (ParScript)GameObject.FindObjectOfType(typeof(ParScript));
			if(ps)
			{
			//	ps.par = m_par[i];
			}
			DestroyImmediate(Camera.main.gameObject);
			GameObject newObject = new GameObject();
			Light light0 = newObject.AddComponent<Light>();
			newObject.transform.rotation = Quaternion.Euler(45,45,0);
			if(light0)
			{
				light0.type = LightType.Directional;
			}
			Instantiate(Resources.Load("MiniGolf/StartUp"),Vector3.zero,Quaternion.identity);

			float precent = i / 17f;
			EditorUtility.DisplayProgressBar("Creating Scenes","Shows progress",precent);

			GameObject go1 = (GameObject)AssetDatabase.LoadAssetAtPath(prefabName, typeof(GameObject));
			RenderSettings.skybox = mat;
			if(go1)
			{
				Instantiate(go1,Vector3.zero,Quaternion.identity);
			}
			EditorApplication.SaveScene(sceneName);
		}
		EditorUtility.ClearProgressBar();
		EditorApplication.OpenScene(oringal);
	}
	void setUpScene()
	{
		LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
		LightmapEditorSettings.realtimeResolution  = 15;		
		
		Light l0 = (Light)GameObject.FindObjectOfType(typeof(Light));
		if(l0)
		{
			l0.intensity = 1f;
			l0.shadows = LightShadows.Hard;
		}else{
			Debug.LogError("NO LIGHT");
		}
		
	}
	private string[] scenesToBake;
	void bakeAllScenes()
	{
		scenesToBake = ReadNames();
		//m_baking = true;
		for(int i=1; i<scenesToBake.Length; i++)
		{
			EditorApplication.OpenScene(scenesToBake[i]);
			setUpScene();
			Lightmapping.Clear ();
			Lightmapping.Bake ();
			EditorApplication.SaveScene(scenesToBake[i]);
		}
	}

	private int m_par = 3;
	private ParScript m_parScript;
	void OnGUI()
	{

		int n = 0;
		Object obj  = null;
		Rect r = new Rect(0.00f,0,.9f,.4f);
		float rx = r.x;
		int nomPerRow = 4;
		int nomPerCol = 6;
		r.x=0;
		r.width=1f;
		r.height = 0.05f;

		bool badTextures = false;
		r.x=0;
		//r.y+= r.height;
		r.width=.2f;
		r.height = 0.05f;
		
		//m_holeBool = EditorGUI.Foldout(GUIHelper.screenRect(r),m_holeBool,"Holes");
		//r.y+=r.height;


		if(m_holeBool)
		{
			r.width = .2f;
			r.height = 0.2f;
			GUI.Box(GUIHelper.screenRect(r),m_holeTextures[m_selectedHole]);

			r.x+=r.width;
			r.width=.2f;
			r.height = 0.05f;

			m_selectedHole = EditorGUI.Popup(GUIHelper.screenRect(r),m_selectedHole, m_holes);

			r.x+=0.2f;
			r.width=0.05f;
			GUI.Label(GUIHelper.screenRect(r),"Par");
			r.x+=0.05f;
			r.width=0.1f;

			Rect tmp = GUIHelper.screenRect(r);
			tmp.height = 20;
			m_par = EditorGUI.IntField(tmp,"",m_par);
			if(m_parScript)
			{
				m_parScript.par = m_par;
			}
			r.x+=r.width;
			r.width=0.2f;

			//r.y+=r.height;

		}
		//r.x=0;


		r.y+=.2f;
		r.x=0;
		r.width = .2f;
		r.height = 0.05f;
		m_tileBool = EditorGUI.Foldout(GUIHelper.screenRect(r),m_tileBool,"Tiles");


		if(m_tileBool)
		{
			r.x=0f;
			r.width = 1f/6f;
			r.height = 0.35f/4f; 
			r.y +=0.05f;
			buttons(4,6,r,m_textures,createObjectN,false);
			r.y+=r.height*4f;
			if(m_textures!=null)
			{
				if(GUI.Button(GUIHelper.screenRect(r),m_textures[24]))
				{
					createObject(m_tileObjects[24]);
				}
			}
			r.x+=r.width;

			if(GUI.Button(GUIHelper.screenRect(r),m_ballTexture))
			{
				GameObject newObject = createObject(m_ballObject,true);
				newObject.name = "Ball";
			}

			r.x=0;
			r.width*=2f;
			r.y+=r.height;
		}
		if(GUI.Button(GUIHelper.screenRect(r),"Update Hole Prefab"))
		{
			updateHole ();
		}
		r.x+=r.width;
		if(GUI.Button(GUIHelper.screenRect(r),"Create Scenes"))
		{
			createNewScenes();
			
		}
		r.x+=r.width;
		if(GUI.Button(GUIHelper.screenRect(r),"Bake All Scenes!"))
		{
			bakeAllScenes();
			
		}
		if(m_prevObjects!=m_selectedHole)
		{
			changeHole();
		}
		if(m_parentObject)
		{
			if(SceneView.lastActiveSceneView!=null)
			{
				Camera sceneCamera = SceneView.lastActiveSceneView.camera;
				if(sceneCamera)
				{
					sceneCamera.transform.LookAt(m_parentObject.transform);
				}
			}
		}
		r.x+=r.width;

		r.x=0;
		r.y+=r.height;
		r.height=0.15f;
		updateHoles();
	}
	private static string[] ReadNames()
	{
		List<string> temp = new List<string>();
		foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
		{
			if (S.enabled)
			{
				string name = S.path.Substring(S.path.LastIndexOf('/')+1);
				name = name.Substring(0,name.Length-6);
				temp.Add(S.path);
				
				Debug.Log (EditorApplication.applicationPath + "/" + S.path);
			}
		}
		return temp.ToArray();
	}	
	public void updateHole()
	{
		GameObject holeGo = GameObject.FindWithTag("Hole");
		GameObject ballGo = GameObject.Find("Ball");
		if(ballGo && holeGo)
		{
			string errorMessage = null;
			if(holeGo && holeGo.transform.position.y <= 0)
			{
				errorMessage += "Hole not above 0 which means its probably below the water line!";
			}
			
			if(ballGo && ballGo.transform.position.y <= 0)
			{
				errorMessage += "Ball not above 0 which means its probably below the water line!";
			}
			if(errorMessage!=null)
			{
				EditorUtility.DisplayDialog("Golf Warning",errorMessage,"Okay");
			}
			doRaycastDownOnObject("Ball");
			createPrefab(m_selectedHole);
			init ();
		}else{
			string errorMessage = "Missing:";
			if(holeGo==null)
			{
				errorMessage += " hole tile";
			}
			if(ballGo==null)
			{
				errorMessage += " ball gameObject";
			}
			
			EditorUtility.DisplayDialog("Golf Error",errorMessage,"Okay");
		}
	}
	public void changeHole()
	{
		m_prefab = loadPrefab(m_prevObjects);
		ParScript ps = (ParScript)GameObject.FindObjectOfType(typeof(ParScript));
		if(ps)
		{
			DestroyImmediate( ps.gameObject);

		}
		if(m_parentObject)
		{
			DestroyImmediate( m_parentObject);
		}
		m_parentObject = GameObject.Find("Hole" + m_selectedHole);
		GameObject g0 = loadPrefab(m_selectedHole);
		
		if(g0==null)
		{
			if(m_childObject)
			{
				m_childObject.SetActive(true);
			}
			m_parentObject = new GameObject();
			m_parScript = m_parentObject.AddComponent<ParScript>();
			m_parentObject.name = "Hole" + m_selectedHole;
		}else{
			GameObject go = (GameObject)Instantiate(g0,Vector3.zero,Quaternion.identity);
			if(go)
			{
				go.name = "Hole" + m_selectedHole;
				
				
			}
			m_parScript = (ParScript)GameObject.FindObjectOfType(typeof(ParScript));
			m_parentObject = go;
			Selection.activeGameObject = (GameObject)m_parentObject;
			m_prevObjects = m_selectedHole;
			
		}
		if(m_parScript)
		{
			m_par = m_parScript.par;
		}
		Selection.activeGameObject = (GameObject)m_parentObject;
		
		init();
	}
	private Texture2D[] m_holeTextures;
	public void updateHoles()
	{
		m_holeTextures = new Texture2D[18];
		for(int i=0; i<m_holeTextures.Length; i++)
		{
			m_holeTextures[i] = AssetPreview.GetAssetPreview(loadPrefab(i));
		}
	}
	public GUIContent[] getContent()
	{
		updateHoles();
		GUIContent[] content = new GUIContent[18];
		for(int i=0; i<content.Length; i++)
		{
			content[i] = new GUIContent(m_holeTextures[i]);
		}
		return content;
	}
	private Object m_prefab;
	public string getDir()
	{
		string[] path= EditorApplication.currentScene.Split(char.Parse("/"));
		path[0] = path[path.Length-1];
		string[] str = path[0].Split('.');

		Debug.Log("currentDir" + EditorApplication.currentScene);
		return  "Assets/MiniGolf/Holes/" + str[0];
	}
	public string getPath(int i, bool createDir)
	{
		string dir = getDir ();
		if(createDir)
		{
			Directory.CreateDirectory(dir);
		}
		return dir  + "/Hole" +  i + ".prefab";
	}
	public GameObject loadPrefab(int i)
	{
		GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(getPath(i,false), typeof(GameObject));
		return go;
	}
	public void createPrefab(int index)
	{
		Debug.Log("createPrefab" + index);
		Object prefab = create(getPath(index,true),m_parentObject);
		m_prefab = prefab;
		//m_chunkManager.replaceChunk(m_selectedIndex,(GameObject)prefab);
		//DestroyImmediate(m_parentObject);
	}
	public Object create(string localPath,GameObject obj) {
		Debug.Log("create" + localPath);
		Object prefab = PrefabUtility.CreateEmptyPrefab(localPath);
		prefab = PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
		return prefab;
	}

	void doRaycastDownOnObject(string objectName)
	{
		GameObject go = GameObject.Find(objectName);
		if(go)
		{
			Vector3 pos = go.transform.position;
			go.transform.position = new Vector3(111111,111111,111111);
			pos.y= 10000f;

			RaycastHit hit;
			if(Physics.Raycast(pos,Vector3.down,out hit))
			{
				pos.y = hit.point.y+1f;
			}else{
				pos.y = 0;
			}
			go.transform.position = pos;
		}
	}
	public GameObject createObject(Object obj, bool raycastDown=false)
	{
		GameObject newGameObject = null;
		if(obj!=null)
		{
			Camera sceneCamera = SceneView.lastActiveSceneView.camera;
			Vector3 cameraPos = sceneCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(sceneCamera.pixelWidth/2f, sceneCamera.pixelHeight/2f, 0f));
			cameraPos.y = 0;

			if(raycastDown)
			{
				cameraPos.y= 10000f;
				RaycastHit hit;
				if(Physics.Raycast(cameraPos,Vector3.down,out hit))
				{
					cameraPos.y = hit.point.y+2f;
				}else{
					cameraPos.y = 0;
				}
			}


			Object newObj = Instantiate(obj,cameraPos,Quaternion.identity);
			if(newObj)
			{
				newGameObject = (GameObject)newObj;
				if(m_parentObject)
				{
					newGameObject.transform.parent = m_parentObject.transform;
				}
				Selection.activeGameObject = (GameObject)newObj;
			}
		}
		return newGameObject;
	}

	public  void updateTextures()
	{
		m_textures = new Texture2D[m_tileObjects.Length];
		for(int i=0; i<m_tileObjects.Length; i++)
		{
			m_textures[i] = AssetPreview.GetAssetPreview(m_tileObjects[i]);
		}
	}
	public bool buttons(int rows, int cols,Rect r, Texture[] textures,MyFunction func, bool requireParField)
	{
		float rx = r.x;
		int n = 0;
		bool badTextures = false;
		//r.width = 1f / (float)bx;
		//r.height = 0.4f / (float)by; 
		
		//	Debug.Log("buttons" + r);
		float rh = r.height;
		for(int i=0;i<rows; i++)
		{
			for(int j=0; j<cols; j++)
			{
				if(textures!=null)
				{
					float r0 = r.height;
					if(requireParField)
					{
						r.height = rh - 0.05f;
					}
					if(GUI.Button(GUIHelper.screenRect(r),textures[n]))
					{
						func(textures,n);
					}
					
					if(requireParField)
					{
						Rect r1 = r;
						r1.y += r.height;
						r1.width *= 0.5f;
						GUI.Label(GUIHelper.screenRect(r1),"Hole: " + (n+1) + " Par");
						r1.x += r1.width;
						r1.height = 0.05f; 
					//	m_par[n] = EditorGUI.IntField(GUIHelper.screenRect(r1),m_par[n]);
					}
					
					
				}else{
					
					badTextures=true;
					
				}
				n++;
				r.x+=r.width;
			}
			r.x = rx;
			float offset = 0f;
			if(requireParField)offset=0.05f;
			
			r.y+= r.height+offset;
		}
		return badTextures;
	}
	public void createObjectN(Texture[] textures,int n)
	{
		createObject(m_tileObjects[n]);
	}
	public void selectHoleN(Texture[] textures,int n)
	{
		m_selectedHole = n;
	}
}
