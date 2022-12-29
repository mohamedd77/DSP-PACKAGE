using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }
        DirectCorrelation corr_tion = new DirectCorrelation();
        public override void Run()
        {
            List<float> x = new List<float>();
            float Ts = InputSamplingPeriod;
            //int N = OutputNonNormalizedCorrelation.Count;
            corr_tion.InputSignal1 = InputSignal1; corr_tion.InputSignal2 = InputSignal2;
            corr_tion.Run();
            for (int n = 0; n < corr_tion.OutputNonNormalizedCorrelation.Count; n++)
            {
                float res = (Math.Abs(corr_tion.OutputNonNormalizedCorrelation[n]));
                x.Add(res);
            }
            int j = corr_tion.OutputNonNormalizedCorrelation.IndexOf(x.Max());
            OutputTimeDelay = Ts * j;
            
        }
    }
}