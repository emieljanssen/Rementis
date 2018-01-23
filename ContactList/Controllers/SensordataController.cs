using ContactList.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Net.Http;

namespace ContactList.Controllers
{
    public class SensordatasController : ApiController
    {
        private const string FILENAME = "sensordatas.json";
        private GenericStorage _storage;


        public SensordatasController()
        {
            _storage = new GenericStorage();
        }

        private async Task<IEnumerable<Sensordata>> GetSensordatas()
        {
            var sensordatas = await _storage.GetSensordata(FILENAME);

            if (sensordatas == null)
            {
                await _storage.SaveSensorData(new Sensordata[]{
                        new Sensordata { Id = 1, SensorId = "Deursensor", KlantId = "Frank", Value = "prioriteit", Timestamp = "10:00:00", Active = true},
                        new Sensordata { Id = 2, SensorId = "Deursensor", KlantId = "Frank", Value = "prioriteit", Timestamp = "10:01:00", Active = false},
                        new Sensordata { Id = 1, SensorId = "Medicatiesensor", KlantId = "Frank", Value = "prioriteit", Timestamp = "10:50:00", Active = true}
                    }
                , FILENAME);
            }

            return sensordatas;
        }

        /// <summary>
        /// Gets the list of contacts
        /// </summary>
        /// <returns>The contacts</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Type = typeof(IEnumerable<Sensordata>))]
        [Route("~/sensordata")]
        public async Task<IEnumerable<Sensordata>> Get()
        {
            return await GetSensordatas();
        }

        /// <summary>
        /// Gets a specific contact
        /// </summary>
        /// <param name="id">Identifier for the contact</param>
        /// <returns>The requested contact</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "OK",
            Type = typeof(IEnumerable<Sensordata>))]
        [SwaggerResponse(HttpStatusCode.NotFound,
            Description = "Contact not found",
            Type = typeof(IEnumerable<Sensordata>))]
        [SwaggerOperation("GetContactById")]
        [Route("~/sensordata/{id}")]
        public async Task<Sensordata> Get([FromUri] int id)
        {
            var sensordatas = await GetSensordatas();
            return sensordatas.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="contact">The new contact</param>
        /// <returns>The saved contact</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created,
            Description = "Created",
            Type = typeof(Sensordata))]
        [Route("~/sensoritem")]
        public async Task<Sensordata> Post([FromBody] Sensordata sensordata)
        {
            var sensordatas = await GetSensordatas();
            var sensordataList = sensordatas.ToList();
            try {
                var highest = sensordataList.LastOrDefault();
                sensordata.Id = highest.Id + 1;
            }
            catch
            {
               
            }
            sensordataList.Add(sensordata);
            await _storage.SaveSensorData(sensordataList, FILENAME);
            return sensordata;
        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id">Identifier of the contact to be deleted</param>
        /// <returns>True if the contact was deleted</returns>
        [HttpDelete]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "OK",
            Type = typeof(bool))]
        [SwaggerResponse(HttpStatusCode.NotFound,
            Description = "Contact not found",
            Type = typeof(bool))]
        [Route("~/sensordata/{id}")]
        public async Task<HttpResponseMessage> Delete([FromUri] int id)
        {
            var sensordatas = await GetSensordatas();
            var sensordataList = sensordatas.ToList();

            if (!sensordataList.Any(x => x.Id == id))
            {
                return Request.CreateResponse<bool>(HttpStatusCode.NotFound, false);
            }
            else
            {
                sensordataList.RemoveAll(x => x.Id == id);
                await _storage.SaveSensorData(sensordataList, FILENAME);
                return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
            }
        }
    }
}