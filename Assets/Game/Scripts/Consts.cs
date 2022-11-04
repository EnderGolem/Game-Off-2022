using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
   /// <summary>
   /// Базовый разброс(уменьшение точности) за выстрел
   /// </summary>
   public const float baseRecoil=2f;
   public const float SpreadReducingCoef = 0.3f;
   /// <summary>
   /// Скорость восстановления точности в секунду
   /// </summary>
   public const float SpreadReducingBaseSpeed = 8f;
   public const float MaxReticleDegreesSize = 90f;
   public const float MaxReticleSizeInUnits = 1.8f;

   public const string pathToObjectProperties = "ObjectProperties/";

   public const string DefaultLoadingSceneName = "MyLoadingScreen";
}
