using Birder.Data.Model;
using Birder.Helpers;
using Birder.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace Birder.Tests.Helpers
{
    public class ObservationAnalysisHelperTests
    {
        #region MapLifeList

        [Fact]
        public void MapLifeList_ReturnsNullReferenceException_WhenArgumentIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => ObservationsAnalysisHelper.MapLifeList(null));
            Assert.Equal("The observations collection is null (Parameter 'observations')", ex.Message);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(5, 20)]
        [InlineData(22, 7)]
        [InlineData(8, 12)]
        public void MapLifeList_ReturnsLifeListViewModelList_OnSuccess(int numberSpecies, int numberObs)
        {
            // Arrange
            var birds = GetTestBirdsList(numberSpecies);
            var testObservations = GetTestObservations(numberObs, birds);

            // Act
            var result = ObservationsAnalysisHelper.MapLifeList(testObservations);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<LifeListViewModel>>(result);
            Assert.Equal(numberSpecies, result.Count());
            foreach(var record in result)
            {
                Assert.Equal(numberObs, record.Count);
            }
        }

        [Fact]
        public void MapLifeList_ReturnsLifeListViewModelList_OnSuccessfulNullNestedObject()
        {
            // Arrange
            int numberSpecies = 3;
            int numberObs = 3;
            var birds = GetTestBirdsList(numberSpecies);
            var testObservations = GetTestObservations(numberObs, birds);
            foreach(var item in testObservations)
            {
                item.Bird.BirdConservationStatus = null;
            }

            // Act
            var result = ObservationsAnalysisHelper.MapLifeList(testObservations);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<LifeListViewModel>>(result);
            Assert.Equal(numberSpecies, result.Count());
            foreach (var item in result)
            {
                Assert.Equal(numberObs, item.Count);
                Assert.Null(item.ConservationStatus);
            }
        }


        #endregion


        #region MapTopObservations

        [Fact]
        public void MapTopObservations_ReturnsNullReferenceException_WhenObservationsArgumentIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => ObservationsAnalysisHelper.MapTopObservations(null, DateTime.Now));
            Assert.Equal("The observations collection is null (Parameter 'observations')", ex.Message);
        }

        [Theory]
        [InlineData(2, 2)]
        [InlineData(5, 20)]
        [InlineData(22, 7)]
        [InlineData(8, 12)]
        public void MapTopObservations_ReturnsLifeListViewModelList_OnSuccess(int numberSpecies, int numberObs)
        {
            // Arrange
            var birds = GetTestBirdsList(numberSpecies);
            var testObservations = GetTestObservations(numberObs, birds);

            // Act
            var result = ObservationsAnalysisHelper.MapTopObservations(testObservations, DateTime.Now);

            // Assert
            Assert.IsType<TopObservationsAnalysisViewModel>(result);
            
            foreach (var record in result.TopObservations)
            {
                var exp = testObservations.Where(x => x.BirdId == record.BirdId).Count();
                Assert.Equal(exp, record.Count);
            }

            foreach (var record in result.TopMonthlyObservations)
            {
                var exp = testObservations.Where(x => x.BirdId == record.BirdId).Count();
                Assert.Equal(exp, record.Count);
            }
        }

        #endregion


        private List<Bird> GetTestBirdsList(int length)
        {
            var conservationStatus = new ConservationStatus() { ConservationList = "Red" };
            var birds = new List<Bird>();
            
            for (int i = 0; i < length; i++)
            {
                birds.Add(GetTestBird(i, conservationStatus));
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
