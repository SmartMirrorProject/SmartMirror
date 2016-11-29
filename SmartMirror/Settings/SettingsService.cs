using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using SmartMirror.LocationModule;

namespace SmartMirror.Settings
{
    class SettingsService
    {
        private const string FileName = "smart_mirror_settings.txt";

        public static async Task<SettingsData> FetchSettingsData()
        {
            string text = await ReadStringFromFile();
            string[] textData = text.Split('\n');
            SettingsData data = new SettingsData()
            {
                MilitaryTime = bool.Parse(textData[0]),
                HomeAddress = new Address(textData[1]),
                WorkAddress = new Address(textData[2]),
                StartTime = textData[3]
            };
            return data;
        }

        public static async void Save(SettingsViewModel viewModel)
        {
            SettingsData data = ConvertViewModelToData(viewModel);
            string content = data.MilitaryTime.ToString() + "\n";
            content += data.HomeAddress.FullAddress + "\n";
            content += data.WorkAddress.FullAddress + "\n";
            content += data.StartTime;
            bool result = await SaveStringToFile(content);
        }

        private static SettingsData ConvertViewModelToData(SettingsViewModel vm)
        {
            return new SettingsData
            {
                MilitaryTime = vm.MilitaryTime,
                HomeAddress = new Address(vm.HomeCity, vm.HomeState, "US", vm.HomeZip) { StreetAddress = vm.HomeAddress },
                WorkAddress = new Address(vm.WorkCity, vm.WorkState, "US", vm.WorkZip) { StreetAddress = vm.WorkAddress },
                StartTime = vm.WorkStartTime
            };
        }

        private static async Task<bool> SaveStringToFile(string content)
        {
            try
            {
                // saves the string 'content' to a file 'filename' in the app's local storage folder
                byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(content.ToCharArray());

                // create a file with the given filename in the local folder; replace any existing file with the same name
                StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.
                    CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);

                // write the char array created from the content string into the file
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    stream.Write(fileBytes, 0, fileBytes.Length);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static async Task<string> ReadStringFromFile()
        {
            // access the local folder
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            // open the file 'filename' for reading
            Stream stream = await local.OpenStreamForReadAsync(FileName);
            string text;

            // copy the file contents into the string 'text'
            using (StreamReader reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }
    }
}
