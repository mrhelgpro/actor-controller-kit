using UnityEngine;

namespace Actormachine
{
    public enum GameMode { Loading, Clip, Play, Pause }
    public class BootstrapGame : MonoBehaviour
    {
        public static GameMode Mode = GameMode.Play;
    }
}