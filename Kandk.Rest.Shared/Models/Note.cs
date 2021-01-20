using System;

namespace Kandk.Rest.Shared.Models
{
  /// <summary>
  /// A personal note.
  /// </summary>
  public class Note
  {
    /// <summary>
    /// The unique identifier of the note.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The title of the note.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// The contents of the note.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// The creation time of the note.
    /// </summary>
    public DateTime CreationTime { get; private set; }

    /// <summary>
    /// The last time the note was modified.
    /// </summary>
    public DateTime LastModifiedTime { get; private set; }

    /// <summary>
    /// Creates a new Note.
    /// </summary>
    /// <param name="id">The unique identifier of the note.</param>
    /// <param name="title">The title of the note.</param>
    /// <param name="message">The contents of the note.</param>
    /// <param name="creationTime">The creation time of the note.</param>
    /// <param name="lastModifiedTime">The last time the note was modified.</param>
    public Note(Guid id, string title, string message, DateTime creationTime, DateTime lastModifiedTime)
    {
      UpdateData(title, message);

      Id = id;
      CreationTime = creationTime;
      LastModifiedTime = lastModifiedTime;
    }

    /// <summary>
    /// Overwrites the data of the note.
    /// </summary>
    /// <param name="title">The title of the note.</param>
    /// <param name="message">The contents of the note.</param>
    /// <returns></returns>
    public bool Update(string title, string message)
    {
      UpdateData(title, message);

      // ToDo: Check data before updating.
      return true;
    }

    private void UpdateData(string title, string message)
    {
      Title = title;
      Message = message;
      LastModifiedTime = DateTime.UtcNow;
    }
  }
}
