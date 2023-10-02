using System.IO;
using FLZ.Audio;
using UnityEditor;
using UnityEngine;

public class SoundCreator
{
    [MenuItem("Assets/Create/Audio/SFX")]
    private static void CreateSFX()
    {
        var selection = Selection.objects;

        foreach (var obj in selection)
        {
            SFX.Sound[] sounds = new SFX.Sound[1];
            sounds[0] = new SFX.Sound(obj as AudioClip);

            SFX newSFX = new SFX(sounds);

            string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(obj));
            string name = "/SFX_" + obj.name + ".asset";
            AssetDatabase.CreateAsset(newSFX, path + name);
            
            Selection.activeObject = newSFX;
        }
    }
    
    [MenuItem("Assets/Create/Audio/SFX (single file) %t")]
    private static void CreateSFXOneFile()
    {
        var selection = Selection.objects;

        SFX newSFX = null;
        SFX.Sound[] sounds = new SFX.Sound[selection.Length];

        for (var i = 0; i < selection.Length; i++)
        {
            var obj = selection[i];
            sounds[i] = new SFX.Sound(obj as AudioClip);
            sounds[i].Volume = 1;
            sounds[i].Pitch = 1;
        }
            
        newSFX = new SFX(sounds);
        string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selection[0]));
        string name = "/" + selection[0].name + ".asset";
        AssetDatabase.CreateAsset(newSFX, path + name);
    }
}
