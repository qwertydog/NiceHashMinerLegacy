﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners.XmrStak.Configs
{
    public abstract class XmrStakGpuItem
    {
        public int index;
        public bool affine_to_cpu;

        protected XmrStakGpuItem(int index, bool affineToCpu)
        {
            this.index = index;
            affine_to_cpu = affineToCpu;
        }
    }

    public abstract class XmrStakConfigGpu<T> where T : XmrStakGpuItem
    {
        public void SetupThreads(IEnumerable<int> indices)
        {
            var indexList = indices as IList<int> ?? indices.ToList();
            // Remove threads without an activated index
            gpu_threads_conf = gpu_threads_conf.FindAll(x => indexList.Contains(x.index));
            foreach (var i in indexList)
            {
                if (gpu_threads_conf.All(x => x.index != i))
                {
                    // Ideally this default shouldn't run, devices should be present in nvidia/amd.txt generated by xmr-stak with proper defaults
                    Helpers.ConsolePrint("Xmr-Stak-Config",
                        $"GPU entry for index {i} not found, setting with default. Performance will likely not be optimal.");
                    gpu_threads_conf.Add(GetGpuItem(i, false));
                }
            }
        }

        protected abstract T GetGpuItem(int index, bool affine);

        /*
         * GPU configuration. You should play around with threads and blocks as the fastest settings will vary.
         * index         - GPU index number usually starts from 0.
         * threads       - Number of GPU threads (nothing to do with CPU threads).
         * blocks        - Number of GPU blocks (nothing to do with CPU threads).
         * bfactor       - Enables running the Cryptonight kernel in smaller pieces.
         *                 Increase if you want to reduce GPU lag. Recommended setting on GUI systems - 8
         * bsleep        - Insert a delay of X microseconds between kernel launches.
         *                 Increase if you want to reduce GPU lag. Recommended setting on GUI systems - 100
         * affine_to_cpu - This will affine the thread to a CPU. This can make a GPU miner play along nicer with a CPU miner.
         *
         * On the first run the miner will look at your system and suggest a basic configuration that will work,
         * you can try to tweak it from there to get the best performance.
         *
         * A filled out configuration should look like this:
         * "gpu_threads_conf" :
         * [
         *     { "index" : 0, "threads" : 17, "blocks" : 60, "bfactor" : 0, "bsleep" :  0, "affine_to_cpu" : false},
         * ],
         */

        public List<T> gpu_threads_conf = new List<T>();
    }

    public class XmrStakAmdItem : XmrStakGpuItem
    {
        public int intensity = 512;
        public int worksize = 8;
        public bool strided_index = true;
        public int mem_chunk = 2;
        public bool comp_mode = true;
        public int unroll = 8;

        public XmrStakAmdItem(int index, bool affine)
            : base(index, affine)
        { }
    }

    public class XmrStakConfigAmd : XmrStakConfigGpu<XmrStakAmdItem>
    {
        protected override XmrStakAmdItem GetGpuItem(int index, bool affine)
        {
            return new XmrStakAmdItem(index, affine);
        }

        /*
        * Platform index. This will be 0 unless you have different OpenCL platform - eg. AMD and Intel.
        */
        public int platform_index = 0;
    }

    public class XmrStakNvidiaItem : XmrStakGpuItem
    {
        public int threads = 12;
        public int blocks = 60;
        public int bfactor = 8;
        public int bsleep = 100;
        public int sync_mode = 3;
        public int mem_mode = 1;

        public XmrStakNvidiaItem(int index, bool affine)
            : base(index, affine)
        { }
    }

    public class XmrStakConfigNvidia : XmrStakConfigGpu<XmrStakNvidiaItem>
    {
        protected override XmrStakNvidiaItem GetGpuItem(int index, bool affine)
        {
            return new XmrStakNvidiaItem(index, affine);
        }

        public void OverrideBVals()
        {
            foreach (var thread in gpu_threads_conf)
            {
                thread.bfactor = 8;
                thread.bsleep = 100;
            }
        }
    }
}
