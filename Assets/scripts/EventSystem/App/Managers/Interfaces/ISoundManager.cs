
using GrandDevs.ExtremeScooling.Common;

namespace GrandDevs.ExtremeScooling
{
    public interface ISoundManager
    {
        bool SoundOn { get; set; }

        void PlaySound(Enumerators.SoundType soundType, bool random = false, SoundManager.SoundParameters parameters = null);

        void StopSound(Enumerators.SoundType soundType);

    }
}