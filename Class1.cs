using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace Agniratha
{
    public class AgnirathaModule : LevelModule
    {
        AudioSource battleMusic;
        AudioSource uniqueBattleMusic;
        AudioSource dayMusic;
        AudioSource nightMusic;
        bool isInCombat;
        public override IEnumerator OnLoadCoroutine()
        {
            battleMusic = level.customReferences.Find(match => match.name == "Music").transforms.Find(match => match.name == "BattleMusic").GetComponent<AudioSource>();
            uniqueBattleMusic = level.customReferences.Find(match => match.name == "Music").transforms.Find(match => match.name == "UniqueBattleMusic").GetComponent<AudioSource>();
            dayMusic = level.customReferences.Find(match => match.name == "Music").transforms.Find(match => match.name == "DayMusic").GetComponent<AudioSource>();
            nightMusic = level.customReferences.Find(match => match.name == "Music").transforms.Find(match => match.name == "NightMusic").GetComponent<AudioSource>();
            return base.OnLoadCoroutine();
        }
        public override void Update()
        {
            base.Update();
            isInCombat = false;
            foreach (Creature creature in Creature.allActive)
            {
                if (creature?.brain?.currentTarget != null && !creature.isPlayer && !creature.isKilled && (creature.brain.currentTarget.isPlayer || creature.brain.currentTarget.faction == Player.local.creature.faction))
                {
                    isInCombat = true;
                    if (battleMusic.isPlaying) return;
                    battleMusic.Play();
                    dayMusic.Pause();
                    nightMusic.Pause();
                }
            }
            if (isInCombat || !battleMusic.isPlaying) return;
            uniqueBattleMusic.Stop();
            battleMusic.Stop();
            battleMusic.mute = false;
            dayMusic.UnPause();
            nightMusic.UnPause();
        }
    }
}