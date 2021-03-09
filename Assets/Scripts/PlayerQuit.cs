using UnityEngine;

public class PlayerQuit
{
    static void Quit()
    {
        Debug.Log("Quitting the Player");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      //Application.Quit();
      if (!Application.isEditor) { System.Diagnostics.Process.GetCurrentProcess().Kill(); }
#endif
    }

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        Application.quitting += Quit;
    }
}