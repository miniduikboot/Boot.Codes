﻿using System;
using System.IO;
using System.Linq;
using Impostor.Api.Games;
using Boot.Codes.Handlers;
using System.Collections.Generic;
using Impostor.Api.Events.Managers;
using Microsoft.Extensions.Logging;

namespace Boot.Codes
{
    public class GameCodeManager : IGameCodeManager
    {
        private static readonly HashSet<char> V2Chars = "QWXRTYLPESDFGHUJKZOCVBINMA".ToHashSet();

        private readonly List<GameCode> _codes;

        private readonly HashSet<GameCode> _inUse;

        private readonly object _sync = new object();

        private readonly ILogger<IGameCodeManager> _logger;

        private readonly IGameCodeFactory _codeFactory;

        public string Path => System.IO.Path.GetFullPath("Boot.Codes");

        public int SixCharCodes { get; }

        public int FourCharCodes { get; }

        public GameCodeManager(ILogger<GameCodeManager> logger, IGameCodeFactory codeFactory, IEventManager eventManager)
        {
            this._logger = logger;
            this._codeFactory = codeFactory;
            logger.LogInformation("Boot.Codes: Reading files from {Path}", Path);

            var validCodes = Read().ToList().Shuffle();

            if (validCodes.Count == 0) return;

            this.FourCharCodes = validCodes.Count(code => code.Code.Length == 4);
            this.SixCharCodes = validCodes.Count(code => code.Code.Length == 6);

            this._codes = validCodes;
            this._inUse = new HashSet<GameCode>();
            
            eventManager.RegisterListener(new GameEventListener(this));
        }

        private IEnumerable<GameCode> Read()
        {
            var dirInfo = new DirectoryInfo(Path);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
                goto fail;
            }

            var comment = new[] { "--" };
            var codes = new HashSet<GameCode>();

            const StringSplitOptions splitOptions = StringSplitOptions.None;

            var startTime = DateTime.Now;
            var invalid = 0;

            foreach (var file in dirInfo.GetFiles())
            {
                _logger.LogInformation("Boot.Codes: reading \"{Name}\"", file.Name);

                foreach (var line in File.ReadLines(file.FullName))
                {
                    if(string.IsNullOrWhiteSpace(line)) continue;
                    var trimStart = line.TrimStart();
                    if(trimStart.StartsWith(comment[0])) continue;
                    var codeStr = trimStart.Split(comment, 2, splitOptions)[0].TrimEnd();

                    if (codeStr.Length != 6 && codeStr.Length != 4 || codeStr.Any(c=>!V2Chars.Contains(c)))
                    {
                        if (invalid++ < 5) _logger.LogWarning("Boot.Codes: The code \"{code}\" is invalid!", codeStr);
                        if (invalid == 6) _logger.LogWarning("Boot.Codes: Found more invalid codes, please check your input files and clean them up");
                        continue;
                    }

                    var code = new GameCode(codeStr);
                    if (!code.IsInvalid && !codes.Contains(code)) codes.Add(code);
                    else invalid++;
                }
            }

            if (codes.Count == 0) goto fail;

            _logger.LogInformation("Boot.Codes: Finished loading files in {Seconds} seconds, with {invalids} invalid codes.", (DateTime.Now - startTime).Seconds, invalid);

            return codes;

            fail:
            {
                _logger.LogWarning("Boot.Codes: No valid word list found.");
                return Enumerable.Empty<GameCode>();
            }
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
                if (!_inUse.Contains(code)) return; // generated by the factory
                _inUse.Remove(code);
                _codes.Add(code);
            }
        }
    }
}
