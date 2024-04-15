using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SaldoVacaciones.Models
{
    public class EmpleadoModel
    {
        public int Id { get; set; }
        public int IdPuesto { get; set; }
        public string? NombrePuesto { get; set; }
        public string? ValorDocumentoIdentidad { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaContratacion {  get; set; }
        public short SaldoVacaciones { get; set; }
        public bool EsActivo { get; set; }

       
    }
}
