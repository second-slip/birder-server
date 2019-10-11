using Birder.Data.Model;
using Birder.Helpers;
using Birder.Tests.Controller;
using Birder.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Birder.Tests.Helpers
{
    public class ObservationAnalysisHelperTests
    {

        [Fact]
        public void GetFollowersUserNames_ReturnsEmptyCollection_WhenInputCollectionIsEmpty()
        {
            // Arrange
            var d = new ConservationStatus() { ConservationList = "Red" };
            var emptyInputCollection = GetTestObservations(7, GetTestBird(1, d));

            // Act
            var result = ObservationsAnalysisHelper.MapLifeList(emptyInputCollection);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<LifeListViewModel>>(result);
            Assert.Single(result);
        }

        private Bird GetTestBird(int birdId, ConservationStatus cs)
        {
            return new Bird() { BirdId = birdId, EnglishName = "Dodo", Species = "Dodo dodo"
            , PopulationSize = "Pop size", BirdConservationStatus = cs };
        }

        private IEnumerable<Observation> GetTestObservations(int length, Bird bird)
        {
            var observations = new List<Observation>();
            for (int i = 0; i < length; i++)
            {
                observations.Add(new Observation
                {
                    ObservationId = i,
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

                var t = new Bird()
                {
                    BirdId = 7,
                    EnglishName = "Test",
                    Species = "Test testus"
,
                    PopulationSize = "Pop size",
                    BirdConservationStatus = bird.BirdConservationStatus
                };
                observations.Add(new Observation
                {
                    ObservationId = 234,
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
                    Bird = t,
                    ApplicationUser = null,
                    ObservationTags = null
                });

            var ty = new Bird()
            {
                BirdId = 79,
                EnglishName = "Test2",
                Species = "Test testus2"
,
                PopulationSize = "Pop size",
                BirdConservationStatus = bird.BirdConservationStatus
            };
            observations.Add(new Observation
            {
                ObservationId = 222,
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
                Bird = ty,
                ApplicationUser = null,
                ObservationTags = null
            });

            return observations;
        }
    }
}
