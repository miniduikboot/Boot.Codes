using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impostor.Api.Games;

namespace Boot.Codes
{
    public interface IGameCodeManager
    {
        string Path { get; }

        GameCode Get();

        void Release(GameCode code);
    }
}
