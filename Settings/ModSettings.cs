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
    using static Game.UI.MapMetadataSystem;




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
        private bool _cs2original;

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
        public bool CitiesSkylines2Original
        {
            get { return _cs2original; }
            set
            {
                if (value)
                {
                    // If ResidentEvil is enabled, disable CitiesSkylines1Original
                    CitiesSkylines1Original = false;
                    ResidentEvil = false;
                    SimCity = false;
                }
                _cs2original = value;
            }
        }



        /// <summary>
        /// Button to open the StreamingAssets location.
        /// </summary>
        [SettingsUISection("MainTab")]
        [SettingsUIButton]
        public bool OpenLocationButton
        {
            set
            {
                OpenLocation();
            }
        }

        /// <summary>
        /// Opens the StreamingAssets location.
        /// </summary>
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

        /// <summary>
        /// String to set the current theme to use.
        /// </summary>
        public string Current { get; set; }

        /// <summary>
        /// Apply settings to switch Current string.
        /// </summary>
        public override void Apply()
        {
            base.Apply();
            if (CitiesSkylines1Original)
            {
                Current = "CitiesSkylines1.ogg";
            }
            if (ResidentEvil)
            {
                Current = "ResidentEvil.ogg";
            }
            if (SimCity)
            {
                Current = "SimCity.ogg";
            }
            if (CitiesSkylines2Original)
                Current = "CitiesSkylines2OST.ogg";

            CopyOver();

        }

        /// <summary>
        /// Copies over the selected theme to the StreamingAssets folder replacing the other one.
        /// </summary>
        public void CopyOver()
        {
            string current = Current;
            string resourceName = "MainMenuThemeReplacer.Themes." + current;
            string destinationPath = @"C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\StreamingAssets\Audio~\";

            // Ensure the destination folder exists
            string destinationFolder = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Get the name of the file without the path
            string destinationFileName = Path.GetFileName(current);
            string existingFilePath = Path.Combine(destinationPath, destinationFileName);

            // Check if a file with the same name already exists at the destination
            if (File.Exists(existingFilePath))
            {
                // Check if the existing file name is different from the one that is being copied.
                if (!string.Equals(existingFilePath, Path.Combine(destinationPath, destinationFileName), StringComparison.OrdinalIgnoreCase))
                {
                    // Delete the existing file
                    File.Delete(existingFilePath);
                    UnityEngine.Debug.Log("Existing file deleted at: " + existingFilePath);
                }
                else
                {
                    UnityEngine.Debug.Log("File with the same name already exists. Aborting operation.");
                    return; // Abort the operation
                }
            }

            // Get the assembly where the resource is embedded
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Copy the embedded resource to the destination path
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream != null)
                {
                    using (FileStream fileStream = File.Create(Path.Combine(destinationPath, destinationFileName)))
                    {
                        // Copy the contents of the resourceStream to fileStream
                        resourceStream.CopyTo(fileStream);
                    }

                    UnityEngine.Debug.Log("Resource copied successfully to: " + destinationPath + ". Please restart the game!");
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

      




     