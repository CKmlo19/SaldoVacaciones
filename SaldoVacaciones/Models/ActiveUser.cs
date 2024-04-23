namespace SaldoVacaciones.Models
{
    public class ActiveUser
    {
        // singleton para el usuario que se encuentra activo
        private static ActiveUser _instance;
        private string username;
        private string password;
        private string IP;

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
        public string GetIP() { return IP;}
        public void SetUsername(string username) { 
            this.username = username;
        }
        public void SetPassword(string password) {
            this.password = password;
        }
        public void SetIP(string ip) {
            this.IP = ip;
        }
    }
}
