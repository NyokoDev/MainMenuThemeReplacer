namespace MainMenuThemeReplacer
{

    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.Tools;
    using UnityEngine.Rendering.HighDefinition;
    using UnityEngine.Rendering;
    using MainMenuThemeReplacer;
    using System;
    using Game.Settings;

    public sealed class Mod : IMod
    {
        /// <summary>
        /// Mod properties.
        /// </summary>
        public const string ModName = "MainMenuThemeReplacer";

        public static Mod Instance { get; private set; }
        internal ILog Log { get; private set; }
        public void OnLoad()
        {
            Instance = this;
            Log = LogManager.GetLogger(ModName);
            Log.Info("setting logging level to Debug");
            Log.effectivenessLevel = Level.Debug;
            Log.Info("loading");
        }

        /// <summary>
        /// Gets the mod's active settings configuration.
        /// </summary>
        internal ModSettings ActiveSettings { get; private set; }

        /// <summary>
        /// Called by the game when the game world is created. 
        /// </summary>
        /// <param name="updateSystem">Game update system.</param>
        public void OnCreateWorld(UpdateSystem updateSystem)
        {
            

            ActiveSettings = new(this);
            ActiveSettings.RegisterInOptionsUI();
            Localization.LoadTranslations(ActiveSettings, Log);

           
        }
        /// <summary>
        /// Called by the game when the mod is disposed of.
        /// </summary>
        public void OnDispose()
        {
            Log.Info("disposing");
            Instance = null;
        }
    }
}