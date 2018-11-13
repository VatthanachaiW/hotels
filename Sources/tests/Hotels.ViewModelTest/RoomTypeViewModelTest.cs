using System;
using Hotels.TestUtilities;
using Hotels.ViewModels;
using Xunit;

namespace Hotels.ViewModelTest
{
    public class RoomTypeViewModelTest
    {
        [Fact]
        public void PropertiesValidationShouldBeFail()
        {
            var roomType = new RoomTypeViewModel();
            Assert.NotEqual(0, ViewModelValidator.Validation(roomType).Count);
        }

        [Fact]
        public void PropertiesValidationShouldBePass()
        {
            var roomType = new RoomTypeViewModel
            {
                RoomTypeName = "Room Type"
            };

            Assert.Equal(0, ViewModelValidator.Validation(roomType).Count);
        }
    }
}