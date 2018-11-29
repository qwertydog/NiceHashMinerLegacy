using Newtonsoft.Json;
using NiceHashMiner.Switching;
using NiceHashMinerLegacy.Common.Enums;

namespace NiceHashMiner
{
    public class Globals
    {
        // Constants
        public static string[] MiningLocation = {"eu", "usa", "hk", "jp", "in", "br"};

        public static readonly string DemoUser = "3C5J6E5Fc74DkvL8oQdTWvCWi715JcuTHR";
        
        // change this if TOS changes
        public static int CurrentTosVer = 3;

        // Variables
        public static JsonSerializerSettings JsonSettings;

        public static int ThreadsPerCpu;

        // quickfix guard for checking internet conection
        public static bool IsFirstNetworkCheckTimeout = true;

        public static int FirstNetworkCheckTimeoutTimeMs = 500;
        public static int FirstNetworkCheckTimeoutTries = 10;

        public static string GetLocationUrl(AlgorithmType algorithmType, string miningLocation, NhmConectionType conectionType)
        {
            if (!NHSmaData.TryGetSma(algorithmType, out var sma)) return "";

            var name = sma.Name;
            var nPort = sma.Port;
            var sslPort = 30000 + nPort;

            var prefix = "";
            var port = nPort;
            switch (conectionType)
            {
                case NhmConectionType.LOCKED:
                    return miningLocation;
                case NhmConectionType.STRATUM_TCP:
                    prefix = "stratum+tcp://";
                    break;
                case NhmConectionType.STRATUM_SSL:
                    prefix = "stratum+ssl://";
                    port = sslPort;
                    break;
            }

            return prefix
                   + name
                   + "." + miningLocation
                   + ".nicehash.com:"
                   + port;
        }

        public static string GetBitcoinUser()
        {
            return BitcoinAddress.ValidateBitcoinAddress(Configs.ConfigManager.GeneralConfig.BitcoinAddress.Trim())
                ? Configs.ConfigManager.GeneralConfig.BitcoinAddress.Trim()
                : DemoUser;
        }
    }
}
