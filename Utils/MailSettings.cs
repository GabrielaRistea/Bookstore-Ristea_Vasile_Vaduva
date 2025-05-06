using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Bookstore.Utils
{
    public class MailSettings
    {
        public required string Mail = "ristea.gabriela.t8i@student.ucv.ro"; // Adresa ta de la "From"
        public required string DisplayName = "Bookstore App";
        public required string Username = "MS_lorIzz@test-69oxl5eex2xl785k.mlsender.net"; // Username exact din Mailersend
        public required string AppPassword = "mssp.VMxajGT.0r83ql3o6yplzw1j.xR3U6G2"; // parola (API key)
        public required string Host = "smtp.mailersend.net";
        public required int Port = 587;
    }
}
