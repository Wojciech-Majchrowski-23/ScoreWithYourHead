using System.Collections.Generic;
using System.Linq;

namespace MindWaveReaderWPF
{
    /// <summary>
    /// Class with converters for data from BCI
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Gets raw value from PoorSignal and returns percentage of signal quality
        /// </summary>
        /// <param name="poorSignalValue">poorSignal raw value</param>
        /// <returns>Percentage of signal quality: 100% means the best signal quality </returns>
        public static int PoorSignalTranscoder(double poorSignalValue)
        {
            return (int) (-0.5*poorSignalValue + 100);
        }

        /// <summary>
        /// Important: Applicable only to saving data to file due to empty rows with the same timestamp - BCI sends data to queue asynchronously
        /// Method reduces amount of junk data in collection and merge records with the same timestamp
        /// </summary>
        /// <param name="tgQueue">input data queue</param>
        /// <returns>new list with merged records</returns>
        public static List<ThinkGearData> TgQueueDataMerge(Queue<ThinkGearData> tgQueue)
        {
            var mergedData = new List<ThinkGearData>();
            var groupedRecords = tgQueue.GroupBy(x => x.TimeStampInfo.ToString(Properties.Settings.Default.DateTimeFormat));

            foreach (var record in groupedRecords)
            {
                if (record.Count() > 1)
                {
                    var mergedTgData = new ThinkGearData();
                    mergedTgData = record.Aggregate(mergedTgData, (current, tgData) => MergeTgObjectsValues(tgData, current));
                    mergedData.Add(mergedTgData);
                }
                else
                {
                    mergedData.Add(record.FirstOrDefault());
                }
            }
            return new List<ThinkGearData>(mergedData.OrderBy(x => x.TimeStampInfo));
        }

        /// <summary>
        /// Method which merges two ThinkGearData objects into one. Merge strategy -  (mainly) bigger values the better
        /// Important: Applicable only to saving data to file due to empty rows with the same timestamp - BCI sends data asynchronously
        /// </summary>
        /// <param name="first">first comparable ThinkGearData object</param>
        /// <param name="second">second comparable ThinkGearData object</param>
        /// <returns>merged ThinkGearData object</returns>
        private static ThinkGearData MergeTgObjectsValues(ThinkGearData first, ThinkGearData second)
        {
            return new ThinkGearData
            {
                TimeStampInfo = first.TimeStampInfo,
                Attention = first.Attention > second.Attention ? first.Attention : second.Attention,
                Meditation = first.Meditation > second.Meditation ? first.Meditation : second.Meditation,
                BlinkStrength = first.BlinkStrength > second.BlinkStrength ? first.BlinkStrength : second.BlinkStrength,
                EegPowerDelta = first.EegPowerDelta > second.EegPowerDelta ? first.EegPowerDelta : second.EegPowerDelta,
                EegPowerTheta = first.EegPowerTheta > second.EegPowerTheta ? first.EegPowerTheta : second.EegPowerTheta,
                EegPowerAlpha1 = first.EegPowerAlpha1 > second.EegPowerAlpha1 ? first.EegPowerAlpha1 : second.EegPowerAlpha1,
                EegPowerAlpha2 = first.EegPowerAlpha2 > second.EegPowerAlpha2 ? first.EegPowerAlpha2 : second.EegPowerAlpha2,
                EegPowerBeta1 = first.EegPowerBeta1 > second.EegPowerBeta1 ? first.EegPowerBeta1 : second.EegPowerBeta1,
                EegPowerBeta2 = first.EegPowerBeta2 > second.EegPowerBeta2 ? first.EegPowerBeta2 : second.EegPowerBeta2,
                EegPowerGamma1 = first.EegPowerGamma1 > second.EegPowerGamma1 ? first.EegPowerGamma1 : second.EegPowerGamma1,
                EegPowerGamma2 = first.EegPowerGamma2 > second.EegPowerGamma2 ? first.EegPowerGamma2 : second.EegPowerGamma2,
                //special cases
                PoorSignal = first.PoorSignal < second.PoorSignal ? first.PoorSignal : second.PoorSignal,
                MentalEffort = (int)first.MentalEffort != 0 ? first.MentalEffort : second.MentalEffort,
                TaskFamiliarity = (int)first.TaskFamiliarity != 0 ? first.TaskFamiliarity : second.TaskFamiliarity
            };
        }
    }

    /// <summary>
    /// Class with Validators for some BCI data
    /// </summary>
    public static class Validators
    {
        /// <summary>
        /// Method checks if there is a gain on EEG waves
        /// </summary>
        /// <param name="data">Think Gear Data object</param>
        /// <returns>true if there is a signal, otherwise false</returns>
        public static bool CheckIfThereIsEEGSignal(ThinkGearData data)
        {
            return data.EegPowerAlpha1 > 0 || data.EegPowerAlpha2 > 0 || data.EegPowerDelta > 0 || data.EegPowerTheta > 0 
                || data.EegPowerBeta1 > 0 || data.EegPowerBeta2 > 0 || data.EegPowerGamma1 > 0 || data.EegPowerGamma2 > 0;
        }

        /// <summary>
        /// Method checks if data from BCI are valid to queue
        /// Interface is firing method with received data very often, but not always data are OK, so this validator
        /// </summary>
        /// <param name="data">Think Gear Data object</param>
        /// <returns>true if data are worth saving into queue, otherwise false</returns>
        public static bool CheckIfThereIsDataFlow(ThinkGearData data)
        {
            return CheckIfThereIsEEGSignal(data) || data.BlinkStrength > -1 || data.Attention > 0 || data.Meditation > 0
                   || (int)data.MentalEffort != 0 || (int)data.TaskFamiliarity != 0;
        }
    }
}
