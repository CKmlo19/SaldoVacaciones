namespace SaldoVacaciones.Models
{
    public class ActiveUser
    {
        // singleton para el usuario que se encuentra activo
        private static ActiveUser _instance;
        private string username;
        private string password;

        public static ActiveUser GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ActiveUser();
            }
            return _instance;
        }
        //getters and setters
        public string GetUsername() { return username;}
        public string GetPassword() { return password;}
        public void SetUsername(string username) { 
            this.username = username;
        }
        public void SetPassword(string password) {
            this.password = password;
        }
    }
}
