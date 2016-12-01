using System.Collections;
using System.Collections.Generic;
using SmartMirror.MainModule;

namespace SmartMirror.SensorModule
{
    public class SensorModel
    {
        private readonly SensorViewModel viewModel;

        private readonly Queue<SensorData> dataQueue;

        private readonly SmartMirrorModel smartMirror;

        private int motionCycleCount;

        public SensorModel(SensorViewModel vm, SmartMirrorModel model)
        {
            viewModel = vm;
            smartMirror = model;
            dataQueue = new Queue<SensorData>();
            motionCycleCount = 0;
            viewModel.Temperature = "";
            viewModel.Humidity = "";
            viewModel.LightSensor = "There is no data from the MCU.";
            viewModel.LongRangeSensor = "";
            viewModel.GestureControl = "";
        }

        public void InitSensorModel()
        {
            ProcessSensorData(true);
        }

        public void ProcessSensorData(bool sensorDisplayOn)
        {

            if (dataQueue.Count > 0)
            {
                SensorData data = dataQueue.Dequeue();
                smartMirror.HandleMotionSensorData(data.LongRangeMotion);
                smartMirror.HandleGesture(data.GestureMotion);
                if (data.LongRangeMotion == 0)
                {
                    motionCycleCount++;
                }
                //If sensor display is on, we need to update the view model
                if (sensorDisplayOn)
                {
                    viewModel.Temperature = "Temperature: " + data.Temperature  + "°C";
                    viewModel.Humidity = "Humidity: " + data.Humidity + "%";
                    if (data.LightSensor == 0)
                    {
                        viewModel.LightSensor = "Light On";
                    }
                    else
                    {
                        viewModel.LightSensor = "Light Off";
                    }
                    if (motionCycleCount > 240)
                    {
                        viewModel.LongRangeSensor = "No Motion Detected";
                    }
                    else
                    {
                        viewModel.LongRangeSensor = "Motion Detected";
                    }
                    switch (data.GestureMotion)
                    {
                        case 0:
                            viewModel.GestureControl = "No Gesture";
                            break;
                        case 1:
                            viewModel.GestureControl = "Swipe Down";
                            break;
                        case 2:
                            viewModel.GestureControl = "Swipe Up";
                            break;
                        case 3:
                            viewModel.GestureControl = "Hold Top";
                            break;
                        case 4:
                            viewModel.GestureControl = "Hold Bottom";
                            break;
                        default:
                            viewModel.GestureControl = "No Gesture";
                            break;
                    }
                }
            }
        }

        public void EnqueueData(SensorData data)
        {
            dataQueue.Enqueue(data);
            if (dataQueue.Count > 5)
            {
                ClearFromQueue();
            }
        }

        private void ClearFromQueue()
        {
            while (dataQueue.Peek().GestureMotion == 0 && dataQueue.Count > 3)
            {
                dataQueue.Dequeue();
            }
        }
    }
}
