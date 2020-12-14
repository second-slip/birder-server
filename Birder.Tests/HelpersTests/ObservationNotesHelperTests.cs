using Birder.Data.Model;
using Birder.Helpers;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.HelpersTests
{

    // (1) null reference
    // (2) empty collections -- no this is part of (3)
    // (3) different scenarios
    public class ObservationNotesHelperTests
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

        // use fluent approach...
        
        

        // multiple deleted 
        // NO --> multiple deleted nut returns > 1 -- add to below test case - multiple notes which have been deleted...
        // two in common?
        //assert that two in common are there...


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


        //[Fact]
        //public void GetFollowersUserNames_ReturnsEmptyCollection_WhenInputCollectionIsEmpty()
        //{
        //    // Arrange
        //    var emptyInputCollection = new List<ObservationNote>();
        //    var 

        //    // Act
        //    var result = ObservationNotesHelper.GetDeletedNotes();

        //    // Assert
        //    Assert.IsType<List<String>>(result);
        //    Assert.Empty(result);
        //}
    }
}
