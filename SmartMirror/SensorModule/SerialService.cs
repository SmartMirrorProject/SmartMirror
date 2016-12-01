using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace SmartMirror.SensorModule
{
    class SerialService
    {
        private static SerialDevice serialPort;
        //private DataWriter dataWriter = null;
        private static DataReader dataReader;

        //private ObservableCollection<DeviceInformation> listOfDevices;
        private static CancellationTokenSource readCancellationTokenSource;

        private static SensorModel sensorModel;

        public static async Task<bool> InitSerialService(SensorModel model)
        {
            sensorModel = model;
            try
            {
                //There should only be one serial device available. Select it.
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);
                DeviceInformation entry = dis[0];
                serialPort = await SerialDevice.FromIdAsync(entry.Id);

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 9600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.None;

//                // Display configured settings
//                string status = "Serial port configured successfully: ";
//                status += serialPort.BaudRate + "-";
//                status += serialPort.DataBits + "-";
//                status += serialPort.Parity.ToString() + "-";
//                status += serialPort.StopBits;

                // Create cancellation token object to close I/O operations when closing the device
                readCancellationTokenSource = new CancellationTokenSource();

                Listen();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static async Task<bool> InitSerialServiceNew(SensorModel model)
        {
            sensorModel = model;
            try
            {
                while (null == serialPort)
                {
                    await InitSerialPort();
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }

                // Configure serial settings
                serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                serialPort.BaudRate = 9600;
                serialPort.Parity = SerialParity.None;
                serialPort.StopBits = SerialStopBitCount.One;
                serialPort.DataBits = 8;
                serialPort.Handshake = SerialHandshake.None;

                //                // Display configured settings
                //                string status = "Serial port configured successfully: ";
                //                status += serialPort.BaudRate + "-";
                //                status += serialPort.DataBits + "-";
                //                status += serialPort.Parity.ToString() + "-";
                //                status += serialPort.StopBits;

                // Create cancellation token object to close I/O operations when closing the device
                readCancellationTokenSource = new CancellationTokenSource();

                NewListen();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static async Task<bool> InitSerialPort()
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);
                DeviceInformation entry = dis[0];
                serialPort = await SerialDevice.FromIdAsync(entry.Id);
            }
            catch (Exception e)
            {
                //TODO, this is not ideal. Will be fixed at some point. JG - 12/1/2016
                return false;
            }
            return true;
        }

    /// <summary>
    /// - Create a DataReader object
    /// - Create an async task to read from the SerialDevice InputStream
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static async void Listen()
        {
            try
            {
                if (serialPort != null)
                {

                    dataReader = new DataReader(serialPort.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        string text = await ReadAsync(readCancellationTokenSource.Token);                        
                        ParseData(text);
                    }
                }
            }
            catch (Exception ex)
            {
                CloseDevice();
            }
            finally
            {
                // Cleanup once complete
                if (dataReader != null)
                {
                    dataReader.DetachStream();
                    dataReader = null;
                }
            }
        }

        /// <summary>
        /// - Create a DataReader object
        /// - Create an async task to read from the SerialDevice InputStream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static async void NewListen()
        {
            try
            {
                if (serialPort != null)
                {

                    dataReader = new DataReader(serialPort.InputStream);

                    // Always read the serial input. If it fails, try to reopen it.
                    while (true)
                    {
                        try
                        {
                            if (null == serialPort)
                            {
                                await Task.Delay(TimeSpan.FromSeconds(15));
                                await InitSerialPort();
                            }
                            else
                            {
                                string text = await ReadAsync(readCancellationTokenSource.Token);
                                ParseData(text);
                            }
                        }
                        catch (Exception e)
                        {
                            serialPort = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CloseDevice();
            }
            finally
            {
                // Cleanup once complete
                if (dataReader != null)
                {
                    dataReader.DetachStream();
                    dataReader = null;
                }
            }
        }

        private static async void ParseData(String data)
        {
            string[] reads = data.Split(' ');
            for (int i = 0; i < reads.Length; i++)
            {
                string[] dataList = reads[0].Trim().Split(',');
                SensorData dataObj = new SensorData
                {
                    Temperature = int.Parse(dataList[0].Trim()),
                    Humidity = int.Parse(dataList[1].Trim()),
                    LightSensor = int.Parse(dataList[2].Trim()),
                    LongRangeMotion = int.Parse(dataList[3].Trim()),
                    GestureMotion = int.Parse(dataList[4].Trim())
                };
                sensorModel.EnqueueData(dataObj);
            }
        }

        /// <summary>
        /// ReadAsync: Task that waits on data and reads asynchronously from the serial device InputStream
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task<string> ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReader.InputStreamOptions = InputStreamOptions.Partial;            

            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = dataReader.LoadAsync(1024).AsTask(cancellationToken);

            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                string receivedData = dataReader.ReadString(bytesRead);
                Debug.WriteLine(receivedData);
                return receivedData;
            }

            return "";
        }

        /// <summary>
        /// CancelReadTask:
        /// - Uses the ReadCancellationTokenSource to cancel read operations
        /// </summary>
        private static void CancelReadTask()
        {
            if (readCancellationTokenSource != null)
            {
                if (!readCancellationTokenSource.IsCancellationRequested)
                {
                    readCancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// CloseDevice:
        /// - Disposes SerialDevice object
        /// - Clears the enumerated device Id list
        /// </summary>
        private static void CloseDevice()
        {
            serialPort?.Dispose();
            serialPort = null;            
        }

//Currently not writing back to uno but if we need to, this will do it.
//        /// <summary>
//        /// WriteAsync: Task that asynchronously writes data from the input text box 'sendText' to the OutputStream 
//        /// </summary>
//        /// <returns></returns>
//        private async Task WriteAsync()
//        {
//            Task<UInt32> storeAsyncTask;
//
//            if (sendText.Text.Length != 0)
//            {
//                // Load the text from the sendText input text box to the dataWriter object
//                dataWriteObject.WriteString(sendText.Text);
//
//                // Launch an async task to complete the write operation
//                storeAsyncTask = dataWriteObject.StoreAsync().AsTask();
//
//                UInt32 bytesWritten = await storeAsyncTask;
//                if (bytesWritten > 0)
//                {
//                    status.Text = sendText.Text + ", ";
//                    status.Text += "bytes written successfully!";
//                }
//                sendText.Text = "";
//            }
//            else
//            {
//                status.Text = "Enter the text you want to write and then click on 'WRITE'";
//            }
//        }
    }
}
