// <copyright file="ModSettings.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the Apache Licence, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace MainMenuThemeReplacer
{
    using System.Xml.Serialization;
    using Colossal.IO.AssetDatabase;
    using MainMenuThemeReplacer;
    using Game.Modding;
    using Game.Settings;
    using UnityEngine;
    using System;
    using Game.UI.Widgets;
    using System.Collections.Generic;

    using UnityEngine.Scripting;
    using Game.Rendering;
    using Unity.Entities;
    using Game.UI;
    using System.IO;
    using System.Reflection;
    using System.Diagnostics;





    /// <summary>
    /// The mod's settings.
    /// </summary>
    [FileLocation("nyoko.MainMenuThemeReplacer")]
    public class ModSettings : ModSetting
    {

        /// <summary>
        /// Boolean to call after settings load.
        /// </summary>
        public bool Loaded;

        /// Initializes a new instance of the <see cref="ModSettings"/> class.
        /// </summary>  
        /// <param name="mod"><see cref="IMod"/> instance.</param>
        public ModSettings(IMod mod)
            : base(mod)
        {
        }


        private bool _citiesSkylines1Original;
        private bool _residentEvil;
        private bool _simcity;

        [SettingsUISection("MainTab")]
        public bool CitiesSkylines1Original
        {
            get { return _citiesSkylines1Original; }
            set
            {
                if (value)
                {
                    // If CitiesSkylines1Original is enabled, disable ResidentEvil and other 
                    ResidentEvil = false;
                    SimCity = false;
                }
                _citiesSkylines1Original = value;
            }
        }

        [SettingsUISection("MainTab")]
        public bool ResidentEvil
        {
            get { return _residentEvil; }
            set
            {
                if (value)
                {
                    // If ResidentEvil is enabled, disable CitiesSkylines1Original
                    CitiesSkylines1Original = false;
                    SimCity = false;
                }
                _residentEvil = value;
            }
        }

        [SettingsUISection("MainTab")]
        public bool SimCity
        {
            get { return _simcity; }
            set
            {
                if (value)
                {
                    // If ResidentEvil is enabled, disable CitiesSkylines1Original
                    CitiesSkylines1Original = false;
                    ResidentEvil = false;
                }
                _simcity = value;
            }
        }



        [SettingsUISection("MainTab")]
        [SettingsUIButton]
        public bool OpenLocationButton
        {
            set
            {
                OpenLocation();
            }
        }

        private void OpenLocation()
        {
            string destinationPath = @"C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\StreamingAssets\Audio~\";

            try
            {
                Process.Start("explorer.exe", destinationPath);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur when trying to open the file explorer
                Console.WriteLine("Error opening File Explorer: " + ex.Message);
            }
        }

        public string Current { get; set; }

        public override void Apply()
        {
            base.Apply();
            if (CitiesSkylines1Original)
            {
                Current = "CitiesSkylines1.ogg";
            }
            if (ResidentEvil)
            {
                Current = "MainMenu.ogg";
            }
            if (SimCity)
            {
                Current = "SimCity.ogg";
            }

            CopyOver();
                        
        }



        public void CopyOver()
        {
            string current = Current;
            string resourceName = "MainMenuThemeReplacer.Themes." + current;
            string destinationPath = @"C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\StreamingAssets\Audio~\MainMenu.ogg";

            // Ensure the destination folder exists
            string destinationFolder = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Delete existing MainMenu.ogg if it exists
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
                UnityEngine.Debug.Log("Existing MainMenu.ogg deleted successfully.");
            }

            // Get the assembly where the resource is embedded
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Copy the embedded resource to the destination path
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream != null)
                {
                    using (FileStream fileStream = File.Create(destinationPath))
                    {
                        resourceStream.CopyTo(fileStream);
                    }

                    UnityEngine.Debug.Log("Resource copied successfully to: " + destinationPath + "Please restart the game!" );
                }
                else
                {
                    UnityEngine.Debug.Log("Resource not found: " + resourceName);
                }
            }
        }

        public override void SetDefaults()
        {
           
        }
    }
}
