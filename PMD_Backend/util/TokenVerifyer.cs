using PMD_Backend.controller.adminControllers;
using PMD_Backend.models;

namespace PMD_Backend.util
{
    public class TokenVerifyer
    {
        private Admin? admin;
        
        public string Message {  get; set; }

        public Admin getUser()
        {
            return this.admin;
        }

        public TokenVerifyer(string token)
        {
            this.Message = Verify(token);
        }

        private string Verify(string token)
        {
            return new GetDetailsController(token).GetDetails(out admin);
        }
    }
}
