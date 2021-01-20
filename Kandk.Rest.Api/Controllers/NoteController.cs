using Kandk.Rest.Shared.Models;
using KandK.Rest.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kandk.Rest.Api.Controllers
{
  /// <summary>
  /// CRUD Controller for Notes.
  /// </summary>
  [ApiController]
  [Route("[controller]")]
  public class NoteController : ControllerBase
  {
    private readonly ILogger<NoteController> _logger;
    private readonly INoteRepository _noteRepository;

    /// <summary>
    /// Creates a new instance of the note controller.
    /// </summary>
    /// <param name="noteRepository">The repository for notes.</param>
    /// <param name="logger">A logger.</param>
    public NoteController(INoteRepository noteRepository, ILogger<NoteController> logger)
    {
      _noteRepository = noteRepository;
      _logger = logger;
    }

    /// <summary>
    /// Gets some of the latest notes. Amount and time span depends on configuration.
    /// </summary>
    /// <returns>Some of the latest notes.</returns>
    [HttpGet("[action]")]
    public IEnumerable<Note> GetDefault()
    {
      // Todo: put values into configuration.
      var minimalCreationDate = DateTime.UtcNow.AddDays(-30);
      var maximumCreationDate = DateTime.UtcNow;
      var offset = 0;
      var maxAmount = 100;

      return GetWithFilter(minimalCreationDate, maximumCreationDate, offset, maxAmount);
    }

    /// <summary>
    /// Gets notes according to the filter.
    /// </summary>
    /// <param name="minimalCreationDate">Inclusive minimal creation date of the notes.</param>
    /// <param name="maximumCreationDate">Inclusive maximum creatiuon date of the notes.</param>
    /// <param name="offset">Amount of notes which should be skipped.</param>
    /// <param name="maxAmount">Maximum amount of notes to return.</param>
    /// <returns></returns>
    [HttpGet("[action]/{minimalCreationDate}/{maximumCreationDate}/{offset}/{maxAmount}")]
    public IEnumerable<Note> GetWithFilter(DateTime minimalCreationDate, DateTime maximumCreationDate, int offset, int maxAmount)
    {
      var notes = _noteRepository.Get(minimalCreationDate, maximumCreationDate, offset, maxAmount);

      return notes;
    }

    /// <summary>
    /// Get the given note. None if not found.
    /// </summary>
    /// <param name="noteId">The unique identifier of the note.</param>
    /// <returns>One or no note.</returns>
    [HttpGet("[action]/{noteId}")]
    public Note GetById(Guid noteId)
    {
      var note = _noteRepository.Get(noteId);

      return note;
    }

    /// <summary>
    /// Updates the note.
    /// </summary>
    /// <param name="note">The note to update.</param>
    /// <returns>True if note was found and successfully updated in the data storage.</returns>
    [HttpPost("[action]")]
    public bool Update(Note note)
    {
      var success = _noteRepository.Update(note);

      return success;
    }

    /// <summary>
    /// Creates a note.
    /// </summary>
    /// <param name="note">The note to create.</param>
    /// <returns>True if note was found and successfully created in the data storage.</returns>
    [HttpPost("[action]")]
    public bool Create(Note note)
    {
      var success = _noteRepository.Add(note);

      return success;
    }

    /// <summary>
    /// Delete the given note. None if not found.
    /// </summary>
    /// <param name="noteId">The unique identifier of the note.</param>
    /// <returns>True if note was found and deleted from data storage.</returns>
    [HttpGet("[action]/{noteId}")]
    public bool Delete(Guid noteId)
    {
      var success = _noteRepository.Delete(noteId);

      return success;
    }
  }
}
