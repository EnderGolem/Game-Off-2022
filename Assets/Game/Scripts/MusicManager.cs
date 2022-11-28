using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class MusicManager : MMSingleton<MusicManager>,MMEventListener<MMGameEvent>
{
   [SerializeField]
   protected AudioClip mainMenuMusic;
   [SerializeField]
   protected AudioClip defaultLevelMusic;
   [SerializeField]
   protected AudioClip defaultAfterBattleMusic;
   [SerializeField]
   protected AudioClip defaultBattleMusic;
   
   protected AudioClip currentPlayingMusic;
   protected int musicId = 100;
   [SerializeField]
   protected float musicFadeDuration = 5f;

   protected IEnumerator SwitchMusic(AudioClip newMusic)
   {
      
      if (currentPlayingMusic != null)
      {
         MMSoundManagerSoundFadeEvent.Trigger(musicId, musicFadeDuration, 0f,
            new MMTweenType(MMTween.MMTweenCurve.EaseInCubic));

         yield return MMCoroutine.WaitFor(musicFadeDuration);
      }

      MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, musicId);
      
      //MMSoundManagerTrackEvent.Trigger(MMSoundManagerTrackEventTypes.StopTrack, MMSoundManager.MMSoundManagerTracks.Music);
      //MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, musicId);

      if (newMusic != null)
      {

         MMSoundManagerPlayOptions options = MMSoundManagerPlayOptions.Default;
         options.Loop = true;
         options.Location = Vector3.zero;
         options.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Music;
         options.Persistent = true;
         options.ID = musicId;

         MMSoundManagerSoundPlayEvent.Trigger(newMusic, options);
         currentPlayingMusic = newMusic;
      }
   }
   

   public void OnMMEvent(MMGameEvent eventType)
   {
      if (eventType.EventName == "MainMenuLoad")
      {
         StartCoroutine(SwitchMusic(mainMenuMusic));
      }
      else if (eventType.EventName == "LevelLoad")
      {
         StartCoroutine(SwitchMusic(defaultLevelMusic));
      }
      else if (eventType.EventName == "BattleStarted")
      {
         StartCoroutine(SwitchMusic(defaultBattleMusic));
      }
   }

   private void OnEnable()
   {
      this.MMEventStartListening<MMGameEvent>();
   }

   private void OnDisable()
   {
      this.MMEventStopListening<MMGameEvent> ();
   }
}
