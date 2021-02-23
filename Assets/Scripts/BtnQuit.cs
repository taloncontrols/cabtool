using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BtnQuit : MonoBehaviour
{
    // Start is called before the first frame update
    public Button m_ButtonQuit;
    void Start()
    {
        m_ButtonQuit.onClick.AddListener(doExitGame);

    }
    public void doExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      //Application.Quit();
      if (!Application.isEditor) { System.Diagnostics.Process.GetCurrentProcess().Kill(); }
#endif
    }

}
