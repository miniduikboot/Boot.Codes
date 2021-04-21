﻿using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using Impostor.Api.Games;
using Boot.Codes.Handlers;
using Boot.Codes.Properties;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Threading;
using Impostor.Api.Events.Managers;
using Microsoft.Extensions.Logging;

namespace Boot.Codes
{
    public class GameCodeManager : IGameCodeManager
    {
        private readonly List<GameCode> _codes;

        private readonly HashSet<GameCode> _inUse;

        private readonly object _sync = new object();

        private readonly ILogger<IGameCodeManager> _logger;

        private readonly IGameCodeFactory _codeFactory;

        public string Path => "Boot.Codes.txt";

        public int SixCharCodes { get; }

        public int FourCharCodes { get; }

        public GameCodeManager(ILogger<GameCodeManager> logger, IGameCodeFactory codeFactory, IEventManager eventManager)
        {
            this._logger = logger;
            this._codeFactory = codeFactory;
            logger.LogInformation("Boot.Codes: Reading file.", Path);

            var validCodes = Read().Select(code => new GameCode(code)).Where(code => !code.IsInvalid).ToList().Shuffle();

            if (validCodes.Count == 0) return;

            this.FourCharCodes = validCodes.Count(code => code.Code.Length == 4);
            this.SixCharCodes = validCodes.Count(code => code.Code.Length == 6);

            this._codes = new List<GameCode>(validCodes);
            this._inUse = new HashSet<GameCode>();

            eventManager.RegisterListener(new GameEventListener(this));
        }

        private IEnumerable<string> Read()
        {
            if (File.Exists(Path))
            {
                var comment = new[] { "--" };
                var lines = File.ReadAllLines(Path, Encoding.UTF8)
                    .Where(line => !line.Trim().StartsWith(comment[0]) && !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Trim().Split(comment, 2, StringSplitOptions.None)[0].ToUpper().TrimEnd())
                    .Distinct().ToArray();

                var invalid = lines.Where(line => line.Length != 6 && line.Length != 4).ToArray();
                var valid = lines.Where(line => !invalid.Contains(line)).ToArray();

                foreach (var line in invalid)
                {
                    _logger.LogWarning($"Boot.Codes: The code \"{line}\" is invalid!", line);
                }

                return valid;
            }

            _logger.LogWarning("Boot.Codes: No word list found.");
            return Enumerable.Empty<string>();
        }

        public GameCode Get()
        {
            lock (_sync)
            {
                if (_codes.Count == 0)
                {
                    _logger.LogWarning("Boot.Codes: Ran out of codes!");
                    return _codeFactory.Create();
                }

                var index = StrongRandom.Next(0, _codes.Count);
                var code = _codes[index];

                _codes.RemoveAt(index);
                _inUse.Add(code);

                return code;
            }
        }

        public void Release(GameCode code)
        {
            lock (_sync)
            {
                if(!_inUse.Contains(code)) return; // generated by the factory
                _inUse.Remove(code);
                _codes.Add(code);
            }
        }
    }
}
