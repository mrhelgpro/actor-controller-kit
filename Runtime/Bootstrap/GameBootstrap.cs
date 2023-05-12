using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public enum GameMode { Loading, Clip, Play, Pause }
    public class GameBootstrap : MonoBehaviour
    {
        public static GameMode Mode = GameMode.Play;
    }
}