using NeuroSky.ThinkGear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MindWaveReaderWPF
{
    /// <summary>
    ///  Class ThinkGearController helps control the BCI device
    /// </summary>
    public class ThingGearController
    {
        #region Mind Developer Tools (MDT) - ThinkGear SDK for .NET (Platform x86)

        private readonly Action<string> _logingAction;
        private readonly Action<ThinkGearData> _refreshUiAction;

        /// <summary>
        /// Connector object for MindWave Mobile 2 device
        /// </summary>
        public Connector TgConnector { get; set; }

        /// <summary>
        /// Queue with collected data from BCI
        /// </summary>
        public Queue<ThinkGearData> TgQueue { get; set; }

        /// <summary>
        /// Latest ThinkGearData row from BCI
        /// </summary>
        public ThinkGearData TgLatestData { get; set; }

        /// <summary>
        /// Constructor with parameters od ThinkGearController class
        /// </summary>
        /// <param name="logger">type Action delegate - points to controling methods of logger for output information</param>
        /// <param name="refreshUi">Action delegate - points to method for refresh the UI state of application</param>
        public ThingGearController(Action<string> logger, Action<ThinkGearData> refreshUi)
        {
            _logingAction = logger;
            _refreshUiAction = refreshUi;
            TgQueue = new Queue<ThinkGearData>();
            TgLatestData = new ThinkGearData();
        }

        /// <summary>
        /// Method to connect BCI to application
        /// </summary>
        /// <param name="portName">name of the port to first try to connect</param>
        public void ThingGearConnect(string portName)
        {
            TgConnector = new Connector();
            TgConnector.DeviceFound += TGConnector_DeviceFound;
            TgConnector.DeviceNotFound += TGConnector_DeviceNotFound;
            TgConnector.DeviceValidating += TGConnector_DeviceValidating;
            TgConnector.DeviceConnected += TGConnector_DeviceConnected;
            TgConnector.DeviceConnectFail += TGConnector_DeviceConnectFail;
            TgConnector.DeviceDisconnected += TGConnector_DeviceDisconnected;
            TgConnector.setBlinkDetectionEnabled(true);
            TgConnector.setMentalEffortEnable(true);
            TgConnector.setTaskFamiliarityEnable(true);

            // Scan for devices across COM ports
            // The COM port named will be the first COM port that is checked.
            TgConnector.ConnectScan(portName);
        }

        /// <summary>
        /// Method to disconnect BCI from application
        /// </summary>
        public void ThingGearDisconnect()
        {
            if (TgConnector != null)
            {
                TgConnector.Close();
                TgConnector.Disconnect();
            }
            TgConnector = null;
        }

        private void TGConnector_DeviceFound(object sender, EventArgs e)
        {
            var devArgs = (Connector.DeviceEventArgs)e;
            _logingAction($"Device ID: {devArgs.Device.HeadsetID}");
        }

        private static void TGConnector_DeviceNotFound(object sender, EventArgs e)
        {
            MessageBox.Show("Device not found!");
        }

        private void TGConnector_DeviceValidating(object sender, EventArgs e)
        {
            var connectionArgs = (Connector.ConnectionEventArgs)e;
            _logingAction($"{connectionArgs?.Connection?.PortName ?? ""} Connecting ...");
        }

        private void TGConnector_DeviceConnected(object sender, EventArgs e)
        {
            var devArgs = (Connector.DeviceEventArgs)e;
            _logingAction("Device connected!");
            if (devArgs?.Device == null) return;

            _logingAction($"ID: {devArgs.Device.HeadsetID}");
            _logingAction($"PortName: { devArgs.Device.PortName}");
            _logingAction($"Data Received Rate: { devArgs.Device.DataReceivedRate}");
            _logingAction($"TimeStamp: { devArgs.Device.lastUpdate:yyyy/MM/dd hh:mm:ss}");

            devArgs.Device.DataReceived += TGConnector_DataReceived;
        }

        private void TGConnector_DeviceConnectFail(object sender, EventArgs e)
        {
            _logingAction("Device connection Failed!");
        }

        private void TGConnector_DeviceDisconnected(object sender, EventArgs e)
        {
            _logingAction("DEVICE DISCONNECTED.");
        }

        private void TGConnector_DataReceived(object sender, EventArgs e)
        {
            var tgParser = new TGParser();
            tgParser.Read((e as Device.DataEventArgs)?.DataRowArray);

            var tgDataRow = new ThinkGearData();
            var tgDataProperties = typeof(ThinkGearData).GetProperties();

            foreach (var data in tgParser.ParsedData)
            {
                foreach (var element in data)
                {
                    var propertyInfo = tgDataProperties.FirstOrDefault(prop => prop.Name == element.Key);
                    if (propertyInfo != null) propertyInfo.SetValue(tgDataRow, element.Value);

                    if (element.Key == "MSG_MODEL_IDENTIFIED")
                    {
                        _logingAction("MSG_MODEL_IDENTIFIED Recevied...");
                        TgConnector.setMentalEffortRunContinuous(true);
                        TgConnector.setMentalEffortEnable(true);
                        TgConnector.setTaskFamiliarityRunContinuous(true);
                        TgConnector.setTaskFamiliarityEnable(true);
                        TgConnector.setPositivityEnable(false);
                        TgConnector.setBlinkDetectionEnabled(true);
                    }
                }

                if (data.ContainsKey("MSG_ERR_CFG_OVERRIDE"))
                {
                    _logingAction($"ErrorConfigurationOverride: {data["MSG_ERR_CFG_OVERRIDE"]} Recevied...");
                }
                if (data.ContainsKey("MSG_ERR_NOT_PROVISIONED"))
                {
                    _logingAction($"ErrorModuleNotProvisioned: {data["MSG_ERR_NOT_PROVISIONED"]} Recevied...");
                }
            }
            
            if (Validators.CheckIfThereIsDataFlow(tgDataRow))
            {
               TgQueue.Enqueue(tgDataRow);
            }

            _refreshUiAction(tgDataRow);
        }

        #endregion
    }
}
