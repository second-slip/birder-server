namespace Birder.Tests.HelpersTests;


// use fluent approach...

// (1) null reference
// (2) empty collections -- no this is part of (3)
// (3) different scenarios
public class DeletedNotesHelperTests
{
    [Theory, MemberData(nameof(NullArgumentTestData))]
    public void GetDeletedNotes_ReturnsNullReferenceException_WhenEitherOrBothArgumentaAreNull(List<ObservationNote> originalNotes, List<ObservationNoteDto> editedNotes)
    {
        // Act & Assert
        var ex = Assert.Throws<NullReferenceException>(() => ObservationNotesHelper.GetDeletedNotes(originalNotes, editedNotes));
        Assert.Equal("The notes collection is null", ex.Message);
    }

    [Theory, MemberData(nameof(ZeroResultTestData))]
    public void GetDeletedNotes_ReturnsZeroResult_WhenAppropriate(List<ObservationNote> originalNotes, List<ObservationNoteDto> editedNotes)
    {
        //Act
        var result = ObservationNotesHelper.GetDeletedNotes(originalNotes, editedNotes);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationNote>>(result);
        Assert.Empty(result);
    }

    [Theory, MemberData(nameof(OneResultTestData))]
    public void GetDeletedNotes_ReturnsOneResult_WhenAppropriate(List<ObservationNote> originalNotes, List<ObservationNoteDto> editedNotes)
    {
        //Act
        var result = ObservationNotesHelper.GetDeletedNotes(originalNotes, editedNotes);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationNote>>(result);
        Assert.Single(result);
        Assert.Equal(1, result.First().Id);
    }

    [Theory, MemberData(nameof(MultipleResultsTestData))]
    public void GetDeletedNotes_ReturnsMultipleResults_WhenAppropriate(List<ObservationNote> originalNotes, List<ObservationNoteDto> editedNotes)
    {
        //Act
        var result = ObservationNotesHelper.GetDeletedNotes(originalNotes, editedNotes);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationNote>>(result);
        Assert.Equal(2, result.Count());
        var deleted1 = result.Where(t => t.Id == 1).FirstOrDefault();
        Assert.Contains(deleted1, result);

        var deleted2 = result.Where(t => t.Id == 2).FirstOrDefault();
        Assert.Contains(deleted2, result);
    }


    // test scenarios which should return an empty collection
    public static IEnumerable<object[]> MultipleResultsTestData
    {
        get
        {
            return new[]
            {
                    // multiple old AND multiple new == 2
                    new object[] {
                        new List<ObservationNote>()
                        {
                            new ObservationNote()
                            {
                                Id = 1,
                                Note = "Test",
                                NoteType = ObservationNoteType.General
                            },
                            new ObservationNote()
                            {
                                Id = 2,
                                Note = "Test",
                                NoteType = ObservationNoteType.General
                            }
                        },
                        new List<ObservationNoteDto>()
                        {
                            new ObservationNoteDto()
                            {
                                Id = 11,
                                Note = "Test",
                                NoteType = "General"
                            },
                            new ObservationNoteDto()
                            {
                                Id = 12,
                                Note = "Test",
                                NoteType = "General"
                            }
                        }
                    }
                };
        }
    }



    // test scenarios which should return an empty collection
    public static IEnumerable<object[]> OneResultTestData
    {
        get
        {
            return new[]
            {
                    // one old and it was deleted == 1
                    new object[] {
                        new List<ObservationNote>()
                        {
                            new ObservationNote()
                            {
                                Id = 1,
                                Note = "Test",
                                NoteType = ObservationNoteType.General
                            }

                        },
                        new List<ObservationNoteDto>()
                    },
                    // multiple old And ONE added AND ONE was deleted == 1 (deleted id == 1)
                    new object[] {
                        new List<ObservationNote>()
                        {
                            new ObservationNote()
                            {
                                Id = 1,
                                Note = "Test",
                                NoteType = ObservationNoteType.General
                            },
                            new ObservationNote()
                            {
                                Id = 2,
                                Note = "Test",
                                NoteType = ObservationNoteType.General
                            }
                        },
                        new List<ObservationNoteDto>()
                        {
                            new ObservationNoteDto()
                            {
                                Id = 2,
                                Note = "Test",
                                NoteType = "General"
                            },
                            new ObservationNoteDto()
                            {
                                Id = 3,
                                Note = "Test",
                                NoteType = "General"
                            }
                        }
                    }
                };
        }
    }


    // test scenarios which should return an empty collection
    //This is unreadable; maybe change to three [Fact] unit tests
    public static IEnumerable<object[]> ZeroResultTestData
    {
        get
        {
            return new[]
            {
                    // no old AND no new == 0
                    new object[] {
                        new List<ObservationNote>(),
                        new List<ObservationNoteDto>()
                    },
                    // no old notes but news ones added == 0
                    new object[] {
                        new List<ObservationNote>(),
                        new List<ObservationNoteDto>()
                        {
                            new ObservationNoteDto()
                            {
                                Id = 1,
                                Note = "Test",
                                NoteType = "General"
                            }
                        }
                    },
                    // old AND new but none deleted == 0
                    new object[] {
                        new List<ObservationNote>()
                        {
                            new ObservationNote()
                            {
                                Id = 1,
                                Note = "Test",
                                NoteType = ObservationNoteType.General
                            }

                        },
                        new List<ObservationNoteDto>()
                        {
                            new ObservationNoteDto()
                            {
                                Id = 1,
                                Note = "Test",
                                NoteType = "General"
                            }
                        }
                    }
                };
        }
    }


    // null argument test helpers...
    public static IEnumerable<object[]> NullArgumentTestData
    {
        get
        {
            return new[]
            {
                    new object[] { null, null },
                    new object[] { new List<ObservationNote>(), null },
                    new object[] { null, new List<ObservationNoteDto>() }
                };
        }
    }

}