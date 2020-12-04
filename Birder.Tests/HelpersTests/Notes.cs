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
    public class Notes
    {
        [Theory, MemberData(nameof(NullArgumentTestData))]
        public void GetDeletedNotes_ReturnsNullReferenceException_WhenEitherOrBothArgumentaAreNull(List<ObservationNote> originalNotes, List<ObservationNoteDto> editedNotes)
        {
            // Act & Assert
            var ex = Assert.Throws<NullReferenceException>(() => ObservationNotesHelper.GetDeletedNotes(originalNotes, editedNotes));
            Assert.Equal("The notes collection is null", ex.Message);
        }





        // test helpers...
        public static IEnumerable<object[]> NullArgumentTestData
        {
            get
            {
                return new[]
                {
                    new object[] { new List<ObservationNote>(), null },
                    new object[] { null, null },
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
