namespace Birder.Tests.HelpersTests;

public class GetAddedNotesTests
{
    [Theory, MemberData(nameof(NullArgumentTestData))]
    public void GetAddedNotes_ReturnsNullReferenceException_WhenEitherOrBothArgumentaAreNull(List<ObservationNoteDto> notes)
    {
        // Act & Assert
        var ex = Assert.Throws<NullReferenceException>(() => ObservationNotesHelper.GetAddedNotes(notes));
        Assert.Equal("The notes collection is null", ex.Message);
    }

    [Theory, MemberData(nameof(ZeroResultTestData))]
    public void GeAddedNotes_ReturnsZeroResult_WhenAppropriate(List<ObservationNoteDto> notes)
    {
        //Act
        var result = ObservationNotesHelper.GetAddedNotes(notes);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationNoteDto>>(result);
        Assert.Empty(result);
    }

    [Theory, MemberData(nameof(MultipleResultsTestData))]
    public void GetAddedNotes_ReturnsMultipleResults_WhenAppropriate(List<ObservationNoteDto> notes)
    {
        //Act
        var result = ObservationNotesHelper.GetAddedNotes(notes);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationNoteDto>>(result);
        Assert.Equal(2, result.Count());
        var deleted1 = result.Where(t => t.Id == 0).FirstOrDefault();
        Assert.Contains(deleted1, result);

        var deleted2 = result.Where(t => t.Id == 0).FirstOrDefault();
        Assert.Contains(deleted2, result);
    }


    // test scenarios which should return an empty collection
    public static IEnumerable<object[]> MultipleResultsTestData
    {
        get
        {
            return new[]
            {
                    // all new
                    new object[] {
                        new List<ObservationNoteDto>()
                        {
                            new ObservationNoteDto()
                            {
                                Id = 0,
                                Note = "Test",
                                NoteType = "General"
                            },
                            new ObservationNoteDto()
                            {
                                Id = 0,
                                Note = "Test",
                                NoteType = "General"
                            }
                        }
                    },
                    // 2 new, 2 existing
                    new object[] {
                        new List<ObservationNoteDto>()
                        {
                            new ObservationNoteDto()
                            {
                                Id = 1,
                                Note = "Test",
                                NoteType = "General"
                            },
                            new ObservationNoteDto()
                            {
                                Id = 2,
                                Note = "Test",
                                NoteType = "General"
                            },
                            new ObservationNoteDto()
                            {
                                Id = 0,
                                Note = "Test",
                                NoteType = "General"
                            },
                            new ObservationNoteDto()
                            {
                                Id = 0,
                                Note = "Test",
                                NoteType = "General"
                            }
                        }
                    }
                };
        }
    }

    // test scenarios which should return an empty collection
    public static IEnumerable<object[]> ZeroResultTestData
    {
        get
        {
            return new[]
            {
                    // empty collection
                    new object[] { new List<ObservationNoteDto>() },
                    // no items with id == 0
                    new object[] {
                        new List<ObservationNoteDto>()
                        {
                            new ObservationNoteDto()
                            {
                                Id = 1,
                                Note = "Test",
                                NoteType = "General"
                            },
                            new ObservationNoteDto()
                            {
                                Id = 2,
                                Note = "Test",
                                NoteType = "General"
                            }
                        }
                    }
                };
        }
    }

    public static IEnumerable<object[]> NullArgumentTestData
    {
        get
        {
            return new[]
            {
                    new object[] { null },
                };
        }
    }
}