using Kandk.Rest.Shared.Models;
using System;
using System.Collections.Generic;

namespace KandK.Rest.Shared.Interfaces
{
  public interface INoteRepository
  {
    /// <summary>
    /// Returns a note by unique identifier.
    /// </summary>
    /// <param name="noteId">The unique identifier of the note.</param>
    /// <returns></returns>
    Note Get(Guid noteId);

    /// <summary>
    /// Returns notes in a given time span. The results are ordered from newest to oldest.
    /// </summary>
    /// <param name="minimalCreationDate">Inclusive minimal creation date of the notes.</param>
    /// <param name="maximumCreationDate">Inclusive maximum creatiuon date of the notes.</param>
    /// <param name="offset">Amount of notes which should be skipped.</param>
    /// <param name="maxAmount">Maximum amount of notes to return.</param>
    /// <returns></returns>
    IEnumerable<Note> Get(DateTime minimalCreationDate, DateTime maximumCreationDate, int offset = 0, int maxAmount = int.MaxValue);

    /// <summary>
    /// Adds a note to the data storage.
    /// </summary>
    /// <param name="note">A new note.</param>
    /// <returns></returns>
    bool Add(Note note);

    /// <summary>
    /// Updates note data in the data storage.
    /// </summary>
    /// <param name="note">The existing not to update.</param>
    /// <returns>True if note was found and successfully updated in the data store.</returns>
    bool Update(Note note);

    /// <summary>
    /// Deletes a note from the data storage.
    /// </summary>
    /// <param name="noteId">The unique identifier of the note.</param>
    /// <returns>True if note was found and successfully removed from data storage.</returns>
    bool Delete(Guid noteId);
  }
}