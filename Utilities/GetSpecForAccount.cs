using myclinic_back.Models;

namespace myclinic_back.Utilities
{
    public class GetSpecForAccount
    {
        private static readonly PiProjectContext _context;
        public static string getSpecForAccount(int? idSpec)
        {
            var spec = _context.Specializations.FirstOrDefault(s => s.IdSpecialization == idSpec);

            if (spec != null)
            {
                return "";
            }

            return spec.Name;
        } 
    }
}
