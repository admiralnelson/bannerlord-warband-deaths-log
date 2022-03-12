using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;

namespace WarbandCasualtyLog
{
    public class SubModule : MBSubModuleBase
    {
        public static readonly string DefaultFriendlyKill = "#9AD7B4FF";
        public static readonly string DefaultFriendlyKilled = "#AF6353FF";
        public static readonly string DefaultFriendlyUnconscious = "#FFA862FF";
        public static readonly string DefaultAllyUnconscious = "#EEA4FFFF";
        public static readonly string DefaultAllyKilled = "#F11CB5FF";
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            var harmony = new Harmony("org.calradia.admiralnelson.warbandkillfeed");
            harmony.PatchAll();
            LogPatch.LoadConfigValues();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {

        }

    }

    
}
