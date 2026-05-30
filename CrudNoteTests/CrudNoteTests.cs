using Arch.EFCore;
using Microsoft.EntityFrameworkCore;

namespace CrudNoteTests;

public class CrudNoteTests
{
    private async Task ClearNotes()
    {
        await using var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        await db.Notes.ExecuteDeleteAsync();
    }


    [Fact]
    public async Task CreateTest()
    {
        await ClearNotes();

        var TestNote = await CrudNote.Create("Test", null);

        Assert.NotEqual(0, TestNote.Id);
        Assert.Equal("Test", TestNote.Text);
        Assert.True((DateTimeOffset.Now - TestNote.CreatedAt).TotalSeconds < 5);

    }

    [Fact]
    public async Task ReadTest_SearchByText()
    {
        await ClearNotes();

        var TestNote = await CrudNote.Create("Test", null);
        var TestSearch = await CrudNote.Read("Tes");

        Assert.NotEmpty(TestSearch);
        Assert.Equal("Test", TestSearch[0].Text);
    }

    [Fact]
    public async Task ReadTest_SearchByID()
    {
        await ClearNotes();

        var TestNote = await CrudNote.Create("Test", null);
        var TestSearch = await CrudNote.Read(TestNote.Id);


        Assert.NotNull(TestSearch);
        Assert.Equal(TestNote.Id, TestSearch.Id);
        Assert.Equal("Test", TestSearch.Text);

    }

    [Fact]
    public async Task UpdateTest()
    {
        await ClearNotes();

        var TestNote = await CrudNote.Create("Test", null);
        await CrudNote.Update(TestNote, "Update");
        var updatedNote = await CrudNote.Read(TestNote.Id);

        Assert.NotNull(updatedNote);
        Assert.Equal("Update", updatedNote.Text);
    }

    [Fact]
    public async Task DeleteTest()
    {
        await ClearNotes();

        var TestNote = await CrudNote.Create("Test", null);
        await CrudNote.Delete(TestNote);
        var deletedNote = await CrudNote.Read(TestNote.Id);

        Assert.Null(deletedNote);
    }
}
