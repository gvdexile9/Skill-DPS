﻿using System;
using System.Collections.Generic;
using PoeHUD.Models.Enums;
using PoeHUD.Plugins;
using PoeHUD.Poe;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.RemoteMemoryObjects;

namespace Skill_DPS.Skill_Data
{
    public class SkillBar
    {
        public static List<ushort> CurrentIDS() => BasePlugin.API.GameController.Game.IngameState.ServerData.SkillBarIds;

        public static List<Data> CurrentSkills()
        {
            List<Data> ReturnSkills = new List<Data>();
            try
            {
                List<ushort> ids = CurrentIDS();
                if (ids == null) return ReturnSkills;
                if (ids.Count > 100)
                {
                    BasePlugin.API.LogError("CurrentIDS.Count > 500", 10);
                    return ReturnSkills;
                }
                //BasePlugin.API.LogError($"ids Count: {ids.Count}", 10);

                for (int index = 0; index < ids.Count; index++)
                {
                    if (GetSkill(ids[index]) == null) continue;

                    ActorSkill Skill = GetSkill(ids[index]);

                    ReturnSkills.Add(new Data
                    {
                            Skill = Skill,
                            SkillStats = Skill.Stats,
                            SkillElement = BasePlugin.API.GameController.Game.IngameState.IngameUi.SkillBar.Children[index]
                    });
                }
            }
            catch (Exception e)
            {
                BasePlugin.API.LogError(e, 10);
            }

            return ReturnSkills;
        }

        public static ActorSkill GetSkill(ushort ID)
        {
            try
            {
                List<ActorSkill> ActorSkills = BasePlugin.API.GameController.Player.GetComponent<Actor>().ActorSkills;
                if (ActorSkills != null)
                {
                    foreach (ActorSkill skill in ActorSkills)
                    {
                        if (skill.Id == ID) return skill;
                    }
                }
            }
            catch (Exception e)
            {
                BasePlugin.API.LogError(e, 10);
            }

            return null;
        }

        public static Dictionary<GameStat, int> GetSkillStats(ActorSkill skill)
        {
            return skill.Stats;
        }

        public class Data
        {
            public ActorSkill Skill { get; set; }
            public Dictionary<GameStat, int> SkillStats { get; set; }
            public Element SkillElement { get; set; }
        }
    }
}