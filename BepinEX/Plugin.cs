using BepInEx;
using Game.Vehicles;
using Game;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Game.Common;
using Game.PSI;
using Game.Rendering;
using Game.Rendering.CinematicCamera;
using Game.SceneFlow;
using Game.UI.InGame;
using HarmonyLib;
using static UnityEngine.MonoBehaviour;
using MainMenuThemeReplacer;

#if BEPINEX_V6
    using BepInEx.Unity.Mono;
#endif

namespace MainMenuMusicReplacer
{
    [BepInPlugin(GUID, "MainMenuMusicReplacer", "1.0")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "com.nyoko.mainmenumusicreplacer";

        private Mod _mod;
 

        public void Awake()
        {

            _mod = new();
            _mod.OnLoad();

            // Apply Harmony patches.
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
        }

        /// <summary>
        /// Harmony postfix to <see cref="SystemOrder.Initialize"/> to substitute for IMod.OnCreateWorld.
        /// </summary>
        /// <param name="updateSystem"><see cref="GameManager"/> <see cref="UpdateSystem"/> instance.</param>
        [HarmonyPatch(typeof(SystemOrder), nameof(SystemOrder.Initialize))]
        [HarmonyPostfix]
        private static void InjectSystems(UpdateSystem updateSystem) => Mod.Instance.OnCreateWorld(updateSystem);

    }
}



// Code authored by Nyoko
// Feel free to reach out for any questions or clarifications!
// Huge thanks to Algernon for input and assistance on mod structure plus photo mode render system prefix implementation.
