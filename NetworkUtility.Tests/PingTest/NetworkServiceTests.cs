using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.Ping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetworkUtility.Tests.PingTest
{
    public class NetworkServiceTests
    {
        private readonly NetworkService _pingService;
        public NetworkServiceTests() 
        {
            //SUT
            _pingService = new NetworkService();
        }

        [Fact]
        public void NetworkService_SendPing_ReturnString()
        {
            //Arrange - variables, classes, mocks

            //Act
            var result = _pingService.SendPing();

            //Assert
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("Success: Ping Sent !");
            result.Should().Contain("Success", Exactly.Once());
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(2, 2, 4)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int expected) 
        {
            //Arrange

            //Act
            var pingResult = _pingService.PingTimeout(a, b);

            //Assert
            pingResult.Should().Be(expected);
            pingResult.Should().BeGreaterThanOrEqualTo(2);
            pingResult.Should().NotBeInRange(-10000, 0);
        }

        [Fact]
        public void NetworkService_LastPingDate_ReturnDate()
        {
            //Arrange - variables, classes, mocks

            //Act
            var result = _pingService.LastPingDate();

            //Assert
            result.Should().BeAfter(1.January(2010));
            result.Should().BeBefore(1.January(2030));

        }

        [Fact]
        public void NetworkService_GetPingOptions_ReturnObject()
        {
            //Arrange
            var expectedResult = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            //Act
            var result = _pingService.GetPingOptions();

            //Assert WARNING: Be careful
            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expectedResult);
            result.Ttl.Should().Be(1);
        }

        [Fact]
        public void NetworkService_MostRecentPings_ReturnsObject()
        {
            //Arrange
            var expectedResult = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            //Act
            var result = _pingService.MostRecentPings();

            //Assert WARNING: Be careful
            result.Should().BeOfType<PingOptions[]>();
            result.Should().ContainEquivalentOf(expectedResult);
            result.Should().Contain(x => x.DontFragment == true);
        }
    }
}
