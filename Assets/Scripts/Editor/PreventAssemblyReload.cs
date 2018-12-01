using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PreventAssemblyReload {
    static PreventAssemblyReload() {
        EditorApplication.playModeStateChanged += PlayModeChanged;
    }

    private static void PlayModeChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.EnteredPlayMode) {
            EditorApplication.LockReloadAssemblies();
        } else if (state == PlayModeStateChange.ExitingPlayMode) {
            EditorApplication.UnlockReloadAssemblies();
        }
    }
}