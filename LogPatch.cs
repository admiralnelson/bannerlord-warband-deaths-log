using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.KillFeed.Personal;

namespace WarbandCasualtyLog
{
    internal class LogPatch
    {

        public static Color CYAN;
        public static Color ORANGE;
        public static Color RED;
        public static Color PURPLE;
        public static Color LIGHT_PURPLE;

        /**
         * Cancel main method call by returning false, don't want the normal log to display.
         * In future would be best to add a separate option from "Report Casualties", but haven't quite figured that out yet.
         */
        [HarmonyPatch(typeof(SPKillFeedVM), "OnAgentRemoved")]
        public class OnAgentRemovedPatch
        {
            public static bool Prefix(ref Agent affectedAgent, ref Agent affectorAgent, bool isHeadshot)
            {
                var builder = new StringBuilder();
                builder.Append(affectedAgent.Name);

                builder.Append(affectedAgent.State == AgentState.Unconscious ? " knocked unconscious by " : " killed by ");

                builder.Append(affectorAgent.Name);
                InformationManager.DisplayMessage(new InformationMessage(builder.ToString(), GetColor(affectedAgent, affectedAgent.State == AgentState.Unconscious)));
                return false;
            }
        }

        [HarmonyPatch(typeof(SPKillFeedVM), "OnPersonalKill")]
        public class OnPersonalKillPatch
        {
            public static bool Prefix(int damageAmount, bool isMountDamage, bool isFriendlyFire, bool isHeadshot, string killedAgentName, bool isUnconscious)
            {
                var builder = new StringBuilder();

                builder.Append("Delivered damage ");
                builder.Append(damageAmount);

                InformationManager.DisplayMessage(new InformationMessage(builder.ToString()));
                return false;
            }
        }

        [HarmonyPatch(typeof(SPKillFeedVM), "OnPersonalDamage")]
        public class OnPersonalDamagePatch
        {
            public static bool Prefix(int totalDamage, bool isVictimAgentMount, bool isFriendlyFire, string victimAgentName)
            {
                var builder = new StringBuilder();

                builder.Append("Delivered damage ");
                builder.Append(totalDamage);

                InformationManager.DisplayMessage(new InformationMessage(builder.ToString()));
                return false;
            }
        }

        public static void LoadConfigValues()
        {
            CYAN = Color.ConvertStringToColor(WarbandCasualtyLog.SubModule.DefaultFriendlyKill);
            ORANGE = Color.ConvertStringToColor(WarbandCasualtyLog.SubModule.DefaultFriendlyUnconscious);
            RED = Color.ConvertStringToColor(WarbandCasualtyLog.SubModule.DefaultFriendlyKilled);
            PURPLE = Color.ConvertStringToColor(WarbandCasualtyLog.SubModule.DefaultAllyKilled);
            LIGHT_PURPLE = Color.ConvertStringToColor(WarbandCasualtyLog.SubModule.DefaultAllyUnconscious);
        }

        private static Color GetColor(Agent killed, bool isUnconscious)
        {
            
            if (killed.Team != null)
            {
                if (killed.Team.IsPlayerTeam)
                {
                    return isUnconscious ? ORANGE : RED;
                }
                else if (killed.Team.IsPlayerAlly)
                {
                    return isUnconscious ? LIGHT_PURPLE : PURPLE;
                }

                return CYAN;
            }
            else
            {
                InformationManager.DisplayMessage(new InformationMessage("ERROR: Warband log exception!"));
                return Color.FromUint(4294967295u);
            }
        }
    }
}
