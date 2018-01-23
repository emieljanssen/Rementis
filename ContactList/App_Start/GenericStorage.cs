using ContactList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContactList
{
    public class GenericStorage
    {
        private string _filePath;

        public GenericStorage()
        {
            var webAppsHome = Environment.GetEnvironmentVariable("HOME")?.ToString();
            if (String.IsNullOrEmpty(webAppsHome))
            {
                _filePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + "\\";
            }
            else
            {
                _filePath = webAppsHome + "\\site\\wwwroot\\";
            }
        }

        public async Task SaveSensorData(IEnumerable<Sensordata> target, string filename)
        {
            var json = JsonConvert.SerializeObject(target);
            File.WriteAllText(_filePath + filename, json);
        }

        public async Task<IEnumerable<Sensordata>> GetSensordata(string filename)
        {
            var sensordatasText = String.Empty;
            if (File.Exists(_filePath + filename))
            {
                sensordatasText = File.ReadAllText(_filePath + filename);
            }

            var sensordatas = JsonConvert.DeserializeObject<Sensordata[]>(sensordatasText);
            return sensordatas;
        }

        public async Task SaveAgendaData(IEnumerable<Agendadata> target, string filename)
        {
            var json = JsonConvert.SerializeObject(target);
            File.WriteAllText(_filePath + filename, json);
        }

        public async Task<IEnumerable<Agendadata>> GetAgenda(string filename)
        {
            var agendadataText = String.Empty;
            if (File.Exists(_filePath + filename))
            {
                agendadataText = File.ReadAllText(_filePath + filename);
            }

            var agenda = JsonConvert.DeserializeObject<Agendadata[]>(agendadataText);
            return agenda;
        }
    }
}
