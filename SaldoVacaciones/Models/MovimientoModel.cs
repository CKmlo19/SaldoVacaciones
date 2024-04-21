using System.ComponentModel.DataAnnotations;

namespace SaldoVacaciones.Models
{
    public class MovimientoModel
    {
        public int Id { get; set; }
        public int IdEmpleado { get; set; }
        public int IdTipoMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public int Monto { get; set; }
        public int NuevoSaldo { get; set;}
        public string? NombreTipoMovimiento { get; set; }
        public string? NombreUsuario {  get; set; }
        public string? PostIP {  get; set; }
        public DateTime PostTime { get; set; }
    }
}
