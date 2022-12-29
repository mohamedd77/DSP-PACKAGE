﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{     //Math.Round(Frequencies[i], 1)

    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            string file_path = @"C:\Users\Public\New folder\DSPToolbox";                   
            Signal InputSignal = LoadSignal(SignalPath);

            FIR fir = new FIR();
            fir.InputFilterType = FILTER_TYPES.BAND_PASS; 
            fir.InputFS = Fs;
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputTimeDomainSignal = InputSignal;
            fir.Run();
            Signal sigfir = fir.OutputYn;

            string full_path = file_path + "\\FilteredSignal.ds";
            SaveSignal(full_path, fir.OutputYn, false, false);

            List<float> samples = new List<float>();
            Signal sigsamplings = new Signal(samples, false);
            if (newFs >= 2 * maxF)
            {
                Sampling sampling = new Sampling();
                sampling.L = L;
                sampling.M = M;
                sampling.InputSignal = sigfir;
                sampling.Run();
                sigsamplings = sampling.OutputSignal;

                full_path = file_path + "\\sampleSignal.ds";
                SaveSignal(full_path, sigsamplings, false, false);
            }
            else
            {
                sigsamplings = fir.OutputYn;
            }

            DC_Component dc = new DC_Component();
            dc.InputSignal = sigsamplings;
            dc.Run();
            Signal sigdc = dc.OutputSignal;
            full_path = file_path + "\\DC_COMPSignal.ds";
            SaveSignal(full_path, sigdc, false, false);

            Normalizer normalization = new Normalizer();
            normalization.InputMaxRange = 1;
            normalization.InputMinRange = -1;
            normalization.InputSignal = sigdc;
            normalization.Run();
            Signal signorm = normalization.OutputNormalizedSignal;

            full_path = file_path + "\\signorm.ds";
            SaveSignal(full_path, signorm, false, false);

  
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputSamplingFrequency = Fs;
            dft.InputTimeDomainSignal = signorm;
            dft.Run();
            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;

            full_path = file_path + "\\DFT_COMPSignal.ds";
            SaveSignal(full_path, OutputFreqDomainSignal, true, false);  // true ( freq domain)

        }
        /// <summary>
        /// /.............................................................................
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
        public void SaveSignal(string filePath, Signal sig, bool flag_freq_or_time, bool periodic_or_not)
        {

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                if (flag_freq_or_time == true) //flag_freq_or_time : if freq print 1 else print 0
                {
                    writer.WriteLine(1);
                }
                else
                {
                    writer.WriteLine(0);
                }
                if (periodic_or_not == true) //periodic_or_not : if preodic print 1 else print 0
                {
                    writer.WriteLine(1);
                }
                else
                {
                    writer.WriteLine(0);
                }



                if (flag_freq_or_time == false) //signal in time domain
                {

                    writer.WriteLine(sig.Samples.Count());

                    for (int i = 0; i < sig.Samples.Count(); i++)
                    {
                        writer.Write(sig.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.Write(sig.Samples[i]);
                        writer.WriteLine();
                    }

                }
                else  // signal in Freq
                {
                    writer.WriteLine(sig.FrequenciesAmplitudes.Count());

                    for (int i = 0; i < sig.FrequenciesAmplitudes.Count(); i++)
                    {
                        writer.Write(sig.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(sig.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.Write(sig.FrequenciesPhaseShifts[i]);
                        writer.WriteLine();
                    }
                }


            }
        }
    }
}


//﻿using DSPAlgorithms.DataStructures;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//namespace DSPAlgorithms.Algorithms
//{
//    public class PracticalTask2 : Algorithm
//    {
//        public String SignalPath { get; set; }
//        public float Fs { get; set; }
//        public float miniF { get; set; }
//        public float maxF { get; set; }
//        public float newFs { get; set; }
//        public int L { get; set; } //upsampling factor
//        public int M { get; set; } //downsampling factor
//        public Signal OutputFreqDomainSignal { get; set; }

//        public override void Run()
//        {
//            Signal InputSignal = LoadSignal(SignalPath);
//            /*string fullpath = "";
//            using (StreamWriter str = new StreamWriter("coftients.txt", false))
//            {
//                Writer.WriteLine(1);
//                Writer.WriteLine(0);
//                Writer.WriteLine(Res.Frequencies.count);

//                for (int i = 0; i < Res.Frequencies.count; i++)
//                {
//                    Writer.WriteLine(Res.Frequencies[i]);
//                }

//            }*/

//            //..............................................................................
//            List<float> output = new List<float>();
//            FIR fir = new FIR();
//            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.HIGH;
//            fir.InputStopBandAttenuation = 50;
//            fir.InputTransitionBand = 500;

//            ///diplay
//            DC_Component dc = new DC_Component();//1
//            ///diplay
//            Normalizer norm = new Normalizer();//2
//            ///diplay
//            DiscreteFourierTransform dft = new DiscreteFourierTransform();//3
//            ///diplay
//            throw new NotImplementedException();
//        }

//        public Signal LoadSignal(string filePath)
//        {
//            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
//            var sr = new StreamReader(stream);

//            var sigType = byte.Parse(sr.ReadLine());
//            var isPeriodic = byte.Parse(sr.ReadLine());
//            long N1 = long.Parse(sr.ReadLine());

//            List<float> SigSamples = new List<float>(unchecked((int)N1));
//            List<int> SigIndices = new List<int>(unchecked((int)N1));
//            List<float> SigFreq = new List<float>(unchecked((int)N1));
//            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
//            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

//            if (sigType == 1)
//            {
//                SigSamples = null;
//                SigIndices = null;
//            }

//            for (int i = 0; i < N1; i++)
//            {
//                if (sigType == 0 || sigType == 2)
//                {
//                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
//                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
//                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
//                }
//                else
//                {
//                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
//                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
//                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
//                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
//                }
//            }

//            if (!sr.EndOfStream)
//            {
//                long N2 = long.Parse(sr.ReadLine());

//                for (int i = 0; i < N2; i++)
//                {
//                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
//                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
//                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
//                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
//                }
//            }

//            stream.Close();
//            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
//        }
//    }
//}


