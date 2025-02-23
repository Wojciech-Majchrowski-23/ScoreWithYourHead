using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MindWaveReaderWPF
{
    /// <summary>
    /// Class with data structure of BCI informations
    /// </summary>
    public class ThinkGearData
    {
        /// <summary>
        /// Time Stamp
        /// </summary>
        [DisplayName("Time Stamp")]
        [DisplayFormat(DataFormatString = "yyyy/MM/dd HH:mm:ss")]
        public DateTime TimeStampInfo { get; set; }

        /// <summary>
        /// PoorSignal : ThinkGearAPI is crazy with this value! If value is 0 signal is OK, otherwise no
        /// The value ranges from 0 to 200, the higher value, the worse signal
        /// </summary>
        [DisplayName("Signal Strength %")]
        public double PoorSignal { get; set; }

        /// <summary>
        /// Delta Power
        /// </summary>
        [DisplayName("Eeg Power Delta")]
        public double EegPowerDelta { get; set; }

        /// <summary>
        /// Theta Power
        /// </summary>
        [DisplayName("Eeg Power Theta")]
        public double EegPowerTheta { get; set; }

        /// <summary>
        /// Low Alpha Power
        /// </summary>
        [DisplayName("Eeg Power Low Alpha")]
        public double EegPowerAlpha1 { get; set; }

        /// <summary>
        /// High Alpha Power
        /// </summary>
        [DisplayName("Eeg Power High Alpha")]
        public double EegPowerAlpha2 { get; set; }

        /// <summary>
        /// Low Beta Power
        /// </summary>
        [DisplayName("Eeg Power Low Beta")]
        public double EegPowerBeta1 { get; set; }

        /// <summary>
        /// High Beta Power
        /// </summary>
        [DisplayName("Eeg Power High Beta")]
        public double EegPowerBeta2 { get; set; }

        /// <summary>
        /// Low Gamma Power
        /// </summary>
        [DisplayName("Eeg Power Low Gamma")]
        public double EegPowerGamma1 { get; set; }

        /// <summary>
        /// High Gamma Power
        /// </summary>
        [DisplayName("Eeg Power High Gamma")]
        public double EegPowerGamma2 { get; set; }

        /// <summary>
        /// Attention: The Attention Meter algorithm indicates the intensity of mental “focus” or “attention.” 
        /// The value ranges from 0 to 100.
        /// </summary>
        [DisplayName("Attention Level")]
        public double Attention { get; set; }
        /// <summary>
        /// Meditation:  The Meditation Meter algorithm indicates the level of mental “calmness” or “relaxation.” 
        /// The value ranges from 0 to 100, and increases when users relax the mind and decreases when they are uneasy or stressed. 
        /// </summary>
        [DisplayName("Meditation Level")]
        public double Meditation { get; set; }

        /// <summary>
        /// BlinkStrength: Its value ranges from 1 to 255 and it is reported whenever an eye blink is detected.
        /// </summary>
        [DisplayName("Blink Strength")]
        public double BlinkStrength { get; set; }

        /// <summary>
        /// The Mental Effort algorithm measures the mental workload while performing a task. 
        /// The harder a user’s brain works on a task, the higher the value.
        /// Mental Effort (BETA)
        /// Mental Effort measures how hard the subject’s brain is working, 
        /// i.e.the amount of workload involved in the task. 
        /// Mental Effort algorithm can be used for both within-trial monitoring 
        /// (continuous real - time tracking) and between - trial comparison.
        /// A trial can be of any length equal to or more than 1 minute.
        /// In each trial, the first output index will be given out after the 
        /// first minute and new output indexes will then be generated at time 
        /// interval defined by the output rate(default: 10s).
        /// </summary>
        [DisplayName("Mental Effort")]
        public double MentalEffort { get; set; }

        /// <summary>
        /// The Familiarity algorithm tracks learning processes to measure the relative level of understanding, learning, or comfort with a task
        /// Familiarity (BETA)
        /// Familiarity measures how well the subject is learning a new task or how 
        /// well his performance is with certain task. 
        /// Familiarity algorithm can be used for both within-trial monitoring 
        /// (continuous real-time tracking) and between-trial comparison. 
        /// A trial can be of any length equal to or more than 1 minute. In each trial, 
        /// the first output index will be given out after the first minute and new output 
        /// indexes will then be generated at time interval defined by the output rate (default: 10s).
        /// </summary>
        [DisplayName("Task Familiarity")]
        public double TaskFamiliarity { get; set; }

        /// <summary>
        /// Connection estabilishment and error flags
        /// </summary>
        public double MSG_MODEL_IDENTIFIED { get; set; }
        /// <summary>
        /// Connection estabilishment and error flags
        /// </summary>
        public double MSG_ERR_CFG_OVERRIDE { get; set; }
        /// <summary>
        /// Connection estabilishment and error flags
        /// </summary>
        public double MSG_ERR_NOT_PROVISIONED { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ThinkGearData()
        {
            TimeStampInfo = DateTime.Now;
            PoorSignal = -1;
            Attention = -1;
            Meditation = -1;
            BlinkStrength = -1;
            MentalEffort = 0;
            TaskFamiliarity = 0;
        }

        /// <summary>
        /// ToString override method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Signal: " + Converters.PoorSignalTranscoder(PoorSignal) + "%");
            stringBuilder.AppendLine("TimeStamp: " + TimeStampInfo);
            stringBuilder.AppendLine("EegPowerDelta: " + EegPowerDelta);
            stringBuilder.AppendLine("EegPowerTheta: " + EegPowerTheta);
            stringBuilder.AppendLine("EegPowerAlpha1: " + EegPowerAlpha1);
            stringBuilder.AppendLine("EegPowerAlpha2: " + EegPowerAlpha2);
            stringBuilder.AppendLine("EegPowerBeta1: " + EegPowerBeta1);
            stringBuilder.AppendLine("EegPowerBeta2: " + EegPowerBeta2);
            stringBuilder.AppendLine("EegPowerGamma1: " + EegPowerGamma1);
            stringBuilder.AppendLine("EegPowerGamma2: " + EegPowerGamma2);
            stringBuilder.AppendLine("Attention Lvl (%): " + Attention);
            stringBuilder.AppendLine("Meditation Lvl (%): " + Meditation);
            stringBuilder.AppendLine("BlinkStrength: " + BlinkStrength);
            stringBuilder.AppendLine(Properties.Resources.ValuesSpacer);
            stringBuilder.AppendLine("MentalEffort: " + MentalEffort);
            stringBuilder.AppendLine("TaskFamiliarity: " + TaskFamiliarity);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Generate Csv File Header for export
        /// </summary>
        /// <returns>Header for CSV file</returns>
        public static string GenerateCsvHeader()
        {
            return string.Join(";", TypeDescriptor.GetProperties(typeof(ThinkGearData))
                .Cast<PropertyDescriptor>().Where(prop => !prop.Name.Contains("MSG"))
                .Select(prop => prop.DisplayName));
        }

        /// <summary>
        /// Generate row of data for CSV file export
        /// </summary>
        /// <returns>Row of data for CSV export file</returns>
        public static string ThinkGearDataToCsvRow(ThinkGearData data)
        {
            var valuesRow = new List<string>
            {
                data.TimeStampInfo.ToString(Properties.Settings.Default.DateTimeFormat, CultureInfo.CurrentCulture),
                Converters.PoorSignalTranscoder(data.PoorSignal).ToString(),
                data.EegPowerDelta.ToString("F"),
                data.EegPowerTheta.ToString("F"),
                data.EegPowerAlpha1.ToString("F"),
                data.EegPowerAlpha2.ToString("F"),
                data.EegPowerBeta1.ToString("F"),
                data.EegPowerBeta2.ToString("F"),
                data.EegPowerGamma1.ToString("F"),
                data.EegPowerGamma2.ToString("F"),
                data.Attention.ToString("F"),
                data.Meditation.ToString("F"),
                data.BlinkStrength.ToString("F"),
                data.MentalEffort.ToString("F"),
                data.TaskFamiliarity.ToString("F")
            };

            return string.Join(";", valuesRow);
        }
    }
}
