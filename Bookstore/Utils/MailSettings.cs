using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Bookstore.Utils
{
    public class MailSettings
    {
        public required string Mail = "gabriellaa2707@gmail.com"; // Adresa ta de la "From"
        public required string DisplayName = "Bookstore App";
        public required string Username = "MS_1jRdlE@test-eqvygm003x8l0p7w.mlsender.net"; // Username exact din Mailersend
        public required string AppPassword = "mssp.oVsbQqL.3z0vklover7g7qrx.HCXO69p"; // parola (API key)
        public required string Host = "smtp.mailersend.net";
        public required int Port = 587;
    }
}
