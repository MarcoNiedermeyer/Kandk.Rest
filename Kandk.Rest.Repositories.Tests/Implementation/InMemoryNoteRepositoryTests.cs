using FluentAssertions;
using KandK.Rest.Repositories;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Kandk.Rest.Shared.Models;

namespace Kandk.Rest.Repositories.Tests.Implementation
{
  [TestClass]
  public class InMemoryNoteRepositoryTests
  {
    const int offset = 0;
    const int maxAmount = int.MaxValue;
    const string title = "New Title";
    const string message = "New Message";

    [TestMethod]
    public void Get_IdExists_ReturnsCorrectNote()
    {
      // Given
      var noteRepository = new InMemoryNoteRepository();
      var note = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, 0, 1).Single();
      var noteId = note.Id;

      // When
      note = noteRepository.Get(noteId);

      // Then
      note.Should().NotBeNull();
      note.Id.Should().Be(noteId);
    }

    [TestMethod]
    public void Get_IdDoesNotExist_ReturnsNull()
    {
      // Given
      var noteRepository = new InMemoryNoteRepository();

      // When
      var note = noteRepository.Get(Guid.NewGuid());

      // Then
      note.Should().BeNull();
    }

    // No better testing possible due to static nature of class.
    [DataTestMethod]
    [DataRow(5, 5, 5)]
    [DataRow(5, 3, 3)]
    [DataRow(int.MaxValue, 3, 0)]
    public void Get_AllOk_ReturnsCorrectAmountOfEntries(int offset, int maxAmount, int expectedValue)
    {
      // Given
      var noteRepository = new InMemoryNoteRepository();

      // When
      var notes = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, offset, maxAmount);

      // Then
      notes.Should().HaveCount(expectedValue);
    }

    [TestMethod]
    public void Create_AllOk_NewNoteIsAdded()
    {
      // Given
      var id = Guid.NewGuid();
      var creationTime = DateTime.UtcNow.AddDays(-10);
      var lastModifiedTime = DateTime.UtcNow.AddDays(-5);

      var noteRepository = new InMemoryNoteRepository();
      var notes = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, offset, maxAmount);
      var amountOfNotesInRepository = notes.Count();
      var note = new Note(id, title, message, creationTime, lastModifiedTime);

      // When
      var isSuccess = noteRepository.Add(note);

      notes = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, offset, maxAmount);
      note = notes.Single(n => n.Id == note.Id);

      // Then
      var expectedNote = new
      {
        Id = id,
        Title = title,
        Message = message,
        CreationTime = creationTime,
        LastModifiedTime = lastModifiedTime
      };

      isSuccess.Should().BeTrue();
      note.Should().BeEquivalentTo(expectedNote);
      notes.Count().Should().Be(amountOfNotesInRepository + 1);
    }

    [TestMethod]
    public void Create_NoteAlreadyExists_ReturnValueIsFalse()
    {
      // Given
      var noteRepository = new InMemoryNoteRepository();
      var noteExisiting = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, 0, 1).Single();
      var noteNew = new Note(noteExisiting.Id, title, message, DateTime.UtcNow, DateTime.UtcNow);

      // When
      var isSuccess = noteRepository.Add(noteNew);

      // Then
      isSuccess.Should().BeFalse();
    }

    [TestMethod]
    public void Update_AllOk_PropertiesAreUpdated()
    {
      // Given
      var noteRepository = new InMemoryNoteRepository();
      var notes = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, offset, maxAmount);
      var amountOfNotesInRepository = notes.Count();
      var note = notes.First();

      // When
      note.Update(title, message);
      var isSuccess = noteRepository.Update(note);

      notes = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, offset, maxAmount);
      note = notes.Single(n => n.Id == note.Id);

      // Then
      isSuccess.Should().BeTrue();
      note.Title.Should().Be(title);
      note.Message.Should().Be(message);
      notes.Count().Should().Be(amountOfNotesInRepository);
    }

    [TestMethod]
    public void Update_NoteDoesNotExist_ReturnValueIsFalse()
    {
      // Given
      var noteRepository = new InMemoryNoteRepository();
      var note = new Note(Guid.NewGuid(), title, message, DateTime.UtcNow, DateTime.UtcNow);

      // When
      var isSuccess = noteRepository.Update(note);

      // Then
      isSuccess.Should().BeFalse();
    }

    [TestMethod]
    public void Delete_NoteDoesNotExist_ReturnValueIsFalse()
    {
      // Given
      var noteRepository = new InMemoryNoteRepository();

      // When
      var isSuccess = noteRepository.Delete(Guid.NewGuid());

      // Then
      isSuccess.Should().BeFalse();
    }

    [TestMethod]
    public void Delete_NoteExists_NoteIsRemovedFromDataStorage()
    {
      // Given
      var noteRepository = new InMemoryNoteRepository();
      var notes = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, offset, maxAmount);
      var amountOfNotesInRepository = notes.Count();
      var noteExisiting = notes.First();

      // When
      var isSuccess = noteRepository.Delete(noteExisiting.Id);
      notes = noteRepository.Get(DateTime.MinValue, DateTime.MaxValue, offset, maxAmount);

      // Then
      isSuccess.Should().BeTrue();
      notes.Count().Should().Be(amountOfNotesInRepository - 1);
    }
  }
}
