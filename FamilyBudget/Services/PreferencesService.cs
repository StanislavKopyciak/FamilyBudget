using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyBudget.Services
{
    public static class PreferencesService
    {
        public static void SetUserId(int id)
        {
            Preferences.Set("UserId", id.ToString()); 
        }

        public static int? GetUserId()
        {
            var value = Preferences.Get("UserId", null);
            return int.TryParse(value, out int id) ? id : (int?)null;
        }
    }

}
