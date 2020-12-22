using Birder.Data.Model;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Helpers
{
    public static class ObservationNotesHelper
    {
        public static IEnumerable<ObservationNote> GetDeletedNotes(IEnumerable<ObservationNote> originalNotes, List<ObservationNoteDto> editedNotes)
        {
            if (originalNotes is null || editedNotes is null) 
            {
                throw new NullReferenceException("The notes collection is null");
            }

            return originalNotes.Where(item => !editedNotes.Any(item2 => item2.Id == item.Id));

            // An alternative approach
            //HashSet<int> oldIds = new HashSet<int>(editedNotes.Select(s => s.Id));
            //var results = originalNotes.Where(m => !oldIds.Contains(m.Id));
        }
    }
}
