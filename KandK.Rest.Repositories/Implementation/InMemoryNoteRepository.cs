using Kandk.Rest.Shared.Models;
using KandK.Rest.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KandK.Rest.Repositories
{
  /// <summary>
  /// Create, Read, Update and Delete notes from an in memory data storage.
  /// </summary>
  public class InMemoryNoteRepository : INoteRepository
  {
    private static List<Note> _notes;

    static InMemoryNoteRepository()
    {
      _notes = new List<Note>();

      for (uint i = 1; i < 11; i++)
      {
        _notes.Add(CreateNote(i));
      }
    }

    private static Note CreateNote(uint noteNumber)
    {
      var id = Guid.NewGuid();
      var title = CreateNoteTitle(noteNumber);
      var message = CreateNoteMessage(noteNumber);
      var creationTime = DateTime.UtcNow;
      var lastModifiedTime = DateTime.UtcNow;

      var note = new Note(id, title, message, creationTime, lastModifiedTime);

      return note;
    }

    private static string CreateNoteTitle(uint noteNumber)
    {
      return "Title " + noteNumber;
    }

    private static string CreateNoteMessage(uint noteNumber)
    {
      return String.Concat("Lorem ", noteNumber, Environment.NewLine, "ipsum ", noteNumber, Environment.NewLine, "dolor ", noteNumber, Environment.NewLine, "sit ", noteNumber, " amet");
    }

    /// <inheritdoc/>
    public Note Get(Guid noteId)
    {
      var note = _notes.FirstOrDefault(n => n.Id == noteId);
      return note;
    }

    /// <inheritdoc/>
    public IEnumerable<Note> Get(DateTime minimalCreationDate, DateTime maximumCreationDate, int offset, int maxAmount)
    {
      var notes = _notes.Where(n => n.CreationTime >= minimalCreationDate && n.CreationTime <= maximumCreationDate).OrderByDescending(n => n.CreationTime).AsEnumerable();
      notes = notes.Skip(offset);
      notes = notes.Take(maxAmount);

      return notes;
    }

    /// <inheritdoc/>
    public bool Add(Note note)
    {
      var noteExisting = _notes.SingleOrDefault(n => n.Id == note.Id);

      if (noteExisting != null)
      {
        return false;
      }

      _notes.Add(note);
      return true;
    }

    /// <inheritdoc/>
    public bool Update(Note note)
    {
      var noteExisting = _notes.SingleOrDefault(n => n.Id == note.Id);

      if (noteExisting == null)
      {
        return false;
      }

      noteExisting.Update(note.Title, note.Message);
      return true;
    }

    /// <inheritdoc/>
    public bool Delete(Guid noteId)
    {
      var note = _notes.SingleOrDefault(n => n.Id == noteId);

      if (note == null)
      {
        return false;
      }

      _notes.Remove(note);
      return true;
    }
  }
}
