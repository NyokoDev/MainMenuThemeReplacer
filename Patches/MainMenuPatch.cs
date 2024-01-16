using Colossal.IO.AssetDatabase;
using Game.Audio;
using Game.Objects;
using Game.SceneFlow;
using Game.Simulation;
using HarmonyLib;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using static Colossal.AssetPipeline.Diagnostic.Report;

[HarmonyPatch(typeof(GameManager))]
[HarmonyPatch("MainMenu")]
public static class AudioManager_PlayMenuMusic_Patch
{
    private static int m_PlayCount;
    static readonly AudioMixer m_Mixer;
    static readonly AudioMixerGroup m_MenuGroup;
    private static AudioLoop m_MainMenuMusic;


    public static bool Prefix()
    {
        MainMenu();
        AudioManager.instance.PlayMenuMusic("New Menu Theme");
        UnityEngine.Debug.Log("PlayMenuMusic patched.");
        return false;
    }
    public static bool MainMenu()
    {
        IAssetData asset = null;
        GameManager.instance.Load(Game.GameMode.MainMenu, Colossal.Serialization.Entities.Purpose.Cleanup, asset); GameManager.instance.WaitForReadyState();
        return true;
    }
}

    