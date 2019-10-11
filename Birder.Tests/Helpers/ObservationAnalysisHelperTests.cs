using Birder.Data.Model;
using Birder.Helpers;
using Birder.Tests.Controller;
using Birder.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Birder.Tests.Helpers
{
    public class ObservationAnalysisHelperTests
    {

        [Theory]
        [InlineData(2, 2)]
        [InlineData(5, 20)]
        [InlineData(22, 7)]
        [InlineData(8, 12)]
        public void GetFollowersUserNames_ReturnsEmptyCollection_WhenInputCollectionIsEmpty(int numberSpecies, int numberObs)
        {
            // Arrange
            var birds = GetTestBirdsList(numberSpecies);
            var emptyInputCollection = GetTestObservations(numberObs, birds);

            // Act
            var result = ObservationsAnalysisHelper.MapLifeList(emptyInputCollection);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<LifeListViewModel>>(result);
            Assert.Equal(numberSpecies, result.Count());
            foreach(var item in result)
            {
                Assert.Equal(numberObs, item.Count);
            }
        }

        private List<Bird> GetTestBirdsList(int length)
        {
            var d = new ConservationStatus() { ConservationList = "Red" };
            var birds = new List<Bird>();
            
            for (int i = 0; i < length; i++)
            {
                birds.Add(GetTestBird(i, d));
            };

            return birds;
        }

        private Bird GetTestBird(int i, ConservationStatus cs)
        {
            return new Bird() 
            { 
                BirdId = i, 
                EnglishName = "Name " + i.ToString(), 
                Species = "Species " + i.ToString(),
                PopulationSize = "Pop size",
                BirdConservationStatus = cs 
            };
        }

        private IEnumerable<Observation> GetTestObservations(int length, List<Bird> birds)
        {
            var observations = new List<Observation>();

            for (int i = 0; i < birds.Count; i++)
            {
                var bird = birds[i];

                for (int y = 0; y < length; y++)
                {
                    observations.Add(new Observation
                    {
                        ObservationId = y,
                        LocationLatitude = 0,
                        LocationLongitude = 0,
                        Quantity = 1,
                        NoteGeneral = "",
                        NoteHabitat = "",
                        NoteWeather = "",
                        NoteAppearance = "",
                        NoteBehaviour = "",
                        NoteVocalisation = "",
                        HasPhotos = false,
                        SelectedPrivacyLevel = PrivacyLevel.Public,
                        ObservationDateTime = DateTime.Now.AddDays(-4),
                        CreationDate = DateTime.Now.AddDays(-4),
                        LastUpdateDate = DateTime.Now.AddDays(-4),
                        ApplicationUserId = "",
                        BirdId = bird.BirdId,
                        Bird = bird,
                        ApplicationUser = null,
                        ObservationTags = null
                    });
                }
            }

            return observations;
        }
    }
}
