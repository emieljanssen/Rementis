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
    public class AgendaController : ApiController
    {
        private const string FILENAME = "agendadata.json";
        private GenericStorage _storage;


        public AgendaController()
        {
            _storage = new GenericStorage();
        }

        private async Task<IEnumerable<Agendadata>> GetAgenda()
        {
            var agenda = await _storage.GetAgenda(FILENAME);

            if (agenda == null)
            {
                await _storage.SaveAgendaData(new Agendadata[]{
                        new Agendadata { MessageId = 1, Titel = "agendatitel", CostumerId = 1, Description = "agendadescriptie", StartDate = "11/11/2017", EndDate="12/11/2017", StartTime = "11:11:11", EndTime = "12:12:12", Priority = "High", State = "pending"},
                        new Agendadata { MessageId = 2, Titel = "agendatitel", CostumerId = 1, Description = "agendadescriptie", StartDate = "11/11/2017", EndDate="12/11/2017", StartTime = "11:11:11", EndTime = "12:12:12", Priority = "High", State = "pending"},
                        new Agendadata { MessageId = 3, Titel = "agendatitel", CostumerId = 1, Description = "agendadescriptie", StartDate = "11/11/2017", EndDate="12/11/2017", StartTime = "11:11:11", EndTime = "12:12:12", Priority = "High", State = "pending"}
                    }
                , FILENAME);
            }

            return agenda;
        }

        /// <summary>
        /// Gets the list of contacts
        /// </summary>
        /// <returns>The contacts</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Type = typeof(IEnumerable<Agendadata>))]
        [Route("~/agenda")]
        public async Task<IEnumerable<Agendadata>> Get()
        {
            return await GetAgenda();
        }

        /// <summary>
        /// Gets a specific contact
        /// </summary>
        /// <param name="id">Identifier for the contact</param>
        /// <returns>The requested contact</returns>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "OK",
            Type = typeof(IEnumerable<Agendadata>))]
        [SwaggerResponse(HttpStatusCode.NotFound,
            Description = "Contact not found",
            Type = typeof(IEnumerable<Agendadata>))]
        [SwaggerOperation("GetAgendaById")]
        [Route("~/agenda/{id}")]
        public async Task<Agendadata> Get([FromUri] int id)
        {
            var agenda = await GetAgenda();
            return agenda.FirstOrDefault(x => x.MessageId == id);
        }

        /// <summary>
        /// Creates a new contact
        /// </summary>
        /// <param name="contact">The new contact</param>
        /// <returns>The saved contact</returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created,
            Description = "Created",
            Type = typeof(Agendadata))]
        [Route("~/agendaitem")]
        public async Task<Agendadata> Post([FromBody] Agendadata agendaitem)
        {
            var agenda = await GetAgenda();
            var agendaList = agenda.ToList();
            try
            {
                var highest = agendaList.LastOrDefault();
                agendaitem.MessageId = highest.MessageId + 1;
            }
            catch
            {

            }
            agendaList.Add(agendaitem);
            await _storage.SaveAgendaData(agendaList, FILENAME);
            return agendaitem;
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
            Description = "agendaitem not found",
            Type = typeof(bool))]
        [Route("~/agenda/{id}")]
        public async Task<HttpResponseMessage> Delete([FromUri] int id)
        {
            var agenda = await GetAgenda();
            var agendaList = agenda.ToList();

            if (!agendaList.Any(x => x.MessageId == id))
            {
                return Request.CreateResponse<bool>(HttpStatusCode.NotFound, false);
            }
            else
            {
                agendaList.RemoveAll(x => x.MessageId == id);
                await _storage.SaveAgendaData(agendaList, FILENAME);
                return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
            }
        }
    }
}