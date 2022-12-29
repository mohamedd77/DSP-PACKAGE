using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            if (InputSignal2 == null)     //auto corr
            {
                List<float> ac = new List<float>();     // NonNormalizedCorrelation
                List<double> signal1_samples = new List<double>(); //x
                List<double> sig_copy = new List<double>();  
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    signal1_samples.Add(InputSignal1.Samples[i]);
                    sig_copy.Add(InputSignal1.Samples[i]);
                }

                double nor_sum = 0 ,s_sum = 0, copy_sum = 0;
                for (int i = 0; i < signal1_samples.Count; i++)
                {
                    s_sum += signal1_samples[i] * signal1_samples[i];
                    copy_sum += sig_copy[i] * sig_copy[i];
                }
                nor_sum = s_sum * copy_sum;  
                nor_sum = Math.Sqrt(nor_sum);
                nor_sum /= signal1_samples.Count;

                if (InputSignal1.Periodic != true)                                      //shift by zero
                {
                    for (int i = 0; i < sig_copy.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0)                                                       //values
                        {
                            double f = 0;
                            for (int j = 0; j < sig_copy.Count - 1; j++)
                            {
                                sig_copy[j] = sig_copy[j + 1];
                                sum += sig_copy[j] * signal1_samples[j];
                            }
                            sig_copy[sig_copy.Count - 1] = f;
                            sum += sig_copy[sig_copy.Count - 1] * signal1_samples[signal1_samples.Count - 1];
                        }
                        else                                                             //zeros
                        {
                            for (int j = 0; j < sig_copy.Count; j++)
                                sum += sig_copy[j] * signal1_samples[j];
                        }
                        ac.Add((float)sum / sig_copy.Count);
                    }
                }

                else                                                        //shift by value
                {
                    for (int i = 0; i < sig_copy.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0)
                        {
                            double first_element = sig_copy[0];
                            for (int j = 0; j < sig_copy.Count - 1; j++)
                            {
                                sig_copy[j] = sig_copy[j + 1];
                                sum += sig_copy[j] * signal1_samples[j];
                            }
                            sig_copy[sig_copy.Count - 1] = first_element;
                            sum += sig_copy[sig_copy.Count - 1] * signal1_samples[signal1_samples.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < sig_copy.Count; j++)
                                sum += sig_copy[j] * sig_copy[j];
                        }
                        ac.Add((float)sum / sig_copy.Count);
                    }

                }

                OutputNonNormalizedCorrelation = ac;
                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                    OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / nor_sum));
            }




            else       //cross corr 
            {
                List<float> cc = new List<float>();
                List<double> signal1_samples = new List<double>();
                List<double> signal2_samples = new List<double>();
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    signal1_samples.Add(InputSignal1.Samples[i]);

                }
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                {
                    signal2_samples.Add(InputSignal2.Samples[i]);

                }
                if (signal1_samples.Count != signal2_samples.Count)
                {
                    int len = signal1_samples.Count + signal2_samples.Count - 1;
                    for (int i = signal1_samples.Count; i < len; i++)
                        signal1_samples.Add(0);
                    for (int i = signal2_samples.Count; i < len; i++)
                        signal2_samples.Add(0);
                }

                double normalize_sum = 0, sig1_sum = 0, sig2_sum = 0;
                for (int i = 0; i < signal1_samples.Count; i++)
                {
                    sig1_sum += signal1_samples[i] * signal1_samples[i];
                    sig2_sum += signal2_samples[i] * signal2_samples[i];
                }
                normalize_sum = sig1_sum * sig2_sum;
                normalize_sum = Math.Sqrt(normalize_sum);
                normalize_sum /= signal1_samples.Count;

                if (InputSignal1.Periodic != true)  
                {
                    for (int i = 0; i < signal2_samples.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0)
                        {
                            double f = 0;
                            for (int j = 0; j < signal2_samples.Count - 1; j++)
                            {
                                signal2_samples[j] = signal2_samples[j + 1];
                                sum += signal2_samples[j] * signal1_samples[j];
                            }
                            signal2_samples[signal2_samples.Count - 1] = f;
                            sum += signal2_samples[signal2_samples.Count - 1] * signal1_samples[signal1_samples.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < signal2_samples.Count; j++)
                                sum += signal1_samples[j] * signal2_samples[j];
                        }
                        cc.Add((float)sum / signal1_samples.Count);
                    }
                }

                else
                {
                    for (int i = 0; i < signal2_samples.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0)
                        {
                            double f = signal2_samples[0];
                            for (int j = 0; j < signal2_samples.Count - 1; j++)
                            {
                                signal2_samples[j] = signal2_samples[j + 1];
                                sum += signal2_samples[j] * signal1_samples[j];//power2
                            }
                            signal2_samples[signal2_samples.Count - 1] = f;
                            sum += signal2_samples[signal2_samples.Count - 1] * signal1_samples[signal1_samples.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < signal2_samples.Count; j++)
                                sum += signal1_samples[j] * signal2_samples[j];
                        }
                        cc.Add((float)sum / signal2_samples.Count);
                    }
                }
                OutputNonNormalizedCorrelation = cc;
                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                {
                    OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normalize_sum));

                }
            }
        }
    }
}