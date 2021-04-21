using Impostor.Api.Games;

namespace Boot.Codes
{
    public interface IGameCodeManager
    {
        int SixCharCodes { get; }

        int FourCharCodes { get; }

        string Path { get; }

        GameCode Get();

        void Release(GameCode code);
    }
}