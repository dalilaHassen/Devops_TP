using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApplication2.model;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {
        private static readonly List<Incident> _incidents = new();
        private static int _nextId = 1;
        private static readonly string[] AllowedSeverities =
        { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses =
        { "OPEN", "IN_PROGRESS", "RESOLVED" };

        [HttpPost("create-incident")]
        public IActionResult CreateIncident([FromBody] Incident incident)
        {
            

            // Valider la sévérité
            if (!AllowedSeverities.Contains(incident.severity.ToUpper()))
            {
                return BadRequest($"La sévérité doit être l'une des valeurs suivantes : {string.Join(", ", AllowedSeverities)}");
            }

            

            // Assigner l'ID auto-incrémenté
            incident.id = _nextId++;

            // Forcer le statut à OPEN
            incident.status = "OPEN";

            // Définir la date de création (utc maintenant)
            incident.createdat = DateTime.UtcNow;

            // Ajouter à la liste
            _incidents.Add(incident);

            // Retourner l'incident créé avec code 200
            return Ok(incident);
        }
        // afficher tout
        [HttpGet("get-all")]
        public IActionResult GetAllIncidents()
        {
            return Ok(_incidents);
        }

        //afficher avec id
        [HttpGet("getbyid/{id}")]
        public IActionResult GetIncidentById(int id)
        {
            var incident = _incidents.First(i => i.id == id);
            if (incident == null)
                return NotFound();
            return Ok(incident);
        }
        //modifier un champ
        [HttpPut("update-status/{id}")]
        public IActionResult UpdateIncidentStatus(int id, [FromBody] string status)
        {
            // Vérifier que le statut n'est pas null ou vide
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Le statut est requis.");
            }

            // Vérifier que le statut est valide
            if (!AllowedStatuses.Contains(status.ToUpper()))
            {
                return BadRequest($"Le statut doit être l'une des valeurs suivantes : {string.Join(", ", AllowedStatuses)}");
            }

            // Chercher l'incident par ID
            var incident = _incidents.FirstOrDefault(i => i.id == id);

            // Si l'incident n'existe pas, retourner 404
            if (incident == null)
            {
                return NotFound($"Incident avec ID {id} non trouvé.");
            }

            // Mettre à jour le statut (convertir en majuscules pour uniformité)
            incident.status = status.ToUpper();

            // Retourner l'incident mis à jour
            return Ok(incident);
        }

        [HttpDelete("delete-incident/{id}")]
        public IActionResult DeleteIncident(int id)
        {
            // Chercher l'incident par ID
            var incident = _incidents.FirstOrDefault(i => i.id == id);

            // Si l'incident n'existe pas, retourner 404
            if (incident == null)
            {
                return NotFound($"Incident avec ID {id} non trouvé.");
            }

            // Vérifier la règle : CRITICAL + OPEN ne peut pas être supprimé
            if (incident.severity == "CRITICAL" && incident.status == "OPEN")
            {
                return BadRequest("Un incident CRITICAL avec le statut OPEN ne peut pas être supprimé.");
            }

            // Supprimer l'incident de la liste
            _incidents.Remove(incident);

            // Retourner 204 No Content
            return NoContent();
        }




        [HttpGet("filter-by-status")]
        public IActionResult FilterByStatus([FromQuery] string status)
        {
            // Si aucun paramètre n'est fourni, retourner tous les incidents
            if (string.IsNullOrWhiteSpace(status))
            {
                return Ok(_incidents);
            }

            // Filtrer les incidents dont le statut contient la chaîne recherchée (insensible à la casse)
            var filteredIncidents = _incidents
                .Where(i => i.status.Contains(status, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Retourner les incidents filtrés
            return Ok(filteredIncidents);
        }

        [HttpGet("filter-by-severity")]
        public IActionResult FilterBySeverity([FromQuery] string severity)
        {
            // Si aucun paramètre n'est fourni, retourner tous les incidents
            if (string.IsNullOrWhiteSpace(severity))
            {
                return Ok(_incidents);
            }

            // Filtrer les incidents dont la gravité contient la chaîne recherchée (insensible à la casse)
            var filteredIncidents = _incidents
                .Where(i => i.severity.Contains(severity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Retourner les incidents filtrés
            return Ok(filteredIncidents);
        }
    }
}

