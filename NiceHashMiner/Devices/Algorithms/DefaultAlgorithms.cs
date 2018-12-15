﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NiceHashMiner.Algorithms;
using NiceHashMinerLegacy.Common.Enums;
using NiceHashMinerLegacy.Extensions;

namespace NiceHashMiner.Devices.Algorithms
{
    public static class DefaultAlgorithms
    {
        #region All

        private static Dictionary<MinerBaseType, List<Algorithm>> All => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.XmrStak,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.XmrStak, AlgorithmType.CryptoNightV7, ""),
                    new Algorithm(MinerBaseType.XmrStak, AlgorithmType.CryptoNightHeavy, ""),
                    new Algorithm(MinerBaseType.XmrStak, AlgorithmType.CryptoNightV8, ""),

                }
            }
        };

        #endregion

        #region GPU

        private static Dictionary<MinerBaseType, List<Algorithm>> Gpu => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.Claymore,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, ""),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Decred),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Lbry),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Pascal),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Sia),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Blake2s),
                    new DualAlgorithm(MinerBaseType.Claymore, AlgorithmType.DaggerHashimoto, AlgorithmType.Keccak)
                }
            },
            {
                MinerBaseType.Phoenix,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.Phoenix, AlgorithmType.DaggerHashimoto, "")
                }
            },
        };

        #endregion

        #region CPU

        public static Dictionary<MinerBaseType, List<Algorithm>> Cpu => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.Xmrig,
                new List<Algorithm>
                {
                    //new Algorithm(MinerBaseType.Xmrig, AlgorithmType.CryptoNight, ""),
                    new Algorithm(MinerBaseType.Xmrig, AlgorithmType.CryptoNightV7, ""),
                    new Algorithm(MinerBaseType.Xmrig, AlgorithmType.CryptoNightV8, "")
                }
            },
            {
                MinerBaseType.cpuminer,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.cpuminer, AlgorithmType.Lyra2z, "lyra2z")
                }
            }
        }.ConcatDict(All);

        #endregion

        #region AMD

        private const string RemDis = " --remove-disabled";
        private const string DefaultParam = RemDis + AmdGpuDevice.DefaultParam;

        public static Dictionary<MinerBaseType, List<Algorithm>> Amd => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.sgminer,
                new List<Algorithm>
                {
                    /*
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.NeoScrypt, "neoscrypt")
                    {
                        ExtraLaunchParameters =
                            DefaultParam +
                            "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 4"
                    },
                    */
                    /*
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.DaggerHashimoto, "ethash")
                    {
                        ExtraLaunchParameters = RemDis + "--xintensity 512 -w 192 -g 1"
                    },
                    */
                    /*
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.Decred, "decred")
                    {
                        ExtraLaunchParameters = RemDis + "--gpu-threads 1 --xintensity 256 --lookup-gap 2 --worksize 64"
                    },
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.Lbry, "lbry")
                    {
                        ExtraLaunchParameters = DefaultParam + "--xintensity 512 --worksize 128 --gpu-threads 2"
                    },
                    */
                    /*
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.Pascal, "pascal")
                    {
                        ExtraLaunchParameters = DefaultParam + "--intensity 21 -w 64 -g 2"
                    },
                    */
                    /*
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.X11Gost, "sibcoin-mod")
                    {
                        ExtraLaunchParameters = DefaultParam + "--intensity 16 -w 64 -g 2"
                    },
                    */
                    /*
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.Keccak, "keccak")
                    {
                        ExtraLaunchParameters = DefaultParam + "--intensity 15"
                    },
                    */
                    new Algorithm(MinerBaseType.sgminer, AlgorithmType.X16R, "X16R")
                    {
                        ExtraLaunchParameters = DefaultParam + "--gpu-threads 2"
                    }
                }
            },
                    { MinerBaseType.CastXMR,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.CastXMR, AlgorithmType.CryptoNightV7, "cryptonightV7") { },
                            new Algorithm(MinerBaseType.CastXMR, AlgorithmType.CryptoNightV8, "cryptonightV8") { },
                            new Algorithm(MinerBaseType.CastXMR, AlgorithmType.CryptoNightHeavy, "cryptonightHeavy") { }
                        }
                    },
                    { MinerBaseType.lyclMiner,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.lyclMiner, AlgorithmType.Lyra2REv2, "Lyra2REv2") { }
                        }
                    },
                    { MinerBaseType.XmrigAMD,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.XmrigAMD, AlgorithmType.CryptoNightV7, "CryptoNightV7") { },
                            new Algorithm(MinerBaseType.XmrigAMD, AlgorithmType.CryptoNightV8, "CryptoNightV8") { }
                        }
                    },
                    { MinerBaseType.SRBMiner,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.SRBMiner, AlgorithmType.CryptoNightV8, "CryptoNightV8")
                            {
                                ExtraLaunchParameters = "--cgpuintensity 0 --cgputhreads 2 "
                            },
                            new Algorithm(MinerBaseType.SRBMiner, AlgorithmType.CryptoNightHeavy, "CryptoNightHeavy")
                            {
                                ExtraLaunchParameters = "--cgpuintensity 0 --cgputhreads 2 "
                            }
                        }
                    },
                    /*
                    { MinerBaseType.mkxminer,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.mkxminer, AlgorithmType.Lyra2REv2, "Lyra2REv2"),
                        }
                    },
                    */
                    /*
                    { MinerBaseType.mkxminer,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.mkxminer, AlgorithmType.Lyra2z, "Lyra2z"),
                        }
                    },
                    */
                    { MinerBaseType.teamredminer,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.teamredminer, AlgorithmType.Lyra2z, "Lyra2z"),
                            new Algorithm(MinerBaseType.teamredminer, AlgorithmType.CryptoNightV8, "CryptoNightV8"),
                        }
                    },
                    { MinerBaseType.lolMiner,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.lolMiner, AlgorithmType.ZHash, "ZHash"),
                        }
                    },
            {
                MinerBaseType.Claymore,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.Claymore, AlgorithmType.CryptoNightV7, ""),
                    new Algorithm(MinerBaseType.Claymore, AlgorithmType.NeoScrypt, "neoscrypt"),
                    new Algorithm(MinerBaseType.Claymore, AlgorithmType.Equihash, "equihash")
                }
            },
            /*
            {
                MinerBaseType.OptiminerAMD,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.OptiminerAMD, AlgorithmType.Equihash, "equihash")
                }
            },
            */
            {
                MinerBaseType.Prospector,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.Prospector, AlgorithmType.Skunk, "sigt"),
                    //new Algorithm(MinerBaseType.Prospector, AlgorithmType.Sia, "sia")
                }
            }
        }.ConcatDictList(All, Gpu);

        #endregion

        #region NVIDIA

        public static Dictionary<MinerBaseType, List<Algorithm>> Nvidia => new Dictionary<MinerBaseType, List<Algorithm>>
        {
            {
                MinerBaseType.ccminer,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.NeoScrypt, "neoscrypt"),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.Lyra2REv2, "lyra2v2"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Decred, "decred"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Lbry, "lbry"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.X11Gost, "sib"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Blake2s, "blake2s"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Sia, "sia"),
                    //new Algorithm(MinerBaseType.ccminer, AlgorithmType.Keccak, "keccak"),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.Skunk, "skunk"),
                    new Algorithm(MinerBaseType.ccminer, AlgorithmType.Lyra2z, "lyra2z")
                }
            },
            /*
            {
                MinerBaseType.ccminer_alexis,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.ccminer_alexis, AlgorithmType.X11Gost, "sib"),
                    new Algorithm(MinerBaseType.ccminer_alexis, AlgorithmType.Keccak, "keccak")
                }
            },
            */
                        { MinerBaseType.hsrneoscrypt,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.hsrneoscrypt, AlgorithmType.NeoScrypt, "Neoscrypt"),
                        }
                    },
                        { MinerBaseType.CryptoDredge,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.Lyra2REv2, "Lyra2REv2"),
                            new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.Lyra2z, "Lyra2z"),
                            new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.Blake2s, "Blake2s"),
                            new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.NeoScrypt, "NeoScrypt"),
                            new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.Skunk, "Skunk"),
                            new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.X16R, "X16R"),
                           new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.CryptoNightHeavy, "CryptoNightHeavy"),
                           new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.CryptoNightV7, "CryptoNightV7"),
                           new Algorithm(MinerBaseType.CryptoDredge, AlgorithmType.CryptoNightV8, "CryptoNightV8"),
                        }
                    },

                        { MinerBaseType.trex,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.trex, AlgorithmType.Lyra2z, "Lyra2z"),
                            new Algorithm(MinerBaseType.trex, AlgorithmType.Skunk, "Skunk"),
                            new Algorithm(MinerBaseType.trex, AlgorithmType.X16R, "X16R"),
                        }
                    },

                        { MinerBaseType.ZEnemy,
                        new List<Algorithm>() {
                            new Algorithm(MinerBaseType.ZEnemy, AlgorithmType.X16R, "X16R"),
                            new Algorithm(MinerBaseType.ZEnemy, AlgorithmType.Skunk, "Skunk"),
                        }
                    },
        /*
            {
                MinerBaseType.ethminer,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.ethminer, AlgorithmType.DaggerHashimoto, "daggerhashimoto")
                }
            },
            */
            /*
            {
                MinerBaseType.nheqminer,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.nheqminer, AlgorithmType.Equihash, "equihash")
                }
            },
            */
            
            {
                MinerBaseType.EWBF,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.EWBF, AlgorithmType.ZHash, "")
                }
            },
            {
                MinerBaseType.GMiner,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.GMiner, AlgorithmType.ZHash, "")
                }
            },
            {
                MinerBaseType.dstm,
                new List<Algorithm>
                {
                    new Algorithm(MinerBaseType.dstm, AlgorithmType.Equihash, "")
                }
            }
        }.ConcatDictList(All, Gpu);

        #endregion
    }
}
