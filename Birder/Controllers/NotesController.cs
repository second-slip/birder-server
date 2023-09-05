
// from Create action:
// _observationNoteRepository.AddRange(observation.Notes); // todo: remove when Notes are managed separately


// from Update action:
// // ToDo: separate ObservationNotesController to handle this stuff.  
// // ...need to redesign UI first...
// var notes = await _observationNoteRepository.FindAsync(x => x.Observation.ObservationId == id);

// var notesDeleted = ObservationNotesHelper.GetDeletedNotes(notes, model.Notes);
// if (notesDeleted.Any())
// {
//     _observationNoteRepository.RemoveRange(notesDeleted);
// }

// var notesAdded = ObservationNotesHelper.GetAddedNotes(model.Notes);
// if (notesAdded.Any())
// {
//     var added = _mapper.Map(notesAdded, new List<ObservationNote>());
//     added.ForEach(a => a.Observation = observation);
//     _observationNoteRepository.AddRange(added);
// }

// // ToDo: is the condition necessary here?
// if (notes.Any())
// {
//     _mapper.Map<List<ObservationNoteDto>, IEnumerable<ObservationNote>>(model.Notes, notes);
// }