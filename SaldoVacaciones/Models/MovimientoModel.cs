using System.ComponentModel.DataAnnotations;

namespace SaldoVacaciones.Models
{
    public class MovimientoModel
    {
        public int Id { get; set; }
        public int IdEmpleado { get; set; }
        public int IdTipoMovimiento { get; set; }
        public DateOnly Fecha { get; set; }
        public int Monto { get; set; }
        public int NuevoSaldo { get; set;}
    }
}
