namespace SaldoVacaciones.Models
{
    public class ActiveEmpleado

    { 
        // singleton para el usuario que se encuentra activo
        private static ActiveEmpleado _instance;
        private EmpleadoModel empleadoActivo;

        public static ActiveEmpleado GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ActiveEmpleado();
            }
            return _instance;
        }
        //getters and setters
        public EmpleadoModel GetEmpleado() { return this.empleadoActivo; }
        public void SetEmpleado(EmpleadoModel empleadoActivo)
        {
            this.empleadoActivo = empleadoActivo;
        }
    }
}
