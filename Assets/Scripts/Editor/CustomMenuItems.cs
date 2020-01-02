using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CustomMenuItems : MonoBehaviour
{
	[MenuItem("TorusShooter/Clear PlayerPrefs")]
	private static void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	[MenuItem("TorusShooter/Reload Current Scene")]
	public static void ReloadCurrentScene()
	{
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			EditorSceneManager.OpenScene(EditorSceneManager.GetActiveScene().path);
	}
}
