using PMD_Backend.models;

namespace PMD_Backend.interfaces
{
    public interface Auditable
    {
        public string CreateAudit(string auditAction, Admin admin, Vehicle vehicle);

        public static readonly string CREATED_AN_ENTRY = "created an entry";
    }
}
