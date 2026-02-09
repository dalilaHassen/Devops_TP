using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.model
{
    public class Incident
    {
        //Titre court de l’incident
        public int id { get; set; }
        [Required]
        [StringLength(30 ,MinimumLength =2 , ErrorMessage="doit etre entre 2 et 30 caracter" )]//le premier est le max le dexieme est minimum on peut ajouter d'autre attribut 
        public string title { get; set; } = null!;//la valeur ne soit jamais null
        //Description détaillée
        [Required]
        [StringLength(200)]
        public string description { get; set; } = string.Empty;
        //Gravité (LOW, MEDIUM, HIGH,cRITICAL)
        [Required] 
        public string severity { get; set; } = string.Empty;
        //État(OPEN, IN_PROGRESS,RESOLVED)
        [Required] 
        public string status { get; set; } = string.Empty;

        //emails
        [EmailAddress]
        [DataType(dataType: DataType.EmailAddress)]
        public string email { get; set; }
        //Date de création de l’incident
        [Required] 
        public  DateTime createdat { get; set; }

    }
}
