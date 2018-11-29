﻿using NiceHashMiner.Algorithms;
using NiceHashMiner.Devices;
using NiceHashMiner.Miners.Equihash;
using NiceHashMinerLegacy.Common.Enums;

namespace NiceHashMiner.Miners
{
    public static class MinerFactory
    {
        private static Miner CreateEthminer(DeviceType deviceType)
        {
            if (DeviceType.AMD == deviceType)
            {
                return new MinerEtherumOCL();
            }

            return DeviceType.NVIDIA == deviceType ? new MinerEtherumCUDA() : null;
        }

        private static Miner CreateClaymore(Algorithm algorithm)
        {
            switch (algorithm.NiceHashID)
            {
                case AlgorithmType.Equihash:
                    return new ClaymoreZcashMiner();
                case AlgorithmType.CryptoNightV7:
                    return new ClaymoreCryptoNightMiner();
                case AlgorithmType.DaggerHashimoto:
                    return new ClaymoreDual(algorithm.SecondaryNiceHashID);
                case AlgorithmType.NeoScrypt:
                    return new ClaymoreNeoscryptMiner();
            }

            return null;
        }

        private static Miner CreateExperimental(DeviceType deviceType, AlgorithmType algorithmType)
        {
            if (AlgorithmType.NeoScrypt == algorithmType && DeviceType.NVIDIA == deviceType)
            {
                return new Ccminer();
            }

            return null;
        }

        public static Miner CreateMiner(DeviceType deviceType, Algorithm algorithm)
        {
            switch (algorithm.MinerBaseType)
            {
                case MinerBaseType.ccminer:
                    return new Ccminer();
                case MinerBaseType.sgminer:
                    return new Sgminer();
                case MinerBaseType.nheqminer:
                    return new NhEqMiner();
                case MinerBaseType.ethminer:
                    return CreateEthminer(deviceType);
                case MinerBaseType.Claymore:
                    return CreateClaymore(algorithm);
                case MinerBaseType.OptiminerAMD:
                    return new OptiminerZcashMiner();
                //case MinerBaseType.excavator:
                //    return new Excavator();
                case MinerBaseType.XmrStak:
                    return new XmrStak.XmrStak();
                case MinerBaseType.ccminer_alexis:
                    return new Ccminer();
                case MinerBaseType.experimental:
                    return CreateExperimental(deviceType, algorithm.NiceHashID);
                case MinerBaseType.EWBF:
                    return new Ewbf();
                case MinerBaseType.Prospector:
                    return new Prospector();
                case MinerBaseType.Xmrig:
                    return new Xmrig();
                case MinerBaseType.XmrigAMD:
                    return new XmrigAMD();
                case MinerBaseType.SRBMiner:
                    return new SRBMiner();
                case MinerBaseType.dstm:
                    return new Dstm();
                case MinerBaseType.cpuminer:
                    return new CpuMiner();
                case MinerBaseType.CastXMR:
                    return new CastXMR();
                case MinerBaseType.hsrneoscrypt:
                    return new hsrneoscrypt();
                case MinerBaseType.CryptoDredge:
                    return new CryptoDredge();
                case MinerBaseType.ZEnemy:
                    return new ZEnemy();
                case MinerBaseType.lyclMiner:
                    return new lyclMiner();
                case MinerBaseType.trex:
                    return new trex();
                case MinerBaseType.mkxminer:
                    return new mkxminer();
                case MinerBaseType.teamredminer:
                    return new teamredminer();
                case MinerBaseType.Phoenix:
                    return new Phoenix();
                case MinerBaseType.GMiner:
                    return new GMiner();
                case MinerBaseType.lolMiner:
                    return new lolMiner();
            }

            return null;
        }

        // create miner creates new miners based on device type and algorithm/miner path
        public static Miner CreateMiner(ComputeDevice device, Algorithm algorithm)
        {
            if (device != null && algorithm != null)
            {
                return CreateMiner(device.DeviceType, algorithm);
            }

            return null;
        }
    }
}
