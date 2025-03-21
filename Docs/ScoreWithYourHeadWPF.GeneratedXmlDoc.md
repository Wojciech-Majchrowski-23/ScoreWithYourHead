# ScoreWithYourHeadWPF #

## Type MindWaveReaderWPF.App

 Interaction logic for App.xaml 

 App 



---
#### Method MindWaveReaderWPF.App.OnStartup(System.Windows.StartupEventArgs)

 Boilerplate summary OnStartup method 

|Name | Description |
|-----|------|
|e: |StartupEventArgs e|


---
#### Method MindWaveReaderWPF.App.OnExit(System.Windows.ExitEventArgs)

 Boilerplate summary OnExit method 

|Name | Description |
|-----|------|
|e: |ExitEventArgs e|


---
#### Method MindWaveReaderWPF.App.InitializeComponent

 InitializeComponent 



---
#### Method MindWaveReaderWPF.App.Main

 Application Entry Point. 



---
## Type MindWaveReaderWPF.ThingGearController

 Class ThinkGearController helps control the BCI device 



---
#### Property MindWaveReaderWPF.ThingGearController.TgConnector

 Connector object for MindWave Mobile 2 device 



---
#### Property MindWaveReaderWPF.ThingGearController.TgQueue

 Queue with collected data from BCI 



---
#### Property MindWaveReaderWPF.ThingGearController.TgLatestData

 Latest ThinkGearData row from BCI 



---
#### Method MindWaveReaderWPF.ThingGearController.#ctor(System.Action{System.String},System.Action{MindWaveReaderWPF.ThinkGearData})

 Constructor with parameters od ThinkGearController class 

|Name | Description |
|-----|------|
|logger: |type Action delegate - points to controling methods of logger for output information|
|refreshUi: |Action delegate - points to method for refresh the UI state of application|


---
#### Method MindWaveReaderWPF.ThingGearController.ThingGearConnect(System.String)

 Method to connect BCI to application 

|Name | Description |
|-----|------|
|portName: |name of the port to first try to connect|


---
#### Method MindWaveReaderWPF.ThingGearController.ThingGearDisconnect

 Method to disconnect BCI from application 



---
## Type MindWaveReaderWPF.ThinkGearData

 Class with data structure of BCI informations 



---
#### Property MindWaveReaderWPF.ThinkGearData.TimeStampInfo

 Time Stamp 



---
#### Property MindWaveReaderWPF.ThinkGearData.PoorSignal

 PoorSignal : ThinkGearAPI is crazy with this value! If value is 0 signal is OK, otherwise no The value ranges from 0 to 200, the higher value, the worse signal 



---
#### Property MindWaveReaderWPF.ThinkGearData.EegPowerDelta

 Delta Power 



---
#### Property MindWaveReaderWPF.ThinkGearData.EegPowerTheta

 Theta Power 



---
#### Property MindWaveReaderWPF.ThinkGearData.EegPowerAlpha1

 Low Alpha Power 



---
#### Property MindWaveReaderWPF.ThinkGearData.EegPowerAlpha2

 High Alpha Power 



---
#### Property MindWaveReaderWPF.ThinkGearData.EegPowerBeta1

 Low Beta Power 



---
#### Property MindWaveReaderWPF.ThinkGearData.EegPowerBeta2

 High Beta Power 



---
#### Property MindWaveReaderWPF.ThinkGearData.EegPowerGamma1

 Low Gamma Power 



---
#### Property MindWaveReaderWPF.ThinkGearData.EegPowerGamma2

 High Gamma Power 



---
#### Property MindWaveReaderWPF.ThinkGearData.Attention

 Attention: The Attention Meter algorithm indicates the intensity of mental “focus” or “attention.” The value ranges from 0 to 100. 



---
#### Property MindWaveReaderWPF.ThinkGearData.Meditation

 Meditation: The Meditation Meter algorithm indicates the level of mental “calmness” or “relaxation.” The value ranges from 0 to 100, and increases when users relax the mind and decreases when they are uneasy or stressed. 



---
#### Property MindWaveReaderWPF.ThinkGearData.BlinkStrength

 BlinkStrength: Its value ranges from 1 to 255 and it is reported whenever an eye blink is detected. 



---
#### Property MindWaveReaderWPF.ThinkGearData.MentalEffort

 The Mental Effort algorithm measures the mental workload while performing a task. The harder a user’s brain works on a task, the higher the value. Mental Effort (BETA) Mental Effort measures how hard the subject’s brain is working, i.e.the amount of workload involved in the task. Mental Effort algorithm can be used for both within-trial monitoring (continuous real - time tracking) and between - trial comparison. A trial can be of any length equal to or more than 1 minute. In each trial, the first output index will be given out after the first minute and new output indexes will then be generated at time interval defined by the output rate(default: 10s). 



---
#### Property MindWaveReaderWPF.ThinkGearData.TaskFamiliarity

 The Familiarity algorithm tracks learning processes to measure the relative level of understanding, learning, or comfort with a task Familiarity (BETA) Familiarity measures how well the subject is learning a new task or how well his performance is with certain task. Familiarity algorithm can be used for both within-trial monitoring (continuous real-time tracking) and between-trial comparison. A trial can be of any length equal to or more than 1 minute. In each trial, the first output index will be given out after the first minute and new output indexes will then be generated at time interval defined by the output rate (default: 10s). 



---
#### Property MindWaveReaderWPF.ThinkGearData.MSG_MODEL_IDENTIFIED

 Connection estabilishment and error flags 



---
#### Property MindWaveReaderWPF.ThinkGearData.MSG_ERR_CFG_OVERRIDE

 Connection estabilishment and error flags 



---
#### Property MindWaveReaderWPF.ThinkGearData.MSG_ERR_NOT_PROVISIONED

 Connection estabilishment and error flags 



---
#### Method MindWaveReaderWPF.ThinkGearData.#ctor

 Default constructor 



---
#### Method MindWaveReaderWPF.ThinkGearData.ToString

 ToString override method 

**Returns**: 



---
#### Method MindWaveReaderWPF.ThinkGearData.GenerateCsvHeader

 Generate Csv File Header for export 

**Returns**: Header for CSV file



---
#### Method MindWaveReaderWPF.ThinkGearData.ThinkGearDataToCsvRow(MindWaveReaderWPF.ThinkGearData)

 Generate row of data for CSV file export 

**Returns**: Row of data for CSV export file



---
## Type MindWaveReaderWPF.Converters

 Class with converters for data from BCI 



---
#### Method MindWaveReaderWPF.Converters.PoorSignalTranscoder(System.Double)

 Gets raw value from PoorSignal and returns percentage of signal quality 

|Name | Description |
|-----|------|
|poorSignalValue: |poorSignal raw value|
**Returns**: Percentage of signal quality: 100% means the best signal quality 



---
#### Method MindWaveReaderWPF.Converters.TgQueueDataMerge(System.Collections.Generic.Queue{MindWaveReaderWPF.ThinkGearData})

 Important: Applicable only to saving data to file due to empty rows with the same timestamp - BCI sends data to queue asynchronously Method reduces amount of junk data in collection and merge records with the same timestamp 

|Name | Description |
|-----|------|
|tgQueue: |input data queue|
**Returns**: new list with merged records



---
#### Method MindWaveReaderWPF.Converters.MergeTgObjectsValues(MindWaveReaderWPF.ThinkGearData,MindWaveReaderWPF.ThinkGearData)

 Method which merges two ThinkGearData objects into one. Merge strategy - (mainly) bigger values the better Important: Applicable only to saving data to file due to empty rows with the same timestamp - BCI sends data asynchronously 

|Name | Description |
|-----|------|
|first: |first comparable ThinkGearData object|
|second: |second comparable ThinkGearData object|
**Returns**: merged ThinkGearData object



---
## Type MindWaveReaderWPF.Validators

 Class with Validators for some BCI data 



---
#### Method MindWaveReaderWPF.Validators.CheckIfThereIsEEGSignal(MindWaveReaderWPF.ThinkGearData)

 Method checks if there is a gain on EEG waves 

|Name | Description |
|-----|------|
|data: |Think Gear Data object|
**Returns**: true if there is a signal, otherwise false



---
#### Method MindWaveReaderWPF.Validators.CheckIfThereIsDataFlow(MindWaveReaderWPF.ThinkGearData)

 Method checks if data from BCI are valid to queue Interface is firing method with received data very often, but not always data are OK, so this validator 

|Name | Description |
|-----|------|
|data: |Think Gear Data object|
**Returns**: true if data are worth saving into queue, otherwise false



---
## Type MindWaveReaderWPF.WindowMain

 WindowMain 



---
#### Method MindWaveReaderWPF.WindowMain.CSVexport_Click(System.Object,System.Windows.RoutedEventArgs)

 Saving collected records to CSV file - click button event handler 

|Name | Description |
|-----|------|
|sender: ||
|e: ||


---
#### Method MindWaveReaderWPF.WindowMain.InitializeComponent

 InitializeComponent 



---
## Type MindWaveReaderWPF.Properties.Resources

 Klasa zasobu wymagająca zdefiniowania typu do wyszukiwania zlokalizowanych ciągów itd. 



---
#### Property MindWaveReaderWPF.Properties.Resources.ResourceManager

 Zwraca buforowane wystąpienie ResourceManager używane przez tę klasę. 



---
#### Property MindWaveReaderWPF.Properties.Resources.Culture

 Przesłania właściwość CurrentUICulture bieżącego wątku dla wszystkich przypadków przeszukiwania zasobów za pomocą tej klasy zasobów wymagającej zdefiniowania typu. 



---
#### Property MindWaveReaderWPF.Properties.Resources.Data

 Wyszukuje zlokalizowany ciąg podobny do ciągu Data: . 



---
#### Property MindWaveReaderWPF.Properties.Resources.FileTitle

 Wyszukuje zlokalizowany ciąg podobny do ciągu title. 



---
#### Property MindWaveReaderWPF.Properties.Resources.MentalEffort

 Wyszukuje zlokalizowany ciąg podobny do ciągu Mental effort: . 



---
#### Property MindWaveReaderWPF.Properties.Resources.SavingFailed

 Wyszukuje zlokalizowany ciąg podobny do ciągu Saving to .csv file failed. 



---
#### Property MindWaveReaderWPF.Properties.Resources.SavingFileNotPossible

 Wyszukuje zlokalizowany ciąg podobny do ciągu Saving file not possible due to small amount or lack of collected data.. 



---
#### Property MindWaveReaderWPF.Properties.Resources.SavingSuccessfull

 Wyszukuje zlokalizowany ciąg podobny do ciągu Saving to .csv file completed successfully. 



---
#### Property MindWaveReaderWPF.Properties.Resources.TaskFamiliarity

 Wyszukuje zlokalizowany ciąg podobny do ciągu Task familiarity: . 



---
#### Property MindWaveReaderWPF.Properties.Resources.TryConnectMindWave

 Wyszukuje zlokalizowany ciąg podobny do ciągu Try connect MindWave Mobile and record some data. 



---
#### Property MindWaveReaderWPF.Properties.Resources.ValuesSpacer

 Wyszukuje zlokalizowany ciąg podobny do ciągu ----Values computed after a few seconds: -----. 



---


