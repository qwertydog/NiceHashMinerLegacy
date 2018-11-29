﻿using NiceHashMiner.Configs;
using NiceHashMiner.Miners.Parsing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NiceHashMiner.Algorithms;
using NiceHashMinerLegacy.Common.Enums;
using System.Windows.Forms;

namespace NiceHashMiner.Miners
{
    public class Xmrig : Miner
    {
        private int benchmarkTimeWait = 300;
        private const string LookForStart = "speed 10s/60s/15m";
        private const string LookForEnd = "h/s max";
        private System.Diagnostics.Process CMDconfigHandle;
        public Xmrig() : base("Xmrig")
        { }

        public override void Start(string url, string btcAdress, string worker)
        {
            LastCommandLine = GetStartCommand(url, btcAdress, worker);
            foreach (var pair in MiningSetup.MiningPairs)
            {
                if (pair.Device.DeviceType == DeviceType.NVIDIA)
                {
                   // RunCMDBeforeMining("NVIDIA");
                }
                else if (pair.Device.DeviceType == DeviceType.AMD)
                {
                   // RunCMDBeforeMining("AMD");
                } else if (pair.Device.DeviceType == DeviceType.CPU)
                {
                   // RunCMDBeforeMining("CPU");
                }
            }

           
            ProcessHandle = _Start();
        }
        /*
        private string GetStartCommand(string url, string btcAdress, string worker)
        {
            var extras = ExtraLaunchParametersParser.ParseForMiningSetup(MiningSetup, DeviceType.CPU);
            return $" -o {url} -u {btcAdress}.{worker}:x --nicehash {extras} --api-port {ApiPort}";
        }
        */
        private string GetStartCommand(string url, string btcAdress, string worker)
        {
            var extras = ExtraLaunchParametersParser.ParseForMiningSetup(MiningSetup, DeviceType.CPU);
            var algo = "cryptonightv7";
            var port = "3363";
            var variant = " --variant 1 ";
            if (MiningSetup.CurrentAlgorithmType.Equals(AlgorithmType.CryptoNightV8))
            {
                algo = "cryptonightv8";
                port = "3367";
                variant = " --variant 2 ";
            }

                return $" -o {url} {variant} -u {btcAdress}.{worker}:x --nicehash {extras} --api-port {ApiPort} --donate-level=1 "
                + $" -o stratum+tcp://{algo}.usa.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.hk.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.jp.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.in.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.br.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.eu.nicehash.com:{port} -u {btcAdress}.{worker}:x ";
        }
        private string GetStartBenchmarkCommand(string url, string btcAdress, string worker)
        {
            var extras = ExtraLaunchParametersParser.ParseForMiningSetup(MiningSetup, DeviceType.CPU);
            var algo = "cryptonightv7";
            var port = "3363";
            var variant = " --variant 1 ";
            if (MiningSetup.CurrentAlgorithmType.Equals(AlgorithmType.CryptoNightV8))
            {
                algo = "cryptonightv8";
                port = "3367";
                variant = " --variant 2 ";
                return $" -o stratum+tcp://xmr-eu.dwarfpool.com:8005 {variant} -u 42fV4v2EC4EALhKWKNCEJsErcdJygynt7RJvFZk8HSeYA9srXdJt58D9fQSwZLqGHbijCSMqSP4mU7inEEWNyer6F7PiqeX.{worker} -p x {extras} --api-port {ApiPort} --donate-level=1 "
                + $" -o stratum+tcp://{algo}.eu.nicehash.com:{port} -u {btcAdress}.{worker}:x ";
            }

            return $" -o {url} {variant} -u {btcAdress}.{worker}:x --nicehash {extras} --api-port {ApiPort} --donate-level=1 "
                + $" -o stratum+tcp://{algo}.usa.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.hk.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.jp.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.in.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.br.nicehash.com:{port} -u {btcAdress}.{worker}:x "
                + $" -o stratum+tcp://{algo}.eu.nicehash.com:{port} -u {btcAdress}.{worker}:x ";
        }

        protected override void _Stop(MinerStopType willswitch)
        {
            Helpers.ConsolePrint("XMRIG", "_Stop");
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        protected override int GetMaxCooldownTimeInMilliseconds()
        {
            return 60 * 1000 * 5; // 5 min
        }

        public override async Task<ApiData> GetSummaryAsync()
        {
            return await GetSummaryCpuAsync();
        }

        protected override bool IsApiEof(byte third, byte second, byte last)
        {
            return third == 0x7d && second == 0xa && last == 0x7d;
        }

        #region Benchmark

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time)
        {
            var server = Globals.GetLocationUrl(algorithm.NiceHashID,
                Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation],
                ConectionType);
            //_benchmarkTimeWait = time;
            return GetStartBenchmarkCommand(server, Globals.DemoUser, ConfigManager.GeneralConfig.WorkerName.Trim())
                + $" -l {GetLogFileName()} --print-time=2 --nicehash";
        }

        protected override void BenchmarkThreadRoutine(object commandLine)
        {
            BenchmarkThreadRoutineAlternate(commandLine, benchmarkTimeWait);
        }

        protected override void ProcessBenchLinesAlternate(string[] lines)
        {
            // Xmrig reports 2.5s and 60s averages, so prefer to use 60s values for benchmark
            // but fall back on 2.5s values if 60s time isn't hit
            var twoSecTotal = 0d;
            var sixtySecTotal = 0d;
            var twoSecCount = 0;
            var sixtySecCount = 0;
            foreach (var line in lines)
            {
                BenchLines.Add(line);
                var lineLowered = line.ToLower();
                if (!lineLowered.Contains(LookForStart)) continue;
                var speeds = Regex.Match(lineLowered, $"{LookForStart} (.+?) {LookForEnd}").Groups[1].Value.Split();

                try { 
                if (double.TryParse(speeds[1], out var sixtySecSpeed))
                    {
                    sixtySecTotal += sixtySecSpeed;
                    ++sixtySecCount;
                    }
                else if (double.TryParse(speeds[0], out var twoSecSpeed))
                    {
                    // Store 2.5s data in case 60s is never reached
                    twoSecTotal += twoSecSpeed;
                    ++twoSecCount;
                    }
                }
                catch
                {
                    MessageBox.Show("Unsupported miner version - " + MiningSetup.MinerPath,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BenchmarkSignalFinnished = true;
                    return;
                }
            }

            if (sixtySecCount > 0 && sixtySecTotal > 0)
            {
                // Run iff 60s averages are reported
                BenchmarkAlgorithm.BenchmarkSpeed = sixtySecTotal / sixtySecCount;
            }
            else if (twoSecCount > 0)
            {
                // Run iff no 60s averages are reported but 2.5s are
                BenchmarkAlgorithm.BenchmarkSpeed = twoSecTotal / twoSecCount;
            }
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata)
        {
            CheckOutdata(outdata);
        }

        protected override bool BenchmarkParseLine(string outdata)
        {
            Helpers.ConsolePrint(MinerTag(), outdata);
            return false;
        }

        #endregion
    }
}
